using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void OnAnimatorMove()   //������Unity�Դ��Ķ�Ӧ����
    {
        SendMessageUpwards("OnUpdateRootMotion", (object)anim.deltaPosition);
    }
}