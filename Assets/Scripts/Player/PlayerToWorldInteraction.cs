using UnityEngine;
using System.Collections;

public class PlayerToWorldInteraction : MonoBehaviour {

    [HideInInspector]
    public LayerMask groundMask;
    public float blockRayLength;

    //public GameObject ref_blockToDestroy;

    MapManager mapManager;
    Camera cam;

    // Use this for initialization
    void Start() {
        cam = Camera.main;
        mapManager = FindObjectOfType<MapManager>();
    }

    // Update is called once per frame
    void Update() {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FindObjectOfType<Tool>().OnActionClick();
        }

    }


    

}
