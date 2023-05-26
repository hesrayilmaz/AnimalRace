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
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float forwardSpeed = 5f;
    Vector3 direction, addedPos;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        pathCreator = GameObject.Find("PathCreator").GetComponent<PathCreator>();
        fixedJoystick = GameObject.Find("Canvas").transform.Find("FixedJoystick").GetComponent<FixedJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            if (fixedJoystick.Vertical != 0 || fixedJoystick.Horizontal != 0)
            {
                WalkAnimation();
            }
            else
            {
                IdleAnimation();
            }
        }
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground" && pv.IsMine)
        {
            transform.position = pathCreator.path.GetClosestPointOnPath(transform.position);
        }
    }


    private void IdleAnimation()
    {
        animator.SetTrigger("Idle");
    }

    private void WalkAnimation()
    {
        animator.SetTrigger("Walk");
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
}
