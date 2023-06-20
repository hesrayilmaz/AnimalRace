using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StartLevel : MonoBehaviour
{
    [SerializeField] private GameObject startLevelPanel;
    [SerializeField] private PhotonView myPhotonView;

    private float timer;
    private float maxWaitTime = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            timer = maxWaitTime;
        else
            myPhotonView.RPC("RPC_RequestStartTimer", RpcTarget.MasterClient);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    AIManager[] AIplayers = GameObject.FindObjectsOfType<AIManager>();
                    foreach(AIManager manager in AIplayers)
                    {
                        manager.EnableMovement();
                    }
                }
                EnableLevel();
            }
        }
    }

    private void EnableLevel()
    {

        startLevelPanel.SetActive(false);
    }

    [PunRPC]
    private void RPC_RequestStartTimer(PhotonMessageInfo info)
    {
        myPhotonView.RPC("RPC_SendStartTimer", info.Sender, timer);
    }

    [PunRPC]
    private void RPC_SendStartTimer(float timeIn)
    {
        timer = timeIn;
    }
}
