using UnityEngine;
using System.Collections;

public class Tool : Item {

    Animator animator;
    Timer swingAnimTimer;

    bool alreadyHitThisSwing = false;

    public LayerMask blockMask;
    public Transform toolDamagePoint;

    //BlockObject ref_blockToDestroy;


    // Use this for initialization
    void Start () {
        OnStart();
    }
	
	// Update is called once per frame
	void Update () {

        HandleTimerUpdate();
    }


    protected override void OnStart()
    {
        base.OnStart();

        animator = GetComponent<Animator>();

        durability = 5;
        swingAnimTimer = new Timer(0.4f);
    }

    public override void OnActionClick()
    {
        //only swing if we are not already swinging
        if (!swingAnimTimer.IsFinished())
            return;
        
        animator.SetBool("isSwinging", true);
        swingAnimTimer.Reset();        
    }


    void HandleTimerUpdate()
    {
        if (!swingAnimTimer.IsFinished())
        {
            swingAnimTimer.Tick(Time.deltaTime);            
            //if it's finished, set animation state back to IDLE
            if (swingAnimTimer.IsFinished())
            {
                animator.SetBool("isSwinging", false);
            }
            //if animation of tool is hitting block(in animation)  
            if (AnimInStateToDestroyBlock() && !alreadyHitThisSwing)
            {
                HandleOnRayHitBlock();
                alreadyHitThisSwing = true;
            }
        }
        else
        {
            alreadyHitThisSwing = false;
        }
        
    }

    bool AnimInStateToDestroyBlock()
    {
        return (swingAnimTimer.currentTime < swingAnimTimer.startTime / 2 && swingAnimTimer.currentTime > (swingAnimTimer.startTime / 2) - 0.1f);
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
            blockScr.OnDestroyByPlayer();
            DestroyBlock(hitInfo.collider.gameObject);
        }

        Debug.DrawRay(cam.transform.position, dir * blockRayLength, Color.red);
    }



    void DestroyBlock(GameObject _block)
    {
        Vector3Int pos = _block.GetComponent<BlockObject>().posID;
        mapManager.CreateSurroundingBlocks(mapManager.chunk, pos);
        mapManager.DestroyBlock(_block, mapManager.chunk);
    }




}
