using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [Header("====Player Input====")]
    [SerializeField] private IUserInput playerInput;

    [Header("====Player Settings====")]
    [SerializeField] private GameObject _camera;                //当前对象相机
    [SerializeField] private float horizontalSpeed = 100.0f;    //水平旋转速度
    [SerializeField] private float verticalSpeed = 100.0f;      //垂直旋转速度
    [SerializeField] private float currentEulerX = 20.0f;       //当前俯仰角
    [SerializeField] private float verticalMax = 60.0f;         //俯角上限
    [SerializeField] private float verticalMin = -60.0f;         //仰角上限
    [SerializeField] private float distance = 2.5f;             //相机距离
    [SerializeField] private float distanceSpeed = 2.0f;        //相机距离灵敏度
    [SerializeField] private float distanceMax = 5f;
    [SerializeField] private float distanceMin = 1.0f;
    //[SerializeField] private float cameraSmoothTime = 0.05f;     //相机缓动时间

    [Header("=====Lock On=====")]
    [SerializeField] private LockTarget lockTarget;
    [SerializeField] private Image lockDot;
    [SerializeField] private float maxLockDistance = 10.0f;

    private GameObject playerHandle;    //控制水平旋转
    private GameObject cameraHandle;    //控制俯仰角
    private GameObject model;           //模型

    private Vector3 cameraDampVelocity; //SmoothDamp中间值

    private void Awake()
    {
        //获取Handle
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        model = playerHandle.GetComponent<ActorController>().Model;
        _camera = Camera.main.gameObject;


        //锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;

        //消除锁定图标
        lockDot.enabled = false;
    }

    private void Update()
    {
        if(lockTarget == null || lockTarget.obj == null)
        {
            Vector3 tmpModelEuler = model.transform.eulerAngles;

            //水平旋转
            playerHandle.transform.Rotate(Vector3.up, playerInput.JRight * horizontalSpeed * Time.deltaTime);
            //垂直旋转
            currentEulerX += playerInput.JUp * -verticalSpeed * Time.deltaTime;
            currentEulerX = Mathf.Clamp(currentEulerX, verticalMin, verticalMax);
            cameraHandle.transform.localEulerAngles = new Vector3(currentEulerX, 0, 0);

            model.transform.eulerAngles = tmpModelEuler;
        }
        else
        {
            Vector3 tmpForward = lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0) - model.transform.position;
            tmpForward.y = 0;
            playerHandle.transform.forward = tmpForward;
            lockDot.transform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
        }

        if(lockTarget != null && Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > maxLockDistance)
        {
            lockTarget = null;
            lockDot.enabled = false;
        }

        //相机距离
        distance -= playerInput.JDistance * distanceSpeed;
        distance = Mathf.Clamp(distance, distanceMin, distanceMax);
        transform.localPosition = new Vector3(0, 0, -distance);


        //_camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, transform.position, ref cameraDampVelocity, cameraSmoothTime);
        _camera.transform.position = transform.position;
        //_camera.transform.eulerAngles = transform.eulerAngles;
        _camera.transform.LookAt(cameraHandle.transform);
    }

    public void LockUnLock()
    {
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), model.transform.rotation, LayerMask.GetMask("Enemy"));
        if(cols.Length == 0)
        {
            lockTarget = null;
        }
        else
        {
            foreach (var col in cols)
            {
                if (lockTarget != null && lockTarget.obj == col.gameObject)
                {
                    lockTarget = null;
                    break;
                }
                lockTarget = new LockTarget(col.gameObject);
                break;
            }
        }
        lockDot.enabled = lockTarget != null;
    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;

        public LockTarget(GameObject _obj)
        {
            obj = _obj;
            halfHeight = obj.GetComponent<Collider>().bounds.extents.y;
        }
    }

    public bool LockState
    {
        get
        {
            return this.lockTarget != null;
        }
    }
}
