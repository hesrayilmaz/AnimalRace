using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    private float speed = 10;
    private Vector3 offset;
    private Vector3 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        newPosition = new Vector3(offset.x + player.position.x, offset.y + player.position.y, offset.z + player.position.z);
        //transform.position = Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);
        //transform.LookAt(player.position);
        transform.position = newPosition;
        //transform.rotation = target.rotation;
    }
}
