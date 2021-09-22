using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("=====Key Settings=====")]
    [SerializeField] private KeyCode keyUp = KeyCode.W;        //前后左右
    [SerializeField] private KeyCode keyDown = KeyCode.S;
    [SerializeField] private KeyCode keyLeft = KeyCode.A;
    [SerializeField] private KeyCode keyRight = KeyCode.D;

    [SerializeField] private KeyCode keyA = KeyCode.LeftShift;      //跑步
    [SerializeField] private KeyCode keyB = KeyCode.Space;          //跳跃
    [SerializeField] private KeyCode keyC = KeyCode.Mouse0;         //攻击
    [SerializeField] private KeyCode keyD;

    [SerializeField] private KeyCode keyJUp = KeyCode.UpArrow;            //视角上下左右
    [SerializeField] private KeyCode keyJDown = KeyCode.DownArrow;
    [SerializeField] private KeyCode keyJLeft = KeyCode.LeftArrow;
    [SerializeField] private KeyCode keyJRight = KeyCode.RightArrow;

    [Header("=====Output Signals=====")]
    [SerializeField] private float dUp = 0;     //directional up
    [SerializeField] private float dRight = 0;
    [SerializeField] private float dMag = 0;    //前进强度
    [SerializeField] private Vector3 dForward = new Vector3(0, 0, 1.0f);  //旋转方向
    [SerializeField] private float jUp = 0;     //右摇杆，控制相机
    [SerializeField] private float jRight = 0;

    //持续按键
    [SerializeField] private bool isRun = false;    //是否奔跑
    //单击触发按键
    [SerializeField] private bool isJump = false;   //是否跳跃
    private bool lastJump = false;
    [SerializeField] private bool isAttack = false; //是否攻击
    private bool lastAttack = false;
    //双击触发按键


    [Header("=====Others=====")]
    [SerializeField] private float dead = 0.001f;       //死区
    [SerializeField] private float smoothTime = 0.1f;   //缓冲时间
    
    [SerializeField] bool inputEnabled = true;  //开关输入


    private float targetDup;        //目标
    private float targetDright;
    private float velocityDup;      //速度
    private float velocityDright;

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

    public bool IsRun
    {
        get
        {
            return this.isRun;
        }
    }

    public bool IsJump
    {
        get
        {
            return this.isJump;
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

    void Start()
    {
        //锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            EditorApplication.isPaused = true;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            EditorApplication.isPlaying = false;
        }

        //获取目标值
        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        //关闭输入
        if (!inputEnabled)
        {
            targetDup = 0;
            targetDright = 0;
        }

        //值计算
        dUp = Mathf.SmoothDamp(dUp, targetDup, ref velocityDup, smoothTime);
        dRight = Mathf.SmoothDamp(dRight, targetDright, ref velocityDright, smoothTime);

        Vector2 tmpDAxis = SquareToCircle(new Vector2(dRight, dUp));

        dMag = Mathf.Min(1, Mathf.Sqrt(tmpDAxis.x * tmpDAxis.x + tmpDAxis.y * tmpDAxis.y));
        dForward = dMag > dead ? tmpDAxis.x * transform.right + tmpDAxis.y * transform.forward : dForward; //如果强度小于死区，保持方向不变


        //视角移动
        //水平面旋转PlayerHandle，俯仰旋转CameraHanle，相机距离修改相机的z轴
        jUp = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0) +
            Input.GetAxis("Mouse Y") * 10.0f;
        jRight = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0) +
            Input.GetAxis("Mouse X") * 10.0f;

        //跑步
        isRun = Input.GetKey(keyA);

        //跳跃
        bool tmpJump = Input.GetKey(keyB);
        if(!lastJump && tmpJump)    //上一帧没跳，同时这一帧跳了
        {
            isJump = true;
        }
        else
        {
            isJump = false;
        }
        lastJump = tmpJump;


        //攻击
        bool tmpAttack = Input.GetKey(keyC);
        if (!lastAttack && tmpAttack)    //上一帧没攻击，同时这一帧攻击了
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }
        lastAttack = tmpAttack;


    }

    private Vector2 SquareToCircle(Vector2 input)   //将矩形坐标转换为圆形坐标，避免分量超过1
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) * 0.5f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) * 0.5f);


        return output;
    }
}
