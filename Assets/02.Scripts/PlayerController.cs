using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;    
    //몸체 선언
    [SerializeField]
    private Rigidbody myRigid;
    [SerializeField]
    private Camera theCamera;

    //카메라의 민감도
    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotation_X = 0f;

    // Start is called before the first frame update
    void Start()
    {
       // theCamera = FindObjectOfType<Camera>();
        myRigid = GetComponent<Rigidbody>();
    }



    // Update is called once per frame
    void Update()
    {
        Move();
        CameraRotation();
    }

    private void Move()
    { 
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotation_X = _xRotation * lookSensitivity;
        currentCameraRotation_X += _cameraRotation_X;
        currentCameraRotation_X = Mathf.Clamp(currentCameraRotation_X ,- cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotation_X, 0f, 0f);
    }
}
