using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public PlayerManager _player;

    public float Score;

    public Slider healthBar;
    public GameObject ImpactImage;
    public GameObject TimerObject;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    public AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
       scoreManagement();;
    }

    void scoreManagement()
    {
        Score += 20 * Time.deltaTime;

        scoreText.text = "Score: " + Score.ToString("F2");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Menu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("map");
    }
}
