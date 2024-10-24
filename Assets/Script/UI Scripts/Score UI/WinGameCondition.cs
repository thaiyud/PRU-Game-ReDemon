using Assets.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class WinGameCondition : MonoBehaviour
{
    private EnemyHealth bossHealth;
    public GameObject WinGameScene;
    public TMP_Text scoreText;
    public TMP_Text timeCounter;

    private bool wonGame = false;
    private void Start()
    {
        WinGameScene.SetActive(false);        
    }
    void Update()
    {
        if (bossHealth == null)
        {
            GameObject bossObject = GameObject.Find("Boss Dark");
            if (bossObject != null)
            {
                bossHealth = bossObject.GetComponent<EnemyHealth>();
            }
        }
        if (bossHealth != null && !wonGame)
        {            
            if (bossHealth.currentHealth <= 0)
            {
                WinGame();
            }
        }
    }
    private void WinGame()
    {
        wonGame = true;
        float score = ScoreManager.Instance.Score;
        score += bossHealth.score;
        TimeSpan finishTime = Timer.Instance.playingTime;

        scoreText.text = "Score: " + score;
        timeCounter.text = "Finish Time: " + finishTime.ToString("mm':'ss'.'ff");
        // Save score and finish time to JSON        
        JsonSave.SaveGame(ScoreManager.Instance.Score, Timer.Instance.playingTime);

        Time.timeScale = 0;
        WinGameScene.SetActive(true);
    }
}
