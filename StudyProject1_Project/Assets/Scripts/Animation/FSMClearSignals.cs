using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//״̬����ʱ����źţ������δ���
public class FSMClearSignals : StateMachineBehaviour
{
    [SerializeField] private string[] clearAtEnter;
    [SerializeField] private string[] clearAtExit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach(var signal in clearAtEnter)
        {
            animator.ResetTrigger(signal);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        foreach(var signal in clearAtExit)
        {
            animator.ResetTrigger(signal);
        }
    }
}
