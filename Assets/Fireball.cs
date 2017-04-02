using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    float speed = 10;

	// Use this for initialization
	void Start () {

        StartCoroutine(DestroyAfter(20f));
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);	
	}



    IEnumerator DestroyAfter(float _sec)
    {
        yield return new WaitForSeconds(_sec);
        Destroy(gameObject);
    }


}
