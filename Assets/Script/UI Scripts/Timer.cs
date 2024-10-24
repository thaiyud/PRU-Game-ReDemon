using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : Singleton<Timer>
{
    public TMP_Text timeCounter;

    public TimeSpan playingTime;
    private bool timerGoing;

    private float elapseTime;

    protected override void Awake()
    {
        base.Awake();
        timeCounter = GameObject.Find("Timer text").GetComponent<TMP_Text>();
        if (!Application.isPlaying)
        {
            PlayerPrefs.DeleteKey("SavedElapsedTime");
        }
    }
    private void Start()
    {
        // Restore the elapsed time if previously saved
        if (PlayerPrefs.HasKey("SavedElapsedTime"))
        {
            elapseTime = PlayerPrefs.GetFloat("SavedElapsedTime");
            playingTime = TimeSpan.FromSeconds(elapseTime);
            timeCounter.text = "Time: " + playingTime.ToString("mm':'ss'.'ff");
            PlayerPrefs.DeleteKey("SavedElapsedTime");
        }
        else
        {
            elapseTime = 0;
            timeCounter.text = "Time: 00:00:00";
        }

        timerGoing = true;
        StartCoroutine(UpdateTimer());
    }

    public void SaveTimerState()
    {
        PlayerPrefs.SetFloat("SavedElapsedTime", elapseTime);
        PlayerPrefs.Save();
    }

    public void BeginTimer()
    {
        timerGoing = true;        
        StartCoroutine(UpdateTimer());
    }
    public void EndTimer()
    {
        timerGoing = false;
        SaveTimerState();
    }
    public void ResetTimer()
    {
        elapseTime = 0;
        playingTime = TimeSpan.Zero;
        timeCounter.text = "Time: 00:00:00";     
        Time.timeScale = 1;

        BeginTimer();
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapseTime += Time.deltaTime;
            playingTime = TimeSpan.FromSeconds(elapseTime);
            string playingTimeStr = "Time: " + playingTime.ToString("mm':'ss'.'ff");            
            timeCounter.text = playingTimeStr;

            yield return null;
        }
    }
}

