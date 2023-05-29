using UnityEngine;
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
    public float forwardSpeed = 5;
    float distanceTravelled;
    public float lanePosition;
    private float spacing = 2.5f; // Distance between each spawned object
    private PhotonView pv;
    private bool isFinished = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start() 
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }

        pv = GetComponent<PhotonView>();
        animator = transform.GetComponent<Animator>();
        pathCreator = GameObject.Find("PathCreator").GetComponent<PathCreator>();
        endOfPathInstruction = EndOfPathInstruction.Stop;
    }


    void FixedUpdate()
    {
        /*if (pathCreator != null)
         {
             distanceTravelled += forwardSpeed * Time.deltaTime;
             transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
             transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
         }*/

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

                // Offset the initial object's position based on the normal vector
                positionOnPath += -normal * spacing * lanePosition;

                transform.position = new Vector3(positionOnPath.x, transform.position.y, positionOnPath.z);

                // Calculate the rotation of the object based on the tangent
                transform.rotation = Quaternion.LookRotation(tangent, Vector3.up);
            }

            /*if (isFinished)
            {
                transform.Rotate(Vector3.up * 20 * Time.deltaTime, Space.World);
            }*/
        }
       
        
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.tag == "Finish")
            {
                forwardSpeed = 0;
                isFinished = true;
                JumpAnimation();
            }

            if (other.tag == "Obstacle")
            {
                pv.RPC("RPC_SlowDown", RpcTarget.All, null);
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
        forwardSpeed = 10;
        yield return new WaitForSeconds(3f);
        forwardSpeed = 5;
    }

    [PunRPC]
    public void RPC_SlowDown()
    {
        StartCoroutine(SlowDown());
    }

    IEnumerator SlowDown()
    {
        forwardSpeed = 2;
        yield return new WaitForSeconds(3f);
        forwardSpeed = 5;
    }
}
