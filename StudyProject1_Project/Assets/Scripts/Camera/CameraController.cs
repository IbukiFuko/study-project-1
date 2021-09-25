using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //[SerializeField] private float cameraSmoothTime = 0.05f;     //相机缓动时间

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
    }

    private void Update()
    {
        Vector3 tmpModelEuler = model.transform.eulerAngles;

        //水平旋转
        playerHandle.transform.Rotate(Vector3.up, playerInput.JRight * horizontalSpeed * Time.deltaTime);
        //垂直旋转
        currentEulerX += playerInput.JUp * -verticalSpeed * Time.deltaTime;
        currentEulerX = Mathf.Clamp(currentEulerX, verticalMin, verticalMax);
        cameraHandle.transform.localEulerAngles = new Vector3(currentEulerX, 0, 0);
        //相机距离
        transform.localPosition = new Vector3(0, 0, -distance);

        model.transform.eulerAngles = tmpModelEuler;

        //_camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, transform.position, ref cameraDampVelocity, cameraSmoothTime);
        _camera.transform.position = transform.position;
        //_camera.transform.eulerAngles = transform.eulerAngles;
        _camera.transform.LookAt(cameraHandle.transform);
    }

    private void FixedUpdate()
    {

    }
}
