using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
Animator animator;

private int isRunningHash;
private int isWalkingHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        bool leftpressed = Input.GetKey("a");
        bool rightpressed = Input.GetKey("d");
        
        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        
        if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (!isRunning && (forwardPressed && runPressed))
        {
            animator.SetBool(isRunningHash, true);
        }
        if ( isRunning && (!forwardPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }
        if ( isWalking && (forwardPressed && rightpressed))
        {
            animator.SetBool("isRight", true);
        }
        if ( isWalking && (!forwardPressed || !rightpressed ))
        {
            animator.SetBool("isRight", false);
        }
        if ( isWalking && (forwardPressed && leftpressed))
        {
            animator.SetBool("isLeft", true);
        }
        if ( isWalking && (!forwardPressed || !leftpressed ))
        {
            animator.SetBool("isLeft", false);
        }
    }
}
