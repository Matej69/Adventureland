using UnityEngine;
using System.Collections;

public class PlayerToWorldInteraction : MonoBehaviour {

    public LayerMask groundMask;
    Camera cam;

    public float blockRayLength;

    MapManager mapManager;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
        mapManager = FindObjectOfType<MapManager>();
    }
	
	// Update is called once per frame
	void Update () {

        BlockInteractionHadler();

    }



    void BlockInteractionHadler()
    {
        Vector3 dir = cam.transform.forward;

        RaycastHit hitInfo;
        if(Physics.Raycast(cam.transform.position, dir, out hitInfo, blockRayLength, groundMask))
        {
            BlockObject blockScr = hitInfo.collider.gameObject.GetComponent<BlockObject>();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                blockScr.OnDestroyByPlayer();
                DestroyBlock(hitInfo.collider.gameObject);
            }
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3Int newPos = new Vector3Int(hitInfo.normal) + blockScr.posID;
                mapManager.CreateBlock(newPos, n_block.E_BLOCK.GRASS);
            }

            

        }
        //Debug.DrawRay(cam.transform.position, dir * blockRayLength, Color.red);        
    }

    void DestroyBlock(GameObject _block)
    {
        Vector3Int pos = _block.GetComponent<BlockObject>().posID;
        mapManager.CreateSurroundingBlocks(mapManager.chunk, pos);
        mapManager.DestroyBlock(_block, mapManager.chunk);
    }



}
