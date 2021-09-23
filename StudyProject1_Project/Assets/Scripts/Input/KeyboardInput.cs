using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KeyboardInput : IUserInput
{
    [Header("=====Key Settings=====")]
    [SerializeField] private KeyCode keyUp = KeyCode.W;        //ǰ������
    [SerializeField] private KeyCode keyDown = KeyCode.S;
    [SerializeField] private KeyCode keyLeft = KeyCode.A;
    [SerializeField] private KeyCode keyRight = KeyCode.D;

    [SerializeField] private KeyCode keyA = KeyCode.LeftShift;      //�ܲ�
    [SerializeField] private KeyCode keyB = KeyCode.Space;          //��Ծ
    [SerializeField] private KeyCode keyC = KeyCode.Mouse0;         //����
    [SerializeField] private KeyCode keyD;

    [SerializeField] private KeyCode keyJUp = KeyCode.UpArrow;            //�ӽ���������
    [SerializeField] private KeyCode keyJDown = KeyCode.DownArrow;
    [SerializeField] private KeyCode keyJLeft = KeyCode.LeftArrow;
    [SerializeField] private KeyCode keyJRight = KeyCode.RightArrow;

    void Start()
    {
        //�������
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

        //��ȡĿ��ֵ
        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        //�ر�����
        if (!inputEnabled)
        {
            targetDup = 0;
            targetDright = 0;
        }

        //ֵ����
        dUp = Mathf.SmoothDamp(dUp, targetDup, ref velocityDup, smoothTime);
        dRight = Mathf.SmoothDamp(dRight, targetDright, ref velocityDright, smoothTime);

        Vector2 tmpDAxis = SquareToCircle(new Vector2(dRight, dUp));

        dMag = Mathf.Min(1, Mathf.Sqrt(tmpDAxis.x * tmpDAxis.x + tmpDAxis.y * tmpDAxis.y));
        dForward = dMag > dead ? tmpDAxis.x * transform.right + tmpDAxis.y * transform.forward : dForward; //���ǿ��С�����������ַ��򲻱�


        //�ӽ��ƶ�
        //ˮƽ����תPlayerHandle��������תCameraHanle����������޸������z��
        jUp = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0) +
            Input.GetAxis("Mouse Y") * 10.0f;
        jRight = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0) +
            Input.GetAxis("Mouse X") * 10.0f;

        //�ܲ�
        isRun = Input.GetKey(keyA);

        //��Ծ
        bool tmpJump = Input.GetKey(keyB);
        if(!lastJump && tmpJump)    //��һ֡û����ͬʱ��һ֡����
        {
            isJump = true;
        }
        else
        {
            isJump = false;
        }
        lastJump = tmpJump;


        //����
        bool tmpAttack = Input.GetKey(keyC);
        if (!lastAttack && tmpAttack)    //��һ֡û������ͬʱ��һ֡������
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
