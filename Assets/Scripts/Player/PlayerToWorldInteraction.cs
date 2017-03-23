using UnityEngine;
using System.Collections;

public class PlayerToWorldInteraction : MonoBehaviour {    

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }


    //Map is created from +y to -y so everithing that is above starting layer of chunk will be considered lvl 0
    public int GetPlayerStandingLevel()
    {
        return ((int)transform.position.y > 0) ? 0 : Mathf.Abs((int)transform.position.y - 1);
    }


    

}
