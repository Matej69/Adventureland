using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {

    GameObject player;

    float speed = 7.5f;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerToWorldInteraction>().gameObject;

        InitMesh();
        StartCoroutine(DestroyAfter(20f));

    }
	
	// Update is called once per frame
	void Update () {
        LookAtPlayer();
        MoveForward();


    }



    IEnumerator DestroyAfter(float _sec)
    {
        yield return new WaitForSeconds(_sec);
        Destroy(gameObject);
    }


    void LookAtPlayer()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }
    void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }




    void InitMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector2[] uvs = new Vector2[mesh.vertices.Length];

        //front
        uvs[0] = new Vector2(0f, 0f);
        uvs[1] = new Vector2(0.333f,0f);
        uvs[3] = new Vector2(0.333f,1f);
        uvs[2] = new Vector2(0f,1f);
        //top
        uvs[8] = new Vector2(0.666f, 0f);
        uvs[9] = new Vector2(1f, 0f);
        uvs[5] = new Vector2(1f, 1f);
        uvs[4] = new Vector2(0.666f, 1f);
        //back
        uvs[10] = new Vector2(0.333f,0f);
        uvs[11] = new Vector2(0.666f,0f);
        uvs[7] = new Vector2(0.666f, 1f);
        uvs[6] = new Vector2(0.333f,1f);
        //bottom
        uvs[12] = new Vector2(0.666f, 0f);
        uvs[14] = new Vector2(1f, 0f);
        uvs[13] = new Vector2(1f, 1f);
        uvs[15] = new Vector2(0.666f, 1f);
        //left
        uvs[16] = new Vector2(0.666f, 0f);
        uvs[17] = new Vector2(1f, 0f);
        uvs[19] = new Vector2(1f, 1f);
        uvs[18] = new Vector2(0.666f, 1f);
        //right
        uvs[20] = new Vector2(0.666f, 0f);
        uvs[21] = new Vector2(1f, 0f);
        uvs[23] = new Vector2(1f, 1f);
        uvs[22] = new Vector2(0.666f, 1f);

        mesh.uv = uvs;
    }


}
