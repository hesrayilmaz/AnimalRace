using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPointManager : MonoBehaviourPunCallbacks
{
    public static SpawnPointManager instance;
    private List<Vector3> spawnPositions;
    private List<int> availableIndices;

    [SerializeField] private PathCreator pathCreator;
    private float spacing = 2.5f; // Distance between each spawned character
    private int randomIndex;

    private const string AvailableIndicesKey = "AvailableIndices";
    private ExitGames.Client.Photon.Hashtable playerCustomProp = new ExitGames.Client.Photon.Hashtable();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        availableIndices = playerCustomProp[AvailableIndicesKey] as List<int>;

        if (spawnPositions == null || availableIndices == null) 
        {
            SetSpawnPositions();
            playerCustomProp[AvailableIndicesKey] = availableIndices;
            //PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { AvailableIndicesKey, availableIndices.ToArray() } });
        }
    }

    private void SetSpawnPositions()
    {
        spawnPositions = new List<Vector3>();
        availableIndices = new List<int>();

        Vector3 position = pathCreator.path.GetPoint(0);
        Vector3 tangent = pathCreator.path.GetDirection(0);
        Vector3 normal = new Vector3(-tangent.z, 0f, tangent.x).normalized;

        position += -normal * spacing * 1.5f;

        for (int i = 0; i < 4; i++)
        {
            spawnPositions.Add(position);
            position += normal * spacing; // Add spacing in the direction of the normal vector
            availableIndices.Add(i);
        }
        foreach (int idx in availableIndices)
        {
            Debug.Log(idx);
        }
    }

    public Vector3 GetRandomPosition()
    {
        availableIndices = playerCustomProp[AvailableIndicesKey] as List<int>;
        randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        Debug.Log("randomIndex " + randomIndex);
        availableIndices.Remove(randomIndex);

        playerCustomProp[AvailableIndicesKey] = availableIndices;

        return spawnPositions[randomIndex];
    }

    public int GetRandomIndex()
    {
        return randomIndex;
    }

    public void ResetLists()
    {
        spawnPositions = new List<Vector3>();
        availableIndices = new List<int>();
    }
}
