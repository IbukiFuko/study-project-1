using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CapsuleCollider col;
    
    [Space(10)]
    [Header("=====Values=====")]
    [SerializeField] private float speed = 2.4f;
    [SerializeField] private float runSpeed = 2.7f;
    [SerializeField] private float rotateTime = 0.3f;
    [SerializeField] private float speedupTime = 0.5f;
    [SerializeField] private float jumpVelocity = 5.0f;
    [SerializeField] private float rollVelocity = 3.0f;
    //[SerializeField] private float jabVelocity = 3.0f;    //后跳
    [SerializeField] private float rollOffset = 5.0f;

    [Space(10)]
    [Header("=====Friction Settings=====")]
    [SerializeField] private PhysicMaterial frictionOne;
    [SerializeField] private PhysicMaterial frictionZero;


    private Animator anim;      //动画控制器
    private Rigidbody rigid;    //刚体
    private Vector3 planarVec;  //位移向量
    private Vector3 thrustVec;  //冲量
    private bool canAttack;     //能否攻击
    private bool canJump;       //能否跳跃
    private bool canMove;       //能否位移

    private bool lockPlanar = false;    //是否锁死移动

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
        playerInput = GetComponent<PlayerInput>();
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
        anim.SetFloat("forward", 
            Mathf.Lerp(anim.GetFloat("forward"), (playerInput.IsRun ? 2.0f : 1.0f) * playerInput.DMag, speedupTime));

        if(rigid.velocity.magnitude > rollOffset)
        {
            anim.SetTrigger("roll");
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

        model.transform.forward = Vector3.Slerp(model.transform.forward, playerInput.DForward, rotateTime); //缓动旋转

        if (!lockPlanar)
        {
            //位移
            planarVec = canMove ? (playerInput.IsRun ? 2.0f : 1.0f) * playerInput.DMag * model.transform.forward : Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector3(Mathf.Lerp(rigid.velocity.x, planarVec.x * (playerInput.IsRun ? runSpeed : speed),speedupTime), 
            rigid.velocity.y, 
            Mathf.Lerp(rigid.velocity.z, planarVec.z * (playerInput.IsRun ? runSpeed : speed), speedupTime))
            + thrustVec;
        thrustVec = Vector3.zero;
    }

    //查询动画机状态
    private bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName); //获取索引值
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
        thrustVec = new Vector3(0, jumpVelocity, 0);
        playerInput.InputEnabled = false;
    }

    public void OnGroundEnter()
    {
        lockPlanar = false;
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

    public void OnAttackIdle()
    {
        lockPlanar = false;
        playerInput.InputEnabled = true;
        canJump = true;
        canMove = true;
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), 0.0f);
    }

    public void OnAttack1hAEnter()
    {
        lockPlanar = true;
        playerInput.InputEnabled = false;
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), 1.0f);
    }
}