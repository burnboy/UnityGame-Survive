using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    private bool isRun = false;
    private bool isGround = true;
    private CapsuleCollider CapsuleCollider;

    //몸체 선언
    [SerializeField]
    private Rigidbody myRigid;
    [SerializeField]
    private Camera theCamera;

    //카메라의 민감도
    [SerializeField]
    private float lookSensitivity;

    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotation_X = 0f;

    // Start is called before the first frame update
    void Start()
    {
       // theCamera = FindObjectOfType<Camera>();
        myRigid = GetComponent<Rigidbody>();
        //처음상태는 걷는상태
        applySpeed = walkSpeed;
    }



    // Update is called once per frame
    void Update()
    {
        TryRun();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {

            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();

        }

    }

    private void Running()
    {
        isRun = true;
        applySpeed = runSpeed;

    }
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }


    private void Move()
    { 
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
       // Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void CameraRotation()
    {
        //위아래 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotation_X = _xRotation * lookSensitivity;
        currentCameraRotation_X -= _cameraRotation_X;
        currentCameraRotation_X = Mathf.Clamp(currentCameraRotation_X ,- cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotation_X, 0f, 0f);
    }

    private void CharacterRotation()
    {
        //좌우 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0, _yRotation, 0) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}
