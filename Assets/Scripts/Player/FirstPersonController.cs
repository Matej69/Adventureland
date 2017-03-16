using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

    public GameObject CameraHandler;

    Camera camera;
    
    public float walkSpeed;
    public float runSpeed;
    float moveSpeed;

    public float rotSpeed;
    public float rotSmoothness;

    public float verticalRotationLimit;
    
    Vector3 velocity;
    Vector3 rotation;
    Vector3 targetRot;

    // Use this for initialization
    void Start () {
        camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        
        velocity = new Vector3(0, 0, 0);
        rotation = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {

        if (ShopOwner.IsShopCreated())
            return;

        SetSpeed();

        ChangeVelocity();
        ChangeRotation();

        ApplyDeltaTime();

        HandleCameraFollow();
        ApplyVelocity();
        ApplyRotation();
    }


    void ChangeVelocity() {
        float dirX = Input.GetAxis("Horizontal");
        float dirZ = Input.GetAxis("Vertical");
        velocity.x = moveSpeed * dirX;
        velocity.z = moveSpeed * dirZ;
    }
    void ChangeRotation() {
        float dirY = Input.GetAxis("Mouse X");
        float dirX = -Input.GetAxis("Mouse Y");

        targetRot += new Vector3(dirX, dirY, 0) * rotSpeed;
        //LIMIT VERTICAL ROTATION
        if (targetRot.x >= verticalRotationLimit)
            targetRot.x = verticalRotationLimit;
        if (targetRot.x <= -verticalRotationLimit)
            targetRot.x = -verticalRotationLimit;
        
        rotation = Vector3.Lerp(rotation, targetRot, rotSmoothness);

        
        
    }

    void HandleCameraFollow() {
        camera.transform.position = CameraHandler.transform.position;
        camera.transform.rotation = Quaternion.Euler(rotation * rotSpeed);
    }
    



    void ApplyDeltaTime() {
        velocity *= Time.deltaTime;
    }
    void ApplyVelocity() {
        transform.Translate(velocity);
    }
    void ApplyRotation() {
        transform.rotation = Quaternion.AngleAxis(rotation.y * rotSpeed, Vector3.up);
    }



    void SetSpeed() {
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = runSpeed;
        else
            moveSpeed = walkSpeed;
    }



}
