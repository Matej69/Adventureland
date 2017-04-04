using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
    
    private float maxHealth = 200;
    private float maxOxygen = 200;
    private float currentHealth;
    private float currentOxygen;

    //Timer reduceOxygenTimer;

    PlayerToWorldInteraction playerWorldInteraction;

    [HideInInspector]
    public bool invulnerable = false;

    // Use this for initialization
    void Start () {
        playerWorldInteraction = gameObject.GetComponent<PlayerToWorldInteraction>();
        //reduceOxygenTimer = new Timer(0.02f);

        SetHealth(maxHealth);
        SetOxygen(maxOxygen);
    }
	
	// Update is called once per frame
	void Update () {
        HandleOxygenRegeneration();
        HandleOxygenReducement();
        OnHealthZero();	
	}

    
    
    public void SetHealth(float _amount)
    {
        currentHealth = (_amount > maxHealth) ? maxHealth : _amount;
    }
    
    public float GetHealth()
    {
        return currentHealth;
    }

    
    public void SetOxygen(float _amount)
    {
        currentOxygen = (_amount > maxOxygen) ? maxOxygen : _amount;
    }
    
    public float GetOxygen()
    {
        return currentOxygen;
    }


    public void ReduceHealthBy(float _amount)
    {
        currentHealth -= _amount;
    }
    public void ReduceOxygenBy(float _amount)
    {
        currentOxygen -= _amount;
    }
    public void AddOxygen(float _amount)
    {
        currentOxygen += _amount;
    }


    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
    public void ResetOxygen()
    {
        currentOxygen = maxOxygen;
    }


    public bool IsHealthEmpty()
    {
        return (currentHealth <= 0);
    }
    public bool IsOxygenEmpty()
    {
        return (currentOxygen <= 0);
    }





    private void HandleOxygenRegeneration()
    {
        if (playerWorldInteraction.GetPlayerStandingLevel() == 0)
        {
            float amount = 50f * Time.deltaTime;
            amount = (currentOxygen + amount > maxOxygen) ? 0 : amount;
            AddOxygen(amount);
        }
    }



    private void HandleOxygenReducement()
    {        
        if (!Textbox.isTextboxActive && !invulnerable)
        {
            ReduceOxygenBy(GetOxygenReducement() * 10 * Time.deltaTime);
            if (IsOxygenEmpty())
            {
                ReduceHealthBy(50 * Time.deltaTime);
            }
        }
    }

    private float GetOxygenReducement()
    {
        return (playerWorldInteraction.GetPlayerStandingLevel() / (float)n_chunk.CHUNK_SIZE.Y);
    }


    private void OnHealthZero()
    {
        if (currentHealth <= 0)
        {
            if (FindObjectOfType<Boss>())
                FindObjectOfType<Boss>().ResetHealth();

            FindObjectOfType<Inventory>().EmptyBlockInventory();
            playerWorldInteraction.ResetPosition();
            ResetHealth();
            ResetOxygen();
            FindObjectOfType<LevelManager>().SetSceneState(LevelManager.E_GAME_STATE.DIGGING);
        }
        
    }



}
