using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PathCreation;
using Photon.Pun;
using Photon.Realtime;

public class ScoreboardManager : MonoBehaviour
{
    public static ScoreboardManager instance;

    [SerializeField] private PhotonView myPhotonView;
    [SerializeField] private PathCreator pathCreator;

    [SerializeField] private List<Transform> orderTransforms;
    public List<string> nickNameList = new List<string>();
    public List<GameObject> playerList = new List<GameObject>();
    public List<float> distanceList = new List<float>();


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            myPhotonView.RPC("SetPlayerAndDistanceList", RpcTarget.All, null);
            myPhotonView.RPC("SortDistanceList", RpcTarget.All, null);
            myPhotonView.RPC("UpdateScoreboard", RpcTarget.All, null);
        } 
    }

    [PunRPC]
    private void SetPlayerAndDistanceList()
    {
        foreach(string name in nickNameList)
        {
            Debug.Log(name);
        }
        distanceList = new List<float>();
        foreach (GameObject player in playerList)
        {
            float distance = pathCreator.path.GetClosestDistanceAlongPath(player.transform.position);
            distanceList.Add(distance);
        }
    }

    [PunRPC]
    private void SortDistanceList()
    {
        float tempDist = 0;
        string tempName = "";
        GameObject tempPlayer = null;

        for (int i = 0; i < distanceList.Count; i++)
        {
            for (int j = i + 1; j < distanceList.Count; j++)
            {
                if (distanceList[i] < distanceList[j]-0.1f)
                {
                    tempDist = distanceList[i];
                    distanceList[i] = distanceList[j];
                    distanceList[j] = tempDist;

                    tempName = nickNameList[i];
                    nickNameList[i] = nickNameList[j];
                    nickNameList[j] = tempName;

                    tempPlayer = playerList[i];
                    playerList[i] = playerList[j];
                    playerList[j] = tempPlayer;
                }
            }
        }
    }

    [PunRPC]
    private void UpdateScoreboard()
    {
        Debug.Log("nickNameList.Count: " + nickNameList.Count);
        for (int i = 0; i < nickNameList.Count; i++)
        {
            Debug.Log("nickNameList: " + nickNameList[i]);
        }

        for (int i = 0; i < orderTransforms.Count; i++)
        {
            orderTransforms[i].parent.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = nickNameList[i];
        }
    }

}
