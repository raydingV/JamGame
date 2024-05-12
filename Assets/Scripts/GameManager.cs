using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class GameManager : MonoBehaviour
{
    public PlayerManager _player;

    public float Score;

    public Slider healthBar;
    public GameObject ImpactImage;
    public GameObject TimerObject;
    public TextMeshProUGUI timerText;
    
    private void Update()
    {
       scoreManagement();;
    }

    void scoreManagement()
    {
        Score += 20 * Time.deltaTime;
    }

    public void GameOver()
    {
        SceneManager.LoadScene(null);
    }
}
