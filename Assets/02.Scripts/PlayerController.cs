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
    private bool isCrouch = false;
    private CapsuleCollider CapsuleCollider;

    //앉았을때 얼마나 앉을지
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;


    //몸체 선언
    [SerializeField]
    private Rigidbody myRigid;
    [SerializeField]
    private Camera theCamera;

    //카메라의 민감도
    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float crouchSpeed;



    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotation_X = 0f;

    // Start is called before the first frame update
    void Start()
    {

        CapsuleCollider = GetComponent<CapsuleCollider>();
       // theCamera = FindObjectOfType<Camera>();
        myRigid = GetComponent<Rigidbody>();
        //처음상태는 걷는상태
        applySpeed = walkSpeed;

        //초기화
        //originPosY = transform.position.y;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }



    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    //앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //앉는 동작 함수
    private void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
            //isCrouch = false;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
            //isCrouch = true;
        }

        //  theCamera.transform.localPosition =
        //    new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);

        StartCoroutine(CrouchCoroutine());
    }
    
    //앉는동작 부드럽게 실행
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;


        while (_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;

        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);

        // yield return new WaitForSeconds(1f);
    }

    //점프시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&isGround)
        {
            Jump();
        }
    }

    //지면 체크
    private void IsGround()
    {
        //
        //isGround=Physics.Raycast(transform.position,-transform.up)
        //isGround = Physics.Raycast(transform.position, Vector3.down, 1f)

        isGround = Physics.Raycast(transform.position, Vector3.down, CapsuleCollider.bounds.extents.y+0.1f);


    }

    //점프관련함수
    private void Jump()
    {
        //앉은상태에서 점프시 앉은상태 해제
        if (isCrouch)
            Crouch();

        myRigid.velocity = transform.up * jumpForce;
    }

    //달리기 시도
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

    //달리기 실행
    private void Running()
    {
        if (isCrouch)
            Crouch();

        isRun = true;
        applySpeed = runSpeed;

    }
    //달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    //움직임 실행
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
