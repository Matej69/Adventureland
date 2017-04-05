using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	void Start()
	{
        SoundManager.GetInstance().PlaySound(SoundManager.E_NON_BLOCK_SOUND.EXPLOSION);
        StartCoroutine(DestroyAfter(2.7f));
	}


    IEnumerator DestroyAfter(float _sec)
    {
        yield return new WaitForSeconds(_sec);
        Destroy(gameObject);
    }


}
