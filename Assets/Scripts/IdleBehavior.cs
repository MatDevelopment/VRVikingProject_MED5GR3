using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : StateMachineBehaviour
{
    [SerializeField]
    private int numberOfIdleAnimations;

    [SerializeField]
    private int numberOfBoredAnimations;

    [SerializeField]
    private int timeUntilBored = 10;

    [SerializeField]
    private float blendDuration = 1f;

    private int idleID;
    private int boredID;
    private int previousBoredID;
    private int currentAnimationID;

    private bool isBored = false;

    private float timeSinceAnimationStart;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleID = Random.Range(0, numberOfIdleAnimations);

        Debug.Log($"Start Animation ID: {idleID}");

        animator.SetFloat("IdleBlend", idleID);

        timeSinceAnimationStart = 0;
        
        isBored = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isBored == false)
        {
            timeSinceAnimationStart += Time.deltaTime;

            if (timeSinceAnimationStart > timeUntilBored && stateInfo.normalizedTime % 1 < 0.02f)
            {
                isBored = true;
                boredID = Random.Range(numberOfIdleAnimations, numberOfBoredAnimations + numberOfIdleAnimations);

                if (boredID == previousBoredID)
                {
                    boredID++;

                    if (boredID >= numberOfIdleAnimations + numberOfBoredAnimations)
                    {
                        boredID -= 2;

                        if (boredID < numberOfIdleAnimations)
                        {
                            boredID += 2;
                        }
                    }
                }

                currentAnimationID = boredID;
                Debug.Log($"Bored: {isBored}, Starting Animation: {boredID}");
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            isBored = false;
            timeSinceAnimationStart = 0;

            idleID = Random.Range(0, numberOfIdleAnimations);
            currentAnimationID = idleID;
            Debug.Log($"Bored: {isBored}, Starting Animation: {idleID}");
        }

        animator.SetFloat("IdleBlend", currentAnimationID, blendDuration, Time.deltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
