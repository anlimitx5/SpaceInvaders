using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public static InGameUI instance;
    public Joystick joystick;
    [SerializeField] private GameObject _defeatWindow;
    [SerializeField] private GameObject _victoryWindow;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _enemiesCounterText;
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _playAgainButton;

  
    private void Awake()
    {
        Singleton();
    }
    private void Start()
    {
        _tryAgainButton.onClick.AddListener(ReloadScene);
        _playAgainButton.onClick.AddListener(ReloadScene);
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
    private void ReloadScene()
    {
        SceneManager.LoadScene("GameScene");
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
    }
}
