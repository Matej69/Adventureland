using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

    enum E_SPRITE
    {
        NOTCH,
        HISENBERG
    }


    public GameObject prefab_fireBall;
    public GameObject prefab_enemy;

    public Sprite sprite_hisenberg;
    public Sprite sprite_notch;

    List<GameObject> connectedGameObjects = new List<GameObject>();    
    
    PlayerToWorldInteraction player;

    Timer shotTimer;
    Timer spawnZombieTimer;

    Vector3 targetPos;
    float movementSpeed = 1.4f;

    void Awake()
    {
        player = FindObjectOfType<PlayerToWorldInteraction>();
    }

    // Use this for initialization
    void Start() {
        OnFirstTimeSpawn();

        targetPos = transform.position;

        shotTimer = new Timer(2.5f);
        spawnZombieTimer = new Timer(3f);
    }

    // Update is called once per frame
    void Update() {

        transform.LookAt(player.transform);        

        HandleSpriteChange();

        if (!Textbox.isTextboxActive)
        {
            ShotHandler();
            SpawnZombieHandler();
            Move();
        }

    }




    void OnFirstTimeSpawn()
    {
        //enable textbox
        Textbox.GetInstance().EnableMessageBox(new List<TextboxMessageInfo>() {
            new TextboxMessageInfo("Boss : You finally arrived ............... MORTAL!"),
            new TextboxMessageInfo("Me : OMG Notch.... is.... is that you??!??"),
            new TextboxMessageInfo("Boss : My name is not Notch idiot.... it is HEISENBERG!!!!")
        });
        //player look at notch
        player.transform.LookAt(transform);
    }


    void ChangeSprite(E_SPRITE _spriteID)
    {
        GetComponent<SpriteRenderer>().sprite = (_spriteID == E_SPRITE.NOTCH) ? sprite_notch : sprite_hisenberg;
    }

    void HandleSpriteChange()
    {
        if (Textbox.isTextboxActive)
            ChangeSprite(E_SPRITE.NOTCH);
        else
            ChangeSprite(E_SPRITE.HISENBERG);
    }



    public void DestroyConnectedGO()
    {
        for(int i = connectedGameObjects.Count - 1; i >= 0; --i)
        {
            Destroy(connectedGameObjects[i]);
            connectedGameObjects.RemoveAt(i);
        }
    }
    


    public void SetInFrontOfPlayer()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y , player.transform.position.z);
        Vector3 dir = player.transform.TransformDirection(Vector3.forward).normalized;
        transform.Translate(dir * 20f, Space.World);
        targetPos = transform.position;
    }




    void Move()
    {
        if(IsTargetPosReached())
        {
            //set new random target point
            targetPos = new Vector3(Random.Range(-10, 10), transform.position.y, Random.Range(-4, 7));
        }        
        else
        {
            //move towards target pos
            transform.position = Vector3.Lerp(transform.position, targetPos, movementSpeed * Time.deltaTime);
        }
    }

    bool IsTargetPosReached()
    {
        if (Vector3.Distance(transform.position, targetPos) < 1f)
            return true;
        return false;
    }
       



    void ShotHandler()
    {
        shotTimer.Tick(Time.deltaTime);
        if (shotTimer.IsFinished())
        {
            ShotFireball();
            shotTimer.SetStartTime(Random.Range(12, 37) / 10);
            shotTimer.Reset();            
        }
    }

    void SpawnZombieHandler()
    {
        spawnZombieTimer.Tick(Time.deltaTime);
        if(spawnZombieTimer.IsFinished())
        {
            SpawnZombie();
            spawnZombieTimer.Reset();
        }
    }


    void ShotFireball()
    {
        GameObject fireball = (GameObject)Instantiate(prefab_fireBall, transform.position, Quaternion.identity);
        connectedGameObjects.Add(fireball);
        fireball.transform.LookAt(player.transform);
    }

    void SpawnZombie()
    {
        Vector3 pos = transform.position;
        pos.y += 150;
        pos.x += Random.Range(-500, 500) / 100;
        pos.z += Random.Range(0, 700) / 100;
        GameObject zombie = (GameObject)Instantiate(prefab_enemy, pos, Quaternion.identity);
        connectedGameObjects.Add(zombie);
    }


}
