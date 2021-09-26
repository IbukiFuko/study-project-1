using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour    //������
{
    [Header("=====Output Signals=====")]
    [SerializeField] protected float dUp = 0;     //directional up
    [SerializeField] protected float dRight = 0;
    [SerializeField] protected float dMag = 0;    //ǰ��ǿ��
    [SerializeField] protected Vector3 dForward = new Vector3(0, 0, 1.0f);  //��ת����
    [SerializeField] protected float jUp = 0;     //��ҡ�ˣ��������
    [SerializeField] protected float jRight = 0;
    [SerializeField] protected float jDistance = 0; //�������

    //��������
    [SerializeField] protected bool isRun = false;    //�Ƿ���
    [SerializeField] protected bool isDefense = false;//�Ƿ����
    //������������
    [SerializeField] protected bool isJump = false;     //�Ƿ���Ծ
    [SerializeField] protected bool isAttack = false;   //�Ƿ񹥻�
    [SerializeField] protected bool isRoll = false;     //�Ƿ񷭹�
    //˫����������


    [Header("=====Others=====")]
    [SerializeField] protected float dead = 0.001f;       //����
    [SerializeField] protected float smoothTime = 0.1f;   //����ʱ��

    [SerializeField] protected bool inputEnabled = true;  //��������

    protected float targetDup;        //Ŀ��
    protected float targetDright;
    protected float velocityDup;      //�ٶ�
    protected float velocityDright;

    protected Vector2 SquareToCircle(Vector2 input)   //����������ת��ΪԲ�����꣬�����������1
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) * 0.5f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) * 0.5f);


        return output;
    }

    //��������
    public float DUp
    {
        get
        {
            return this.dUp;
        }
    }

    public float DRight
    {
        get
        {
            return this.dRight;
        }
    }

    public float DMag
    {
        get
        {
            return this.dMag;
        }
    }

    public Vector3 DForward
    {
        get
        {
            return this.dForward;
        }
    }

    public float JUp
    {
        get
        {
            return this.jUp;
        }
    }

    public float JRight
    {
        get
        {
            return this.jRight;
        }
    }

    public float JDistance
    {
        get
        {
            return this.jDistance;
        }
    }

    public bool IsRun
    {
        get
        {
            return this.isRun;
        }
    }

    public bool IsDefense
    {
        get
        {
            return this.isDefense;
        }
    }

    public bool IsJump
    {
        get
        {
            return this.isJump;
        }
    }

    public bool IsRoll
    {
        get
        {
            return this.isRoll;
        }
    }

    public bool IsAttack
    {
        get
        {
            return this.isAttack;
        }
    }

    public bool InputEnabled
    {
        get
        {
            return this.inputEnabled;
        }
        set
        {
            this.inputEnabled = value;
        }
    }
}
