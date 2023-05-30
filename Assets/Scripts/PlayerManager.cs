using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    private FixedJoystick fixedJoystick;
    private Animator animator;
    private PathCreator pathCreator;
    private PhotonView pv;
    private float rotateSpeed = 100f;
    private float forwardSpeed;
    private float initialSpeed = 10f;
    Vector3 direction, addedPos;
    private bool isFinished = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        pathCreator = GameObject.Find("PathCreator").GetComponent<PathCreator>();
        fixedJoystick = GameObject.Find("Canvas").transform.Find("FixedJoystick").GetComponent<FixedJoystick>();
    }

    // Start is called before the first frame update
    void Start()
    {
        forwardSpeed = initialSpeed;
        if (pv.IsMine)
        {
            AudioManager.instance.PlayRaceAudio();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            if (fixedJoystick.Vertical != 0 || fixedJoystick.Horizontal != 0)
            {
                RunAnimation();
            }
            else
            {
                IdleAnimation();
            }

            /*if (isFinished)
            {
                transform.Rotate(Vector3.up * 20 * Time.deltaTime, Space.World);
            }*/
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine)
        {
            if (other.tag == "Finish")
            {
                fixedJoystick.gameObject.SetActive(false);
                forwardSpeed = 0;
                isFinished = true;
                JumpAnimation();
                AudioManager.instance.PlayLevelEndAudio();
            }

            if(other.tag == "Obstacle")
            {
                pv.RPC("RPC_SlowDown", RpcTarget.All, null);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground" && pv.IsMine)
        {
            transform.position = pathCreator.path.GetClosestPointOnPath(transform.position);
        }
    }


    private void IdleAnimation()
    {
        animator.SetTrigger("Idle");
    }

    private void RunAnimation()
    {
        animator.SetTrigger("Run");
    }

    private void JumpAnimation()
    {
        animator.SetTrigger("Jump");
    }

    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            if (fixedJoystick.Vertical != 0 || fixedJoystick.Horizontal != 0)
            {
                addedPos = new Vector3(fixedJoystick.Horizontal * forwardSpeed * Time.fixedDeltaTime, 0, fixedJoystick.Vertical * forwardSpeed * Time.fixedDeltaTime);
                transform.position += addedPos;

                direction = (Vector3.forward * fixedJoystick.Vertical + Vector3.right * fixedJoystick.Horizontal);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * rotateSpeed);
            }
        }
    }

    [PunRPC]
    public void RPC_SpeedUp()
    {
        StartCoroutine(SpeedUp());
    }

    IEnumerator SpeedUp()
    {
        forwardSpeed = initialSpeed*2;
        yield return new WaitForSeconds(3f);
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
        yield return new WaitForSeconds(3f);
        forwardSpeed = initialSpeed;
    }
}
