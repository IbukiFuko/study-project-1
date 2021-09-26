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

    [SerializeField] private KeyCode keyRun = KeyCode.LeftShift;        //跑步
    [SerializeField] private KeyCode keyJump = KeyCode.Space;           //跳跃
    [SerializeField] private KeyCode keyAttack = KeyCode.Mouse0;        //攻击
    [SerializeField] private KeyCode keyDefense = KeyCode.Mouse1;       //防御
    [SerializeField] private KeyCode keyLockOn = KeyCode.LeftAlt;       //锁定

    [SerializeField] private KeyCode keyJUp = KeyCode.UpArrow;          //视角上下左右
    [SerializeField] private KeyCode keyJDown = KeyCode.DownArrow;
    [SerializeField] private KeyCode keyJLeft = KeyCode.LeftArrow;
    [SerializeField] private KeyCode keyJRight = KeyCode.RightArrow;

    [Header("=====Mouse Settings=====")]
    [SerializeField] private bool mouseEnable = true;
    [SerializeField] private float mouseSensitivityX = 10.0f;
    [SerializeField] private float mouseSensitivityY = 10.0f;

    private MyButton btnRun = new MyButton();
    private MyButton btnJump = new MyButton();
    private MyButton btnAttack = new MyButton();
    private MyButton btnDefense = new MyButton();
    private MyButton btnLockOn = new MyButton();


    void Update()
    {
        btnRun.Tick(Input.GetKey(keyRun));
        btnJump.Tick(Input.GetKey(keyJump));
        btnAttack.Tick(Input.GetKey(keyAttack));
        btnDefense.Tick(Input.GetKey(keyDefense));
        btnLockOn.Tick(Input.GetKey(keyLockOn));

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
            (mouseEnable ? Input.GetAxis("Mouse Y") * mouseSensitivityY : 0);
        jRight = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0) +
            (mouseEnable ? Input.GetAxis("Mouse X") * mouseSensitivityX : 0);

        //相机距离
            jDistance = Input.GetAxis("Mouse ScrollWheel");

        //跑步
        isRun = btnRun.IsLongPressing || btnRun.IsExtending;    //长按或者等待连击状态

        //翻滚
        isRoll = btnRun.OnTap;

        //跳跃
        isJump = btnJump.OnPressed;

        //攻击
        isAttack = btnAttack.OnPressed;

        //防御
        isDefense = btnDefense.IsPressing;

        //锁定
        isLockOn = btnLockOn.OnPressed;
    }

}
