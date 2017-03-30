using UnityEngine;
using System.Collections;

public class BlockPickupText : MonoBehaviour {

    GameObject player;
    Color col;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerStats>().gameObject;
        col = GetComponent<TextMesh>().color;

        StartCoroutine(DestroyAfter(3f));
    }
	
	// Update is called once per frame
	void Update () {
        LookTowardsPlayer();
        ReduceOpacity(0.7f);
        MoveUp(0.7f);
    }





    void LookTowardsPlayer()
    {
        transform.LookAt(player.transform);
    }

    void ReduceOpacity(float _speed)
    {
        Color col = GetComponent<TextMesh>().color;
        GetComponent<TextMesh>().color = new Color(col.r, col.g, col.b, col.a -= _speed * Time.deltaTime);

    }

    void MoveUp(float _speed)
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }



    public void SetText(string _text)
    {
        GetComponent<TextMesh>().text = _text;
    }



    IEnumerator DestroyAfter(float _sec)
    {
        yield return new WaitForSeconds(_sec);
        Destroy(gameObject);
    }
    


}
