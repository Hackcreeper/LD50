using GameFlow;
using UnityEngine;

namespace States
{
    public class JumpBehaviour : StateMachineBehaviour
    {
        private static readonly int JumpingAction = Animator.StringToHash("jumping");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Camera.main.GetComponent<FlowCamera>()?.player.StartJump();
            animator.SetBool(JumpingAction, false);
        }
    }
}
