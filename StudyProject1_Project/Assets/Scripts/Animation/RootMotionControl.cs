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
    void OnAnimatorMove()   //重载了Unity自带的对应函数
    {
        SendMessageUpwards("OnUpdateRootMotion", (object)anim.deltaPosition);
    }
}
