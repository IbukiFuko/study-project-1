using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("=====Key Settings=====")]
    [SerializeField] private KeyCode keyUp = KeyCode.W;        //ǰ������
    [SerializeField] private KeyCode keyDown = KeyCode.S;
    [SerializeField] private KeyCode keyLeft = KeyCode.A;
    [SerializeField] private KeyCode keyRight = KeyCode.D;

    [SerializeField] private KeyCode keyA = KeyCode.LeftShift;      //�ܲ�
    [SerializeField] private KeyCode keyB = KeyCode.Space;          //��Ծ
    [SerializeField] private KeyCode keyC;
    [SerializeField] private KeyCode keyD;

    [Header("=====Output Signals=====")]
    [SerializeField] private float dUp = 0;     //directional up
    [SerializeField] private float dRight = 0;
    [SerializeField] private float dMag = 0;    //ǰ��ǿ��
    [SerializeField] private Vector3 dForward = new Vector3(0, 0, 1.0f);  //��ת����

    //��������
    [SerializeField] private bool isRun = false;    //�Ƿ���
    //��������
    [SerializeField] private bool isJump = false;     //�Ƿ���Ծ
    private bool lastJump = false;

    [Header("=====Others=====")]
    [SerializeField] private float dead = 0.001f;       //����
    [SerializeField] private float smoothTime = 0.1f;   //����ʱ��
    
    [SerializeField] bool inputEnabled = true;  //��������


    private float targetDup;        //Ŀ��
    private float targetDright;
    private float velocityDup;      //�ٶ�
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
        
    }
    
    void Update()
    {
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
    }

    private Vector2 SquareToCircle(Vector2 input)   //����������ת��ΪԲ�����꣬�����������1
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) * 0.5f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) * 0.5f);


        return output;
    }
}
