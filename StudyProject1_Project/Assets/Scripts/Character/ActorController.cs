using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private IUserInput playerInput;
    [SerializeField] private CapsuleCollider col;       //���ڸı��������
    [SerializeField] private CameraController camcon;
    
    [Space(10)]
    [Header("=====Values=====")]
    [SerializeField] private float speed = 2.4f;
    [SerializeField] private float runSpeed = 2.7f;
    [SerializeField] private float rotateTime = 0.3f;
    [SerializeField] private float speedupTime = 0.5f;
    [SerializeField] private float jumpVelocity = 5.0f;
    [SerializeField] private float rollVelocity = 3.0f;
    //[SerializeField] private float jabVelocity = 3.0f;    //����
    [SerializeField] private float rollOffset = 5.0f;       //���������ٶ��򷭹�

    [Space(10)]
    [Header("=====Friction Settings=====")]
    [SerializeField] private PhysicMaterial frictionOne;
    [SerializeField] private PhysicMaterial frictionZero;


    private Animator anim;      //����������
    private Rigidbody rigid;    //����
    private Vector3 planarVec;  //λ������
    private Vector3 thrustVec;  //����
    private bool canAttack;     //�ܷ񹥻�
    private bool canJump;       //�ܷ���Ծ
    private bool canMove;       //�ܷ�λ��

    private float lerpTarget;   //���Բ�ֵĿ��ֵ0||1
    private float lerpTargetStep = 0.1f;    //���Բ�ֵ���
    private float curMoveMulti = 1.0f;   //��ǰ�ƶ�����
    private float lerpMoveStep = 0.05f;  //���Բ�ֵ���

    private Vector3 animationDeltaPos;   //�����Դ�λ�ƴ���

    private bool lockPlanar = false;    //�Ƿ������ƶ�
    private bool trackDirection = false;    //�Ƿ�׷�ٷ���

    public GameObject Model
    {
        get
        {
            return this.model;
        }
    }

    void Awake()
    {
        anim = model.GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator is missing!");
        }
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if(input.enabled == true)
            {
                playerInput = input;
                break;
            }
        }
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput is missing!");
        }
        rigid = GetComponent<Rigidbody>();
        if (rigid == null)
        {
            Debug.LogError("Rigidbody is missing!");
        }
        col = GetComponent<CapsuleCollider>();
        if(col == null)
        {
            Debug.LogError("CapsuleColider is missing!");
        }
    }

    void Update()
    {
        curMoveMulti = Mathf.Lerp(curMoveMulti, playerInput.IsRun ? 2.0f : 1.0f, lerpMoveStep);
        //Forward

        anim.SetFloat("forward", 
            camcon.LockState ?
            curMoveMulti * transform.InverseTransformVector(playerInput.DForward).z: 
            Mathf.Lerp(anim.GetFloat("forward"), curMoveMulti * playerInput.DMag, speedupTime));
        //Right
        anim.SetFloat("right",
            camcon.LockState ? 
            curMoveMulti * transform.InverseTransformVector(playerInput.DForward).x :
            0);

        anim.SetBool("defense", playerInput.IsDefense);

        if (playerInput.IsLockOn)
        {
            camcon.LockUnLock();
        }

        if(playerInput.IsRoll || -rigid.velocity.y > rollOffset)
        {
            anim.SetTrigger("roll");
            canAttack = false;
        }

        if (playerInput.IsJump && canJump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }

        if (playerInput.IsAttack && CheckState("ground") && canAttack)
        {
            anim.SetTrigger("attack");
            canJump = false;
            canMove = false;
        }

        //��תģ��
        model.transform.forward = camcon.LockState ? 
            (trackDirection ? planarVec.normalized : transform.forward) : 
            Vector3.Slerp(model.transform.forward, playerInput.DForward, rotateTime); //������ת;

        if (!lockPlanar)
        {
            //λ��
            planarVec = canMove ? 
                curMoveMulti * playerInput.DMag * (camcon.LockState ? playerInput.DForward : model.transform.forward) : 
                Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        //�����Դ�λ��
        rigid.position += animationDeltaPos;
        animationDeltaPos = Vector3.zero;

        //������Ƶ�λ��
        rigid.velocity = new Vector3(Mathf.Lerp(rigid.velocity.x, planarVec.x * (playerInput.IsRun ? runSpeed : speed),speedupTime), 
            rigid.velocity.y, 
            Mathf.Lerp(rigid.velocity.z, planarVec.z * (playerInput.IsRun ? runSpeed : speed), speedupTime))
            + thrustVec;
        thrustVec = Vector3.zero;
    }

    //��ѯ������״̬
    private bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName); //��ȡ����ֵ
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }

    /// <summary>
    /// Message processing block
    /// </summary>
    public void OnJumpEnter()
    {
        //print("On Jump Enter");
        lockPlanar = true;
        trackDirection = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
        playerInput.InputEnabled = false;
    }

    public void OnGroundEnter()
    {
        lockPlanar = false;
        trackDirection = false;
        playerInput.InputEnabled = true;
        canAttack = true;
        col.material = frictionOne;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void IsGround()
    {
        anim.SetBool("isGround", true);
    }

    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }

    public void OnFallEnter()
    {
        lockPlanar = true;
        playerInput.InputEnabled = false;
    }

    public void OnRollEnter()
    {
        lockPlanar = true;
        trackDirection = true;
        thrustVec = new Vector3(0, rollVelocity, 0);
        playerInput.InputEnabled = false;
    }

    public void OnJabEnter()
    {
        lockPlanar = true;
        playerInput.InputEnabled = false;
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

    public void OnAttackIdleEnter()
    {
        lockPlanar = false;
        playerInput.InputEnabled = true;
        canJump = true;
        canMove = true;
        lerpTarget = 0;
    }

    public void OnAttackIdleUpdate()
    {
        LerpAttackLayerWeight();
    }

    public void OnAttack1hAEnter()
    {
        lockPlanar = true;
        playerInput.InputEnabled = false;
        lerpTarget = 1.0f;
    }

    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
        LerpAttackLayerWeight();
    }

    private void LerpAttackLayerWeight()
    {
        float curWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        curWeight = Mathf.Lerp(curWeight, lerpTarget, lerpTargetStep);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), curWeight);
    }

    public void OnAttack1hBUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hBVelocity");
    }

    public void OnAttack1hCUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hCVelocity");
    }

    public void OnUpdateRootMotion(object _deltaPos)
    {
        //if(CheckState("attack1hC", "attack")) //��Ҫ�����Դ�λ��ʱʹ��
        //{
        //    deltaPos += (deltaPos + (Vector3)_deltaPos) / 2.0f;
        //}
    }
}