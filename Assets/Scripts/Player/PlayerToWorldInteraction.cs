using UnityEngine;
using System.Collections;

public class PlayerToWorldInteraction : MonoBehaviour {

    PlayerStats playerStats;
  
    // Use this for initialization
    void Start() {
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update() {
    }


    //Map is created from +y to -y so everithing that is above starting layer of chunk will be considered lvl 0
    public int GetPlayerStandingLevel()
    {
        return ((int)transform.position.y > 0) ? 0 : Mathf.Abs((int)transform.position.y - 1);
    }

    public void ResetPosition()
    {
        Vector3 teleportToPos = GameObject.FindGameObjectWithTag("Teleporter").gameObject.transform.position;
        teleportToPos.y += 0.3f;
        transform.position = teleportToPos;          
    }



    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("LavaArea"))
        {
            FindObjectOfType<LevelManager>().SetSceneState(LevelManager.E_GAME_STATE.BOSS_BATTLE);
        }
        if (col.CompareTag("Fireball") || col.CompareTag("Zombie"))
        {
            playerStats.ReduceHealthBy( col.CompareTag("Fireball")?30f:15f );            
            SoundManager.GetInstance().PlaySound(SoundManager.E_NON_BLOCK_SOUND.HURT);
            Destroy(col.gameObject);
        }
    }

    

}
