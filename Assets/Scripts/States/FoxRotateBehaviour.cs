using Entities;
using UnityEngine;

namespace States
{
    public class FoxRotateBehaviour : StateMachineBehaviour
    {
         // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponentInParent<Player>().FinishedStandingUp();   
        }
    }
}
