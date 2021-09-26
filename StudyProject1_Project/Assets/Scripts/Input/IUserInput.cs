using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour    //抽象类
{
    [Header("=====Output Signals=====")]
    [SerializeField] protected float dUp = 0;     //directional up
    [SerializeField] protected float dRight = 0;
    [SerializeField] protected float dMag = 0;    //前进强度
    [SerializeField] protected Vector3 dForward = new Vector3(0, 0, 1.0f);  //旋转方向
    [SerializeField] protected float jUp = 0;     //右摇杆，控制相机
    [SerializeField] protected float jRight = 0;
    [SerializeField] protected float jDistance = 0; //相机距离

    //持续按键
    [SerializeField] protected bool isRun = false;    //是否奔跑
    [SerializeField] protected bool isDefense = false;//是否防御
    //单击触发按键
    [SerializeField] protected bool isJump = false;     //是否跳跃
    [SerializeField] protected bool isAttack = false;   //是否攻击
    [SerializeField] protected bool isRoll = false;     //是否翻滚
    //双击触发按键


    [Header("=====Others=====")]
    [SerializeField] protected float dead = 0.001f;       //死区
    [SerializeField] protected float smoothTime = 0.1f;   //缓冲时间

    [SerializeField] protected bool inputEnabled = true;  //开关输入

    protected float targetDup;        //目标
    protected float targetDright;
    protected float velocityDup;      //速度
    protected float velocityDright;

    protected Vector2 SquareToCircle(Vector2 input)   //将矩形坐标转换为圆形坐标，避免分量超过1
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) * 0.5f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) * 0.5f);


        return output;
    }

    //属性声明
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
