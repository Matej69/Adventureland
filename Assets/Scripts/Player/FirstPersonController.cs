using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FirstPersonController : MonoBehaviour {

    public GameObject CameraHandler;

    Camera camera;
    
    [Header("HOR MOVEMENT")]
    public float walkSpeed;
    public float runSpeed;
    float moveSpeed;

    [Header("VERT MOVEMENT")]
    public float gravity;

    [Header("ROTATION")]
    public float rotSpeed;
    public float rotSmoothness;
    public float verticalRotationLimit;

    [Header("RAYCAST MASK")]
    public LayerMask m_block;

    Vector3 velocity;
    Vector3 rotation;
    Vector3 targetRot;

    BoxCollider boxCollider;
    RaycastingInfo raycastingInfo;


    class RaycastingInfo
    {
        public enum E_SIDE { LEFT,RIGHT,TOP,BOT,FORWARD,BACKWARD }

        private int raysPerSide;
        public Dictionary<E_SIDE, Vector3> startPos;
        public List<GameObject> leftPoints, rightPoints, forwardPoints, backPoints, topPoints, bottomPoints;

        public RaycastingInfo(Vector3 _size, Transform tran)
        {
            //must be ^2 number
            raysPerSide = 12 * 12;
            int raysPerRow = (int)Mathf.Sqrt(raysPerSide);
            Vector3 dist = _size / ((raysPerRow) - 1);

            //start of opposite sides will be different for -/+ one axis that determines side of start position       
            Vector3 pos = tran.position;
            startPos = new Dictionary<E_SIDE, Vector3> {    { E_SIDE.LEFT,      new Vector3(pos.x-_size.x/2, pos.y - _size.y/2, pos.z - _size.z / 2) },
                                                            { E_SIDE.RIGHT,     new Vector3(pos.x+_size.x/2, pos.y - _size.y/2, pos.z - _size.z / 2) },
                                                            { E_SIDE.FORWARD,   new Vector3(pos.x-_size.x/2, pos.y - _size.y/2, pos.z + _size.z / 2) },
                                                            { E_SIDE.BACKWARD,  new Vector3(pos.x-_size.x/2, pos.y - _size.y/2, pos.z - _size.z / 2) },
                                                            { E_SIDE.BOT,       new Vector3(pos.x-_size.x/2, pos.y - _size.y/2, pos.z - _size.z / 2) },
                                                            { E_SIDE.TOP,       new Vector3(pos.x-_size.x/2, pos.y + _size.y/2, pos.z - _size.z / 2) }
            };
                        
            leftPoints = new List<GameObject>(); rightPoints = new List<GameObject>();
            forwardPoints = new List<GameObject>(); backPoints = new List<GameObject>();
            topPoints = new List<GameObject>(); bottomPoints = new List<GameObject>();

            //define position of all points on all sides
            CreateRayPointsObjects(dist, raysPerRow, tran, _size);
        }

        public void CreateRayPointsObjects(Vector3 dist, int raysPerRow, Transform tran, Vector3 _size)
        {
            //create gameobjects
            for (int row = 0; row < raysPerRow; ++row)
                for (int col = 0; col < raysPerRow; ++col)
                {
                    GameObject newRayPointObj = new GameObject("RAY POINT");
                    //for left/right
                    leftPoints.Add(Instantiate(newRayPointObj, new Vector3(startPos[E_SIDE.LEFT].x, startPos[E_SIDE.LEFT].y + dist.y * row, startPos[E_SIDE.LEFT].z + dist.z * col), Quaternion.identity) as GameObject);
                    rightPoints.Add(Instantiate(newRayPointObj, new Vector3(startPos[E_SIDE.RIGHT].x, startPos[E_SIDE.RIGHT].y + dist.y * row, startPos[E_SIDE.RIGHT].z + dist.z * col), Quaternion.identity) as GameObject);                    
                    //for forward/back
                    forwardPoints.Add(Instantiate(newRayPointObj, new Vector3(startPos[E_SIDE.FORWARD].x + dist.x * row, startPos[E_SIDE.FORWARD].y + dist.y * col, startPos[E_SIDE.FORWARD].z), Quaternion.identity) as GameObject);
                    backPoints.Add(Instantiate(newRayPointObj, new Vector3(startPos[E_SIDE.BACKWARD].x + dist.x * row, startPos[E_SIDE.BACKWARD].y + dist.y * col, startPos[E_SIDE.BACKWARD].z), Quaternion.identity) as GameObject); 
                    //for top/bot
                    bottomPoints.Add(Instantiate(newRayPointObj, new Vector3(startPos[E_SIDE.BOT].x + dist.x * row, startPos[E_SIDE.BOT].y, startPos[E_SIDE.BOT].z + dist.z * col), Quaternion.identity) as GameObject); 
                    topPoints.Add(Instantiate(newRayPointObj, new Vector3(startPos[E_SIDE.TOP].x + dist.x * row, startPos[E_SIDE.TOP].y, startPos[E_SIDE.TOP].z + dist.z * col), Quaternion.identity) as GameObject);
                    Destroy(newRayPointObj);

                    //set parents
                    leftPoints[leftPoints.Count - 1].transform.SetParent(tran);
                    rightPoints[rightPoints.Count - 1].transform.SetParent(tran);
                    forwardPoints[forwardPoints.Count - 1].transform.SetParent(tran);
                    backPoints[backPoints.Count - 1].transform.SetParent(tran);
                    bottomPoints[bottomPoints.Count - 1].transform.SetParent(tran);
                    topPoints[topPoints.Count - 1].transform.SetParent(tran);
                }
        }

    }




    // Use this for initialization
    void Start () {
        boxCollider = GetComponent<BoxCollider>();
        camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;

        raycastingInfo = new RaycastingInfo(boxCollider.size, transform);

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

        GetComponent<Rigidbody>().AddForce(velocity);

        //VerticalVelocityCheck(raycastingInfo);
        //HorizontalVelocityCheck(raycastingInfo);



        HandleCameraFollow();
        //ApplyVelocity();
        ApplyRotation();
    }


    void ChangeVelocity() { 
               
        float dirX = Input.GetAxis("Horizontal");
        float dirZ = Input.GetAxis("Vertical");
        velocity.x = moveSpeed * dirX * Time.deltaTime;
        velocity.z = moveSpeed * dirZ * Time.deltaTime;          
        
        //velocity.y -= gravity * Time.deltaTime;
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



    void VerticalVelocityCheck(RaycastingInfo raycastsInfo)
    {
        int dir = (int)Mathf.Sign(velocity.y);
        float skin = 0.002f;

        RaycastHit hitInfo;
        float minDistance = 777; //random value for unsigned minDistance (can not be null)
        bool isHitMinOnce = false;
        foreach (GameObject go in ( (dir == 1) ? raycastsInfo.topPoints : raycastsInfo.bottomPoints) )
        {
            Vector3 castPos = go.transform.position;
            bool isHit = Physics.Raycast(castPos, Vector3.up * dir, out hitInfo, Mathf.Abs(velocity.y), m_block);
            if (isHit)
            {
                isHitMinOnce = true;
                if (hitInfo.distance < minDistance)
                    minDistance = hitInfo.distance;                
            }
            Debug.DrawRay(castPos, Vector3.down * Mathf.Abs(velocity.y) * 10, Color.red);               
        }

        if (isHitMinOnce)
        {
            velocity.y = (minDistance - skin) * dir;            
        }    
    }

    void HorizontalVelocityCheck(RaycastingInfo raycastsInfo)
    {
        //***forward/backward
        {
            int dir = (int)Mathf.Sign(velocity.z);
            RaycastHit hitInfo;
            bool isHitMinOnce = false;
            foreach (GameObject go in ((dir == 1) ? raycastsInfo.forwardPoints : raycastsInfo.backPoints))
            {
                Vector3 castPos = go.transform.position;
                bool isHit = Physics.Raycast(castPos, transform.forward * dir, out hitInfo, Mathf.Abs(velocity.z) * 2, m_block);
                if (isHit)
                {
                    isHitMinOnce = true;
                }
                Debug.DrawRay(castPos, transform.forward * Mathf.Abs(velocity.z) * 10, Color.red);
            }
            Debug.Log(isHitMinOnce);
            if (isHitMinOnce)
            {
                velocity.z = -0.001f * dir;
            }

        }
        //***left/right
        {

        }
    }



    void HandleCameraFollow() {
        camera.transform.position = CameraHandler.transform.position;
        camera.transform.rotation = Quaternion.Euler(rotation * rotSpeed);
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
