using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ResetTrigger(string triggerName)
    {
        anim.ResetTrigger(triggerName);
    }
}
