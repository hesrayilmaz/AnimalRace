using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraMovement : MonoBehaviour
{
    private Transform player;
    public float speed = 5f;
    public Vector3 locationOffset;
    public Vector3 rotationOffset;
    Vector3 desiredPosition, smoothedPosition;
    Quaternion desiredrotation, smoothedrotation;
    private bool isStarted = false;

    private void Update()
    {
        if (!isStarted)
        {
            if (player == null)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject plyr in players)
                {
                    if (PhotonView.Get(plyr).IsMine)
                    {
                        player = plyr.transform;
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("player: " + player.name);
                locationOffset = transform.position - player.position;
                rotationOffset = transform.eulerAngles - player.eulerAngles;
                isStarted = true;
            }
        }
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }
    
    private void FollowPlayer()
    {
        if (player != null)
        {
            desiredPosition = player.position + player.rotation * locationOffset;
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
            transform.position = smoothedPosition;

            desiredrotation = player.rotation * Quaternion.Euler(rotationOffset);
            smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, speed * Time.deltaTime);
            transform.rotation = smoothedrotation;
        } 
    }
}
