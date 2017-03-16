using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatsGUI : MonoBehaviour {

    PlayerStats playerStats;

    public RectTransform HealthBar;
    public RectTransform OxygenBar;
    

    // Use this for initialization
    void Start () {
        playerStats = FindObjectOfType<PlayerStats>();

    }
	
	// Update is called once per frame
	void Update () {
        SetHealthBar(playerStats.GetHealth());
        SetOxygenBar(playerStats.GetOxygen());
    }


    public void SetHealthBar(int _amount)
    {
        HealthBar.sizeDelta = (_amount >= 0) ? new Vector2(_amount, HealthBar.sizeDelta.y) : new Vector2(0, HealthBar.sizeDelta.y);
    }
    public void SetOxygenBar(int _amount)
    {
        OxygenBar.sizeDelta = (_amount >= 0) ? new Vector2(_amount, OxygenBar.sizeDelta.y) : new Vector2(0, OxygenBar.sizeDelta.y);
    }



}
