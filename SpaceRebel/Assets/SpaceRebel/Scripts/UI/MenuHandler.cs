using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script handle the Menu buttons in Menu scene.
/// </summary>

public class MenuHandler : MonoBehaviour
{
    #region SERIALIZE FILEDS

    [SerializeField] GameObject settingPanel;

    [SerializeField] Text scoreText;

    [SerializeField] Slider themeSlider;

    [SerializeField] Slider audioSlider;

    [SerializeField] AudioSource bgAudioSource;

    [SerializeField] SpriteRenderer bottomSpriteRenderer;

    [SerializeField] SpriteRenderer topSpriteRenderer;

    [Tooltip("Galaxy Theme sprites")]
    [SerializeField] Sprite galaxyBottomSprite;
    [SerializeField] Sprite galaxyTopSprite;

    [Tooltip("Nebula Theme Sprites")]
    [SerializeField] Sprite nebulaBottomSprite;
    [SerializeField] Sprite nebulaTopSprite;

    #endregion

    #region PRIVATE FIELDS

    Color galaxyColor = new Color(0.34f,0.34f,0.34f,1);

    GameController gameManager;

    int score;

    #endregion

    #region UNITY METHODS

    private void Start()
    {
        gameManager = GameController.GetInstance();

        gameManager.SetGameState(GAMESTATE.UI);

        score = PlayerPrefs.GetInt(GameController.highestScoreKey);

        scoreText.text = "Highest Score :- " + score;
 
    }

    #endregion

    void ChangeTheme(bool isNebula)
    {
        if(isNebula)
        {
            bottomSpriteRenderer.sprite = nebulaBottomSprite;

            topSpriteRenderer.sprite = nebulaTopSprite;

            bottomSpriteRenderer.color = Color.white;

            topSpriteRenderer.color = Color.white;
        }
        else
        {
            bottomSpriteRenderer.sprite = galaxyBottomSprite;

            topSpriteRenderer.sprite = galaxyTopSprite;

            bottomSpriteRenderer.color = galaxyColor;

            topSpriteRenderer.color = galaxyColor;
        }
    }

    #region BUTTONS ACTIONS

    // Button Action for play
    public void OnClickPlay()
    {
        gameManager.PlayAudio(AUDIOTYPE.BUTTON);

        SceneManager.LoadScene((int)GAMESTATE.GAMEPLAY);   
    }

    // Button Action for Settings
    public void OnClickSettings()
    {
        gameManager.PlayAudio(AUDIOTYPE.BUTTON);

        settingPanel.SetActive(true);

    }

    // Button Action for Ok
    public void OnClickOk()
    {
        gameManager.PlayAudio(AUDIOTYPE.BUTTON);

        gameManager.isAudioOff = audioSlider.value>0.5f ? true : false;

        gameManager.isNebulaTheme = themeSlider.value > 0.5f ? true : false;

        bgAudioSource.mute = gameManager.isAudioOff;

        ChangeTheme(gameManager.isNebulaTheme);

        settingPanel.SetActive(false);
    }

    // Button Action for Quit
    public void OnClickQuit()
    {
        gameManager.PlayAudio(AUDIOTYPE.BUTTON);

        Application.Quit();
    }

    #endregion
}
