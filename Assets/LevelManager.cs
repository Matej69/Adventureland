using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public GameObject ref_Lava;

    public enum E_GAME_STATE
    {
        START_SCREEN,
        DIGGING,
        BOSS_BATTLE,
        END_SCREEN      
    }

    public E_GAME_STATE state = E_GAME_STATE.DIGGING;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}





    public void InitStateScene(E_GAME_STATE _newState)
    {
        HandleStateChange(_newState);
        state = _newState;
    }

    public void HandleStateChange(E_GAME_STATE _newState)
    {
        E_GAME_STATE oldState = state;
        switch(oldState)
        {
            case E_GAME_STATE.START_SCREEN: { } break;
            case E_GAME_STATE.DIGGING: {  } break;
            case E_GAME_STATE.BOSS_BATTLE: { ref_Lava.SetActive(false); Debug.Log("old called"); } break;
            case E_GAME_STATE.END_SCREEN: { } break;
        }

        switch(_newState)
        {
            case E_GAME_STATE.START_SCREEN: { } break;
            case E_GAME_STATE.DIGGING: { } break;
            case E_GAME_STATE.BOSS_BATTLE: { ref_Lava.SetActive(true); Debug.Log("new called"); } break;
            case E_GAME_STATE.END_SCREEN: { } break;
        }                        
    }






}
