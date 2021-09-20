using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("====Player Input====")]
    [SerializeField] private PlayerInput playerInput;

    [Header("====Player Settings====")]
    [SerializeField] private GameObject _camera;                //��ǰ�������
    [SerializeField] private float horizontalSpeed = 100.0f;    //ˮƽ��ת�ٶ�
    [SerializeField] private float verticalSpeed = 100.0f;      //��ֱ��ת�ٶ�
    [SerializeField] private float currentEulerX = 20.0f;       //��ǰ������
    [SerializeField] private float verticalMax = 40.0f;         //��������
    [SerializeField] private float verticalMin = -30.0f;         //��������
    [SerializeField] private float distance = 2.5f;             //�������
    [SerializeField] private float cameraSmoothTime = 0.05f;     //�������ʱ��

    private GameObject playerHandle;    //����ˮƽ��ת
    private GameObject cameraHandle;    //���Ƹ�����
    private GameObject model;           //ģ��

    private Vector3 cameraDampVelocity; //SmoothDamp�м�ֵ

    private void Awake()
    {
        //��ȡHandle
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        model = playerHandle.GetComponent<ActorController>().Model;
        _camera = Camera.main.gameObject;
    }

    private void FixedUpdate()
    {
        Vector3 tmpModelEuler = model.transform.eulerAngles;

        //ˮƽ��ת
        playerHandle.transform.Rotate(Vector3.up, playerInput.JRight * horizontalSpeed * Time.fixedDeltaTime);
        //��ֱ��ת
        currentEulerX += playerInput.JUp * -verticalSpeed * Time.fixedDeltaTime;
        currentEulerX = Mathf.Clamp(currentEulerX, verticalMin, verticalMax);
        cameraHandle.transform.localEulerAngles = new Vector3(currentEulerX, 0, 0);
        //�������
        transform.localPosition = new Vector3(0, 0, -distance);

        model.transform.eulerAngles = tmpModelEuler;

        _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, transform.position, ref cameraDampVelocity, cameraSmoothTime);
        _camera.transform.eulerAngles = transform.eulerAngles;
    }
}
