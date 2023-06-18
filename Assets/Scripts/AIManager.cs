﻿using UnityEngine;
using PathCreation;
using Photon.Pun;
using System.Collections;

// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
public class AIManager : MonoBehaviour
{
    private PathCreator pathCreator;
    private Animator animator;
    public EndOfPathInstruction endOfPathInstruction;
    public int initialSpeed;
    private float forwardSpeed;
    float distanceTravelled;
    public float lanePosition;
    private float targetLanePosition;
    private float laneChangeSpeed = 1f;
    private float spacing = 2.5f; // Distance between each spawned object
    private PhotonView pv;
    private bool isFinished = false;
    public string nickName;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        pv = GetComponent<PhotonView>();
        animator = transform.GetComponent<Animator>();
        pathCreator = GameObject.Find("PathCreator").GetComponent<PathCreator>();
    }

    void Start() 
    {
        initialSpeed = Random.Range(10, 12);
        forwardSpeed = initialSpeed;
        nickName = "Player" + Random.Range(1, 100);

        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }

        endOfPathInstruction = EndOfPathInstruction.Stop;
        // Initialize the target lane position
        targetLanePosition = lanePosition;
    }


    void FixedUpdate()
    {
        if (pv.IsMine)
        {
            if (pathCreator != null)
            {
                RunAnimation();

                distanceTravelled += forwardSpeed * Time.deltaTime;
                Vector3 positionOnPath = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

                // Calculate the normalized tangent and normal vectors of the path
                Vector3 tangent = pathCreator.path.GetDirectionAtDistance(distanceTravelled);
                Vector3 normal = new Vector3(-tangent.z, 0f, tangent.x).normalized;

                // Calculate the new lane position using lerping
                lanePosition = Mathf.Lerp(lanePosition, targetLanePosition, laneChangeSpeed * Time.deltaTime);

                // Offset the initial object's position based on the normal vector
                positionOnPath += -normal * spacing * lanePosition;

                transform.position = new Vector3(positionOnPath.x, transform.position.y, positionOnPath.z);

                // Calculate the rotation of the object based on the tangent
                transform.rotation = Quaternion.LookRotation(tangent, Vector3.up);
            }

        }
          
    }

    public void ChangeLane(float newLanePosition)
    {
        // Set the target lane position for smooth transition
        targetLanePosition = newLanePosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            forwardSpeed = 0;
            isFinished = true;
            JumpAnimation();
        }

        else 
        {
            if (other.tag == "Obstacle")
            {
                pv.RPC("RPC_SlowDown", RpcTarget.All, null);
            }

            if (other.tag != "Ground")
            {
                int randomIndex = Random.Range(0, 4);
                float newLanePosition = (1.5f) - randomIndex;
                ChangeLane(newLanePosition);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
         if (other.gameObject.tag == "Ground")
         {
             transform.position = pathCreator.path.GetClosestPointOnPath(transform.position);
         }
    }

    private void RunAnimation()
    {
        animator.SetTrigger("Run");
    }

    private void JumpAnimation()
    {
        animator.SetTrigger("Jump");
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged() {
         distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    [PunRPC]
    public void RPC_SpeedUp()
    {
        StartCoroutine(SpeedUp());
    }

    IEnumerator SpeedUp()
    {
        forwardSpeed = initialSpeed*2;
        yield return new WaitForSeconds(3.5f);
        forwardSpeed = initialSpeed;
    }

    [PunRPC]
    public void RPC_SlowDown()
    {
        StartCoroutine(SlowDown());
    }

    IEnumerator SlowDown()
    {
        forwardSpeed = initialSpeed/2;
        yield return new WaitForSeconds(3.5f);
        forwardSpeed = initialSpeed;
    }
}
