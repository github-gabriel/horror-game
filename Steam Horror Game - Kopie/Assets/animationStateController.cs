using UnityEngine;
using Mirror;

public class animationStateController : NetworkBehaviour
{
    Animator animator;

    private int isRunningHash;
    private int isWalkingHash;
    private int isBackHash;
    private int isIdleHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isBackHash = Animator.StringToHash("isBackwards");
        isIdleHash = Animator.StringToHash("isIdle");
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            bool isRunning = animator.GetBool(isRunningHash);
            bool isWalking = animator.GetBool(isWalkingHash);
            bool isBackwards = animator.GetBool(isBackHash);
            bool isIdle = animator.GetBool(isIdleHash);
            bool forwardPressed = Input.GetKey("w");
            bool runPressed = Input.GetKey("left shift");
            bool leftpressed = Input.GetKey("a");
            bool rightpressed = Input.GetKey("d");
            bool backpressed = Input.GetKey("s");

            if (forwardPressed && backpressed)
            {
                animator.SetBool(isIdleHash, true);
            }
            else
            {
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
                if (!isBackwards && backpressed)
                {
                    animator.SetBool(isBackHash, true);
                }
            
                if ( isBackwards && !backpressed)
                {
                    animator.SetBool(isBackHash, false);
                }

                if (!isWalking && !forwardPressed && !isRunning && rightpressed)
                {
                    animator.SetBool("isRight", true);
                }
                if (!isWalking && !forwardPressed && !isRunning && !rightpressed)
                {
                    animator.SetBool("isRight", false);
                }
                if (!isWalking && !forwardPressed && !isRunning && leftpressed)
                {
                    animator.SetBool("isLeft", true);
                }
                if (!isWalking && !forwardPressed && !isRunning && !leftpressed)
                {
                    animator.SetBool("isLeft", false);
                }
            }
        }

    }
}
