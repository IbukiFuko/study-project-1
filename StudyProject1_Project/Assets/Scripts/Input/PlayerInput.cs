using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField] private KeyCode keyC = KeyCode.Mouse0;         //����
    [SerializeField] private KeyCode keyD;

    [SerializeField] private KeyCode keyJUp = KeyCode.UpArrow;            //�ӽ���������
    [SerializeField] private KeyCode keyJDown = KeyCode.DownArrow;
    [SerializeField] private KeyCode keyJLeft = KeyCode.LeftArrow;
    [SerializeField] private KeyCode keyJRight = KeyCode.RightArrow;

    [Header("=====Output Signals=====")]
    [SerializeField] private float dUp = 0;     //directional up
    [SerializeField] private float dRight = 0;
    [SerializeField] private float dMag = 0;    //ǰ��ǿ��
    [SerializeField] private Vector3 dForward = new Vector3(0, 0, 1.0f);  //��ת����
    [SerializeField] private float jUp = 0;     //��ҡ�ˣ��������
    [SerializeField] private float jRight = 0;

    //��������
    [SerializeField] private bool isRun = false;    //�Ƿ���
    //������������
    [SerializeField] private bool isJump = false;   //�Ƿ���Ծ
    private bool lastJump = false;
    [SerializeField] private bool isAttack = false; //�Ƿ񹥻�
    private bool lastAttack = false;
    //˫����������


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

    private Vector2 SquareToCircle(Vector2 input)   //����������ת��ΪԲ�����꣬�����������1
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) * 0.5f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) * 0.5f);


        return output;
    }
}
