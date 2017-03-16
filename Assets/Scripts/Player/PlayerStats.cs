using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
    
    private int maxHealth = 200;
    private int maxOxygen = 200;
    private int currentHealth;
    private int currentOxygen;

    Timer reduceOxygenTimer;


    // Use this for initialization
    void Start () {
        reduceOxygenTimer = new Timer(0.02f);
        SetHealth(maxHealth);
        SetOxygen(maxOxygen);
    }
	
	// Update is called once per frame
	void Update () {
        HandleOxygenReducement();	
	}

    
    
    public void SetHealth(int _amount)
    {
        currentHealth = (_amount > maxHealth) ? maxHealth : _amount;
    }
    
    public int GetHealth()
    {
        return currentHealth;
    }

    
    public void SetOxygen(int _amount)
    {
        currentOxygen = (_amount > maxOxygen) ? maxOxygen : _amount;
    }
    
    public int GetOxygen()
    {
        return currentOxygen;
    }


    public void ReduceHealthBy(int _amount)
    {
        currentHealth -= _amount;
    }
    public void ReduceOxygenBy(int _amount)
    {
        currentOxygen -= _amount;
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


    private void HandleOxygenReducement()
    {
        reduceOxygenTimer.Tick(Time.deltaTime);
        if(reduceOxygenTimer.IsFinished())
        {
            ReduceOxygenBy(1);
            if (IsOxygenEmpty())
                ReduceHealthBy(1);

            reduceOxygenTimer.Reset();
        }
    }


}
