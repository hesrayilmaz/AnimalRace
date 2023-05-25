using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private FixedJoystick fixedJoystick;
    [SerializeField] private Animator animator;
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float forwardSpeed = 2f;
    Vector3 direction, addedPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
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
        if (fixedJoystick.Vertical != 0 || fixedJoystick.Horizontal != 0)
        {
            addedPos = new Vector3(fixedJoystick.Horizontal * forwardSpeed * Time.fixedDeltaTime, 0, fixedJoystick.Vertical * forwardSpeed * Time.fixedDeltaTime);
            transform.position += addedPos;

            direction = (Vector3.forward * fixedJoystick.Vertical + Vector3.right * fixedJoystick.Horizontal);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * rotateSpeed);
        }
    }
}
