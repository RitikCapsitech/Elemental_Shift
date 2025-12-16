using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject StartPanel;
    [SerializeField] private GameObject GameOverPanel;

    public GameObject pauseButton;
    public GameObject resumeButton;

    [SerializeField] public TextMeshProUGUI scoreDisplay;
    [SerializeField] public TextMeshProUGUI highScoreDisplay;
    [SerializeField] private TextMeshProUGUI finalScoreDisplay;

    [Header("Score Settings")]
    [SerializeField] private float scoreIncreaseRate = 2f; // Points per second

    private PlayerController playerController;
    private ObstacleSpawner obstacleSpawner;
    private CameraFollow cameraFollow;

    private Vector3 playerSpawnPosition;
    private bool isGameStarted = false;
    private bool isGameOver = false;
    private bool isPaused = false;

    private float currentScore = 0;
    private int highScore = 0;
    private const string HIGH_SCORE_KEY = "HighScore";

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {

        playerController = FindFirstObjectByType<PlayerController>();
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
        cameraFollow = FindFirstObjectByType<CameraFollow>();


        if (playerController != null)
            playerSpawnPosition = playerController.transform.position;


        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

        Time.timeScale = 0f;
        StartPanel.SetActive(true);
        GameOverPanel.SetActive(false);
        isGameStarted = false;
        isGameOver = false;

        currentScore = 0;
        UpdateScoreDisplay();

        if (playerController != null)
            playerController.enabled = false;
        if (obstacleSpawner != null)
            obstacleSpawner.enabled = false;
    }

    void Update()
    {

        if (isGameStarted && !isGameOver)
        {
            IncreaseScore();
        }
    }

    public void StartGame()
    {
        if (isGameStarted) return;

        StartPanel.SetActive(false);
        isGameStarted = true;
        isGameOver = false;


        currentScore = 0;
        UpdateScoreDisplay();

        if (playerController != null)
        {
            playerController.transform.position = playerSpawnPosition;
            playerController.enabled = true;
            playerController.ResetPlayer();
        }


        if (obstacleSpawner != null)
        {
            obstacleSpawner.enabled = true;
            obstacleSpawner.ResetSpawner();
        }


        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        // Check if current score is higher than high score
        if (Mathf.FloorToInt(currentScore) > highScore)
        {
            highScore = Mathf.FloorToInt(currentScore);
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
            PlayerPrefs.Save();
        }

        UpdateScoreDisplay();

        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);
    }



    public void Restart()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    private void IncreaseScore()
    {
        currentScore += (scoreIncreaseRate * Time.deltaTime);
        UpdateScoreDisplay();
    }


    private void UpdateScoreDisplay()
    {
        if (scoreDisplay != null)
            scoreDisplay.text = "Score: " + Mathf.FloorToInt(currentScore);
        if (finalScoreDisplay != null)
            finalScoreDisplay.text = "Final Score: " + Mathf.FloorToInt(currentScore);

        if (highScoreDisplay != null)
            highScoreDisplay.text = "High Score: " + highScore;
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (resumeButton != null) resumeButton.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(false);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (resumeButton != null) resumeButton.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true);
        isPaused = false;
    }



    public float GetCurrentScore() => currentScore;


    public int GetHighScore() => highScore;
    public bool IsGameStarted => isGameStarted;
    public bool IsGameOver => isGameOver;


}
