using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    [SerializeField] private CapsuleCollider capcol;    //胶囊体

    private Vector3 point1;
    private Vector3 point2;
    private float radius;

    [SerializeField] private float offset = 0.3f;    //精度,避免错误识别未落地

    void Awake()
    {
        radius = capcol.radius;
    }

    void FixedUpdate()
    {
        //获取胶囊体的上下球心
        point1 = transform.position + transform.up * (radius - offset);
        point2 = transform.position + transform.up * capcol.height - transform.up * (radius + offset);

        //获取碰撞到的物体
        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));
        if (outputCols.Length != 0)
        {
            SendMessageUpwards("IsGround");
            //foreach (var col in outputCols)
            //{
            //    print(col.name);
            //}
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }
}
