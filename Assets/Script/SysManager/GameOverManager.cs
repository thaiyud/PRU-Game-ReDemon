using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
	public GameObject gameOverMenu;
	public GameObject chapterMenu;
	public GameObject sceneMenu;
	public GameObject pauseMenu;
	public GameObject winMenu;
	public string SceneToLoad;

    public void EnableGameMenu()
	{
		gameOverMenu.SetActive(true);
		Time.timeScale = 0;
		Timer.Instance.EndTimer();
	}
    private void OnEnable()
	{
		PlayerHealth.OnPlayerDeath += EnableGameMenu;
	}
	private void OnDisable()
	{
		PlayerHealth.OnPlayerDeath -= EnableGameMenu;
	}
	public void Restart()
    {
        GameObject player = GameObject.FindWithTag("Player");             
        Destroy(player);
		Timer.Instance.SaveTimerState();
        SceneManager.LoadScene(SceneToLoad);
		gameOverMenu.SetActive(false);
		pauseMenu.SetActive(false);		
		Time.timeScale = 1;		
	}
	public void ResetPlay()
	{
        GameObject player = GameObject.FindWithTag("Player");
        Destroy(player);       
		Destroy(winMenu);
		PlayerPrefs.DeleteAll();
        Timer.Instance.ResetTimer();
        Time.timeScale = 1;
		SceneManager.LoadScene(4, LoadSceneMode.Single);
        StartCoroutine(EnsureTimeFlowAfterSceneLoad());
    }

	private IEnumerator EnsureTimeFlowAfterSceneLoad()
	{
		// Wait until the end of the current frame to ensure scene load is complete
		yield return new WaitForEndOfFrame();

		// Ensure time is flowing after the scene has loaded
		Time.timeScale = 1;

		// If there’s a PauseMenu component, ensure it’s unpaused
		PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
		if (pauseMenu != null)
		{
			pauseMenu.isPaused = false;  // Unpause the game if necessary
		}
	}
	public void ChapterMenu()
	{
		gameOverMenu.SetActive(false);
		chapterMenu.SetActive(true);
	}
	public void LoadChapter(string buttonName)
	{
		switch (buttonName)
		{
			case "Chapter 1 Button":
				SceneManager.LoadScene("Scenes/Chapter1/1.1");
				break;
			case "Chapter 2 Button":
				SceneManager.LoadScene("Scenes/Chapter1/2.1");
				break;
			case "Chapter 3 Button":
				SceneManager.LoadScene("Scenes/Chapter3/Scene 3.1");
				break;
			default:
				Debug.LogError("Invalid chapter button name: " + buttonName);
				break;
		}
		chapterMenu.SetActive(false);
		sceneMenu.SetActive(true);
	}
}
