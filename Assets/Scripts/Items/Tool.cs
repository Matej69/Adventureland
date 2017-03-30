using UnityEngine;
using System.Collections;

public class Tool : Item {
    
    Animator animator;
    Timer swingAnimTimer;
    

    bool attackAnimRunning = false;
    
    public LayerMask blockMask;
    public Transform toolDamagePoint;
    
    // Use this for initialization
    void Start () {
        OnStart();
    }
	
	// Update is called once per frame
	void Update () {        
        //HandleTimerUpdate();
    }


    protected override void OnStart()
    {
        base.OnStart();

        animator = GetComponent<Animator>();        

        //durability = 5;
        swingAnimTimer = new Timer(0.4f);
        swingAnimTimer.currentTime = 0;
    }

    public override void OnActionClick()
    {
        if (ShopOwner.IsShopCreated())
            return;

        //only swing if we are not already swinging       
        if (animator)
        {
            if (!IsAttackAnimRunning())
            {
                SetAttackAnimState(1);
            }
            else
            {
                SetAttackAnimState(0);
            }
        }
    }
    
    public void SetAttackAnimState(int _stateInt)
    {        
        bool state = (_stateInt == 0) ? false : true;
        animator.SetBool("isSwinging", state);
    }
    public bool IsAttackAnimRunning()
    {
        return animator.GetBool("isSwinging");
    }

    void HandleOnRayHitBlock()
    {
        Camera cam = Camera.main;
        Vector3 dir = cam.transform.forward;
        float blockRayLength = 2.2f;

        RaycastHit hitInfo;
        if (Physics.Raycast(cam.transform.position, dir, out hitInfo, blockRayLength, blockMask))
        {
            BlockObject blockScr = hitInfo.collider.gameObject.GetComponent<BlockObject>();            
            blockScr.OnToolHit(Item.GetInfo(itemType).power);
        }
    }
    
    




}
