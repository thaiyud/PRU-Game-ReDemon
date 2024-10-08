using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private float waitToLoadTime = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController3 playerController = other.gameObject.GetComponent<PlayerController3>();
        if (playerController != null)
        {            
            // Set the transition name in the scene management system
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneRoutine() );
        }
    }
    private IEnumerator LoadSceneRoutine()
    {
        while (waitToLoadTime >= 0f)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene( sceneToLoad );
    }
}
