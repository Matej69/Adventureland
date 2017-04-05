using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public GameObject ref_Lava;
    public GameObject ref_Boss;

    public enum E_GAME_STATE
    {
        DIGGING,
        BOSS_BATTLE,
        BOSS_DEFEATED
    }

    public E_GAME_STATE state = E_GAME_STATE.DIGGING;
    
	// Use this for initialization
	void Start () {
        SetSceneState(E_GAME_STATE.DIGGING);
	
	}
	
	// Update is called once per frame
	void Update () {
        
	
	}




    //calls HandleStateChange with oldState and newState actions and THEN changes state to newState
    public void SetSceneState(E_GAME_STATE _newState)
    {
        HandleStateChange(_newState);
        state = _newState;
    }

    public void HandleStateChange(E_GAME_STATE _newState)
    {
        E_GAME_STATE oldState = state;
        switch(oldState)
        {
            case E_GAME_STATE.DIGGING: {  } break;
            case E_GAME_STATE.BOSS_BATTLE:
                {
                    ref_Lava.SetActive(false);
                    ref_Boss.GetComponent<Boss>().DestroyConnectedGO();
                } break;
            case E_GAME_STATE.BOSS_DEFEATED: { } break;
        }

        switch(_newState)
        {
            case E_GAME_STATE.DIGGING: { } break;
            case E_GAME_STATE.BOSS_BATTLE: {
                    ref_Lava.SetActive(true);
                    ref_Boss.GetComponent<Boss>().SetInFrontOfPlayer();
                } break;
            case E_GAME_STATE.BOSS_DEFEATED: { } break;
        }                        
    }






}
