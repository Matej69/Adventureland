using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatsGUI : MonoBehaviour {

    PlayerStats playerStats;
    PlayerToWorldInteraction playerWorldInteraction;

    public RectTransform HealthBar;
    public RectTransform OxygenBar;
    public Text UndergroundLevel;
    

    // Use this for initialization
    void Start () {
        playerStats = FindObjectOfType<PlayerStats>();
        playerWorldInteraction = FindObjectOfType<PlayerToWorldInteraction>();

    }
	
	// Update is called once per frame
	void Update () {
        SetHealthBar((int)playerStats.GetHealth());
        SetOxygenBar((int)playerStats.GetOxygen());
        UpdateUndergroundLevelText();
    }


    public void SetHealthBar(int _amount)
    {
        HealthBar.sizeDelta = (_amount >= 0) ? new Vector2(_amount, HealthBar.sizeDelta.y) : new Vector2(0, HealthBar.sizeDelta.y);
    }
    public void SetOxygenBar(int _amount)
    {
        OxygenBar.sizeDelta = (_amount >= 0) ? new Vector2(_amount, OxygenBar.sizeDelta.y) : new Vector2(0, OxygenBar.sizeDelta.y);
    }

    void UpdateUndergroundLevelText()
    {
        UndergroundLevel.text = playerWorldInteraction.GetPlayerStandingLevel() + "/" + n_chunk.CHUNK_SIZE.Y;
    }

}
