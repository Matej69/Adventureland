using UnityEngine;
using System.Collections;

public class DestroiedBlock : MonoBehaviour {

    public GameObject[] blocks;
    public float disappearingSpeed;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {        
        foreach(GameObject block in blocks)
        {
            //reduce opacity of color on material for each child block
            Color col = block.GetComponent<Renderer>().material.color;
            col.a -= disappearingSpeed * Time.deltaTime;
            block.GetComponent<MeshRenderer>().material.color = col;
            //if opacity of any child is less then 0 destroy them all
            if (col.a <= 0)
                Destroy(gameObject);
        }

        //speed of opacity reducing is exponential function
        disappearingSpeed += 0.0075f;

    }


    public void SetMaterial(Material _mat)
    {
        foreach (GameObject block in blocks)
        {
            block.GetComponent<MeshRenderer>().material = _mat;
        }
    }


}
