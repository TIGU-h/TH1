using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button startButton;
    public Button quitButton;

    [Header("Settings UI")]
    public Slider volumeSlider;
    public Text difficultyText;
    public Button leftArrowButton;
    public Button rightArrowButton;

    private string[] difficulties = { "easy", "normal", "hard" };
    private int currentDifficulty = 1;

    void Start()
    {
        leftArrowButton.onClick.AddListener(() => ChangeDifficulty(-1));
        rightArrowButton.onClick.AddListener(() => ChangeDifficulty(1));
        volumeSlider.onValueChanged.AddListener(SetVolume);

        UpdateDifficultyText();
        LoadSettings();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }

    void ChangeDifficulty(int direction)
    {
        currentDifficulty = Mathf.Clamp(currentDifficulty + direction, 0, difficulties.Length - 1);
        UpdateDifficultyText();
        PlayerPrefs.SetInt("difficulty", currentDifficulty);
    }

    void UpdateDifficultyText()
    {
        difficultyText.text = difficulties[currentDifficulty];
    }

    void LoadSettings()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
        currentDifficulty = PlayerPrefs.GetInt("difficulty", 1);
        UpdateDifficultyText();
    }
}
