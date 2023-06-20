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
    private string[] nickNameList = new string[4];
    private GameObject[] playerList = new GameObject[4];
    private int[] playerIDList = new int[4];
    public List<float> distanceList = new List<float>();

    private string nickName;
    private bool isListsSet = false;

    // Dictionary to track if a player has finished the level
    private Dictionary<string, bool> playerFinishStatus = new Dictionary<string, bool>();

    private void Awake()
    {
        instance = this;
    }        
 

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (nickNameList[nickNameList.Length - 1] == null)
            {
                SetLists();
                return;
            }

            if (!isListsSet)
            {
                myPhotonView.RPC("RPC_SendLists", RpcTarget.All, nickNameList, playerIDList, playerFinishStatus);
                isListsSet = true;
            }
        }

        if (nickNameList[nickNameList.Length - 1] != null)
        {
            SetPlayerAndDistanceList();
            SortDistanceList();
            UpdateScoreboard();
        }
        
    }


    private void SetLists()
    {
        // Reset finish status for all players
        playerFinishStatus.Clear();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] AIplayers = GameObject.FindGameObjectsWithTag("AI");
        for (int i = 0; i < players.Length; i++)
        {
            GameObject player = players[i];
            playerList[i] = player;
            playerIDList[i] = player.GetComponent<PhotonView>().ViewID;
            //Debug.Log("playerList: " + player.name);
            //Debug.Log("playerID: " + playerIDList[i]);
            nickName = player.GetComponent<PhotonView>().Owner.NickName;
            //Debug.Log("player nickname: " + nickName);
            nickNameList[i] = nickName;

            // Set finish status as false for all players initially
            playerFinishStatus.Add(nickNameList[i], false);
        }

        for (int i = players.Length, j = 0; i < players.Length + AIplayers.Length; i++, j++)
        {
            GameObject player = AIplayers[j];
            playerList[i] = player;
            playerIDList[i] = player.GetComponent<PhotonView>().ViewID;
            //Debug.Log("playerList: " + player.name);
            //Debug.Log("playerID: " + playerIDList[i]);
            nickName = player.GetComponent<AIManager>().nickName;
            //Debug.Log("ai nickname: " + nickName);
            if (nickName != string.Empty)
            {
                nickNameList[i] = nickName;
                // Set finish status as false for all players initially
                playerFinishStatus.Add(nickNameList[i], false);
            }
        }
    }

    [PunRPC]
    private void RPC_SendLists(string[] masterNickNameList, int[] masterPlayerIDList, Dictionary<string,bool> masterPlayerFinishStatus)
    {
        nickNameList = masterNickNameList;
        playerIDList = masterPlayerIDList;
        playerFinishStatus = masterPlayerFinishStatus;

        for(int i=0; i<playerList.Length; i++)
        {
            playerList[i] = PhotonView.Find(playerIDList[i]).gameObject;
        }
    }


    private void SetPlayerAndDistanceList()
    {
        distanceList = new List<float>();
        foreach (GameObject player in playerList)
        {
            float distance = pathCreator.path.GetClosestDistanceAlongPath(player.transform.position);
            distanceList.Add(distance);
        }
    }


    private void SortDistanceList()
    {
        float tempDist = 0;
        string tempName = "";
        GameObject tempPlayer = null;

        for (int i = 0; i < distanceList.Count; i++)
        {
            // Check if the player has finished the level, if true, do not update their position in the scoreboard
            if (playerFinishStatus[nickNameList[i]])
                continue;

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


    private void UpdateScoreboard()
    {
        for (int i = 0; i < orderTransforms.Count; i++)
        {
            orderTransforms[i].parent.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = nickNameList[i];
        }
    }

    // Method to mark a player as finished
    public void MarkPlayerFinished(string playerNickName)
    {
        if (playerFinishStatus.ContainsKey(playerNickName))
        {
            playerFinishStatus[playerNickName] = true;
            Debug.Log("finished nickname " + playerNickName);
        }
    }

}
