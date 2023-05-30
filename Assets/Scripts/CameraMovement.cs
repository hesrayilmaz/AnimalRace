using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;
    public float speed = 5f;
    public Vector3 locationOffset;
    public Vector3 rotationOffset;
    Vector3 desiredPosition, smoothedPosition;
    Quaternion desiredrotation, smoothedrotation;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        locationOffset = transform.position - player.position;
        rotationOffset = transform.eulerAngles - player.eulerAngles;
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }
    
    private void FollowPlayer()
    {
        desiredPosition = player.position + player.rotation * locationOffset;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
        transform.position = smoothedPosition;
        
        desiredrotation = player.rotation * Quaternion.Euler(rotationOffset);
        smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, speed * Time.deltaTime);
        transform.rotation = smoothedrotation;
    }
}
