using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	void Start()
	{
        StartCoroutine(DestroyAfter(4f));
	}


    IEnumerator DestroyAfter(float _sec)
    {
        yield return new WaitForSeconds(_sec);
        Destroy(gameObject);

    }


}
