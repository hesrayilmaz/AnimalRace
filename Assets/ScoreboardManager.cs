using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PathCreation;

public class ScoreboardManager : MonoBehaviour
{
    public static ScoreboardManager instance;

    public List<GameObject> playerList;
    [SerializeField] private string[] playerNames;
    [SerializeField] private TextMeshProUGUI[] playerNameTexts;
    [SerializeField] private Transform[] playerOrders;

    public PathCreator pathCreator;
    private float[] distanceTravelledArray = new float[4];

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            // Get the distance travelled for each player based on their position
            Vector3 playerPosition = GetPlayerPosition(i);
            distanceTravelledArray[i] = pathCreator.path.GetClosestDistanceAlongPath(playerPosition);
        }

        // Compare and reorganize the distanceTravelledArray in descending order
        SortDistanceTravelledArrayDescending();

        // Use the sorted array as needed
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("Player " + (i + 1) + " Distance: " + distanceTravelledArray[i]);
        }
    }

    private Vector3 GetPlayerPosition(int playerIndex)
    {
        // Implement your logic to get the position of the player at the given index
        // You can use PhotonPlayerList or any other method to retrieve player positions
        // Replace this with your actual implementation
        return Vector3.zero;
    }

    private void SortDistanceTravelledArrayDescending()
    {
        // Sort the distanceTravelledArray in descending order
        System.Array.Sort(distanceTravelledArray, (a, b) => -a.CompareTo(b));
    }

}
