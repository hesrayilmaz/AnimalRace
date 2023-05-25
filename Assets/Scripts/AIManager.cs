using UnityEngine;
using PathCreation;

    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class AIManager : MonoBehaviour
    {
        private PathCreator pathCreator;
        private Animator animator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        public float lanePosition;
        private float spacing = 2.5f; // Distance between each spawned object
        

        void Start() 
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }

            animator = transform.GetComponent<Animator>();
            pathCreator = GameObject.Find("PathCreator").GetComponent<PathCreator>();
            endOfPathInstruction = EndOfPathInstruction.Loop;
        }

        void Update()
        {
            /*if (pathCreator != null)
             {
                 distanceTravelled += speed * Time.deltaTime;
                 transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                 transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
             }*/

            if (pathCreator != null)
            {
                WalkAnimation();

                distanceTravelled += speed * Time.deltaTime;
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
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Ground")
            {
                transform.position = pathCreator.path.GetClosestPointOnPath(transform.position);
            }
        }

        private void WalkAnimation()
        {
            animator.SetTrigger("Walk");
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
