using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textGameover;

    private readonly string formatScore = "score: {0}";

    private bool isGameover = false;

    private int score = 0;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            textScore.text = string.Format(formatScore, score);
        }
    }
    public bool IsGameOver { get; }
    private void Awake()
    {
        Score = 0;
    }
    private void Start()
    {
        textGameover.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameover && Input.GetKeyDown(KeyCode.E))
        {
            ++Score;
        }

        if (isGameover && Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void EndGame()
    {
        isGameover = true;
        textGameover.gameObject.SetActive(true);
        var backgroundLoops = FindObjectsOfType<BackgroundLoop>();
        for (int i = 0; i < backgroundLoops.Length; i++)
        {
            backgroundLoops[i].enabled = false;
        }
        var platforms = FindObjectsOfType<Platform>();
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].enabled = false;
        }
        var platformsSpawners = FindObjectsOfType<PlatformSpawner>();
        for (int i = 0; i < platformsSpawners.Length; i++)
        {
            platformsSpawners[i].enabled = false;
        }
    }
}
