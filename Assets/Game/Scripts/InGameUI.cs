using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public static InGameUI instance;
    public Joystick joystick;
    [SerializeField] private GameObject _defeatWindow;
    [SerializeField] private GameObject _victoryWindow;
    [SerializeField] private GameObject _currentLevelStartFrame;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _currentLevelText;
    [SerializeField] private Text _currentLevelStartText;
    [SerializeField] private Text _enemiesCounterText;
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _playNextLevelButton;
    [SerializeField] private Text _playNextLevelText;
    [SerializeField] private GameObject _thanks;


    private void Awake()
    {
        Singleton();
    }
    private void Start()
    {
        _tryAgainButton.onClick.AddListener(ReloadScene);
        _playNextLevelButton.onClick.AddListener(LoadNextLevel);
        SetLevelText();
        ChangeScore(GameVariables.fullScore);
        ChangeEnemiesCounter(LevelManager.instance.levels[GameVariables.currentLevel].targetEnemiesCount);
    }
    private void Singleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }
    private void LoadNextLevel()
    {
        GameVariables.currentLevel += 1;
        ReloadScene();
    }
    private void ReloadScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    private void SetLevelText()
    {
        _currentLevelStartFrame.SetActive(true);
        _currentLevelText.text = $"Level: {GameVariables.currentLevel}";
        _currentLevelStartText.text = $"Level: {GameVariables.currentLevel}";
    }
    public void ChangeScore(int score)
    {
        _scoreText.text = $"Score:\n{score}";
    }
    public void ChangeEnemiesCounter(int enemies)
    {
        _enemiesCounterText.text = $"Enemies left:\n{enemies}";
    }
    public void ChangeHealth(int currentHealth, int maximumHealth)
    {
        _healthText.text = $"Health:\n{currentHealth} / {maximumHealth}";
    }
    public void ShowDefeat()
    {
        _defeatWindow.SetActive(true);
    }
    public void ShowVictory()
    {
        _victoryWindow.SetActive(true);
        bool lastLevel = GameVariables.currentLevel == LevelManager.instance.levels.Length;
        _playNextLevelText.text = $"Play next {GameVariables.currentLevel + 1} level";
        _playNextLevelButton.gameObject.SetActive(!lastLevel);
        _thanks.SetActive(lastLevel);
    }
}
