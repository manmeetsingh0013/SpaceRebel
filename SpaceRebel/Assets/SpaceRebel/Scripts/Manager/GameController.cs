using UnityEngine;

public enum GAMESTATE
{
    UI = 0,
    GAMEPLAY = 1
}
public enum AUDIOTYPE
{
    BUTTON,
    PLAYERSHOT,
    EXPLOSION
}

/// <summary>
/// Scripts handle the game state and the sound.
/// </summary>

public class GameController : MonoBehaviour
{
    #region PUBLIC FIELDS

    [HideInInspector] public bool isAudioOff = false;

    [HideInInspector] public bool isNebulaTheme = false;

    public const string highestScoreKey = "HighestScore";

    #endregion

    #region PRIVATE FIELDS

    SoundManager soundManager;

    static GameController instance;

    AudioSource source;

    GAMESTATE currentGameState;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this);

        source = GetComponent<AudioSource>();

        soundManager = GetComponent<SoundManager>();
    }

    #endregion

    public static GameController GetInstance()
    {
        if (instance == null)
        {
            GameObject gameManager = new GameObject("GameManager");

            instance = gameManager.AddComponent<GameController>();
        }
        return instance;
    }

    #region PUBLIC METHODS

    public GAMESTATE GetGameState()
    {
        return currentGameState;
    }

    // Set the game current states
    public void SetGameState(GAMESTATE state)
    {
        currentGameState = state;
    }

    // Plays common audios
    public void PlayAudio(AUDIOTYPE type, bool isLoop = false, float volume = 1)
    {
        source.mute = isAudioOff;
        soundManager.PlayAudio(type, isAudioOff, isLoop, volume);
    }
    #endregion
}
