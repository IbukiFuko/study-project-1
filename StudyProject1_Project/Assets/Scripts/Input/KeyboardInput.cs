using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KeyboardInput : IUserInput
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

}
