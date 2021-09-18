using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    [SerializeField] private CapsuleCollider capcol;    //������

    private Vector3 point1;
    private Vector3 point2;
    private float radius;

    [SerializeField] private float offset = 0.3f;    //����,�������ʶ��δ���

    void Awake()
    {
        radius = capcol.radius;
    }

    void FixedUpdate()
    {
        //��ȡ���������������
        point1 = transform.position + transform.up * (radius - offset);
        point2 = transform.position + transform.up * capcol.height - transform.up * (radius + offset);

        //��ȡ��ײ��������
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
