using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerManager _player;

    public float Score;
    
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
