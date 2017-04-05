using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {


    static private SoundManager soundManager;
    static public SoundManager GetInstance()
    {
        return ((soundManager != null) ? soundManager : soundManager = FindObjectOfType<SoundManager>());
    }




    public enum E_NON_BLOCK_SOUND
    {
        PLAYING_GAME,
        BOSS_BATTLE,
        END_GAME,
        SWING,
        LAVA,
        HURT,
        FIRE,
        EXPLOSION,
        HEALTH_UP,
        WALKING,
        ZOMBIE_DEATH,
        ZOMBIE_IDLE,
        SIZE
    }
    
    [System.Serializable]
    public struct SoundBlockListInfo
    {
        public n_block.E_BLOCK id;
        public List<AudioClip> clips;       
    }
    [System.Serializable]
    public struct SoundNonBlockInfo
    {
        public E_NON_BLOCK_SOUND id;
        public List<AudioClip> clips;
    }
    

    


    /*
    ************************* IMPORTANT ************************
    ALL "ListInfo" ITEMS MUST BE ASSIGNED IN ORDER OF ENUMS THAT THEIR ID'S ARE REPRESENTING,
    THIS WAY THERE IS NO NEED TO SEARCH FOR ITEM WITH ENUM_ID FROM LIST BUT INSTEAD, WE JUST USE LIST[ID]
    TO GET TARGETED SOUND
    */
    [Header("***ASSIGN IN ORDER OF THEIR ENUMS***")]  
      
    [Header("BLOCK SOUNDS")]
    public List<SoundBlockListInfo> sounds_block;
    [Header("BACKGROUND MUSIC")]
    public List<SoundNonBlockInfo> sounds_nonBlock;

    private AudioSource ref_backgroundAudioSource;
    E_NON_BLOCK_SOUND activeBackMusicID;  
    private LevelManager levelManager;


    // Use this for initialization
    void Start () {
        levelManager = FindObjectOfType<LevelManager>();
        ref_backgroundAudioSource = GetComponent<AudioSource>();
        SetBackgroundMusic(E_NON_BLOCK_SOUND.PLAYING_GAME, 0.2f);
    }
	
	// Update is called once per frame
	void Update () {
        HandleMusic();	
	}





    public void PlaySound<enumT>(enumT _ID)
    {
        GameObject go = new GameObject("SOUND");
        AudioSource controler = go.AddComponent<AudioSource>();
        controler.clip = GetSound(_ID);
        controler.Play();                                        
    }



    private void HandleMusic()
    {
        //handle digging music reset
        if (!ref_backgroundAudioSource.isPlaying && activeBackMusicID == E_NON_BLOCK_SOUND.PLAYING_GAME)
            SetBackgroundMusic(E_NON_BLOCK_SOUND.PLAYING_GAME, 0.2f);
        //handle boss music reset
        if (!ref_backgroundAudioSource.isPlaying && activeBackMusicID == E_NON_BLOCK_SOUND.BOSS_BATTLE)
            SetBackgroundMusic(E_NON_BLOCK_SOUND.BOSS_BATTLE, 0.7f);
        //handle end game music reset
        if (!ref_backgroundAudioSource.isPlaying && activeBackMusicID == E_NON_BLOCK_SOUND.END_GAME)
            SetBackgroundMusic(E_NON_BLOCK_SOUND.END_GAME, 0.5f);

        //handle music change according to game state
        if (levelManager.state == LevelManager.E_GAME_STATE.DIGGING && activeBackMusicID != E_NON_BLOCK_SOUND.PLAYING_GAME)
            SetBackgroundMusic(E_NON_BLOCK_SOUND.PLAYING_GAME, 0.2f);
        if (levelManager.state == LevelManager.E_GAME_STATE.BOSS_BATTLE && activeBackMusicID != E_NON_BLOCK_SOUND.BOSS_BATTLE)
            SetBackgroundMusic(E_NON_BLOCK_SOUND.BOSS_BATTLE, 0.7f);
        if (levelManager.state == LevelManager.E_GAME_STATE.BOSS_DEFEATED && activeBackMusicID != E_NON_BLOCK_SOUND.END_GAME)
            SetBackgroundMusic(E_NON_BLOCK_SOUND.END_GAME, 0.5f);

    }

    private void SetBackgroundMusic(E_NON_BLOCK_SOUND _ID, float _vol)
    {
        activeBackMusicID = _ID;
        ref_backgroundAudioSource.clip = GetSound(_ID);
        ref_backgroundAudioSource.volume = _vol;
        ref_backgroundAudioSource.Play();
    }




    //************************* GET SOUND ******************************
    private AudioClip GetSound<enumT>(enumT _ID)
    {        
        if (IsSameType(_ID, n_block.E_BLOCK.SIZE))
            return GetBlockSound( (n_block.E_BLOCK)System.Convert.ChangeType(_ID, typeof(n_block.E_BLOCK)) );
        else if (IsSameType(_ID, E_NON_BLOCK_SOUND.SIZE))
            return GetNonBlockSound((E_NON_BLOCK_SOUND)System.Convert.ChangeType(_ID, typeof(E_NON_BLOCK_SOUND)));
        
        Debug.LogError("Trying to get SOUND with parameter of TYPE = " + _ID.GetType() + " That does not exist");
        return null;
    }
           

    private AudioClip GetBlockSound(n_block.E_BLOCK _blockID)
    {
        int clipListID = (int)_blockID;
        if (clipListID < sounds_block.Count)
        {
            int numOfSounds = sounds_block[clipListID].clips.Count;
            int rand = Random.Range(0, numOfSounds);
            return sounds_block[clipListID].clips[rand];
        }
        else
        {
            Debug.LogError("Trying to get SOUND of TYPE = "+ _blockID.GetType() +" with ID = " + _blockID + " That does not exist");
            return null;
        }
    }
    private AudioClip GetNonBlockSound(E_NON_BLOCK_SOUND _soundID)
    {
        int clipListID = (int)_soundID;
        if (clipListID < sounds_nonBlock.Count)
        {
            int numOfSounds = sounds_nonBlock[clipListID].clips.Count;
            int rand = Random.Range(0, numOfSounds);
            return sounds_nonBlock[clipListID].clips[rand];
        }
        else
        {
            Debug.LogError("Trying to get SOUND of TYPE = " + _soundID.GetType() + " with ID = " + _soundID + " That does not exist");
            return null;
        }
    }


    //**************************CHECKING FOR SAME TYPE**************************
    private bool IsSameType<T1, T2>(T1 _t1, T2 _t2)
    {
        if (_t1.GetType() == _t2.GetType())
            return true;
        return false;
    }





}
