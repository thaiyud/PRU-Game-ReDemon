using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start()
    {
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController3 playerController = PlayerController3.Instance;
            if (playerController != null)
            {
                // Move the player to this entrance's position
                playerController.transform.position = this.transform.position;

                // Set the camera to follow the player
                CameraController.Instance.SetPlayerCameraFollow();
                UIFade.Instance.FadeToClear();
            }
        }
    }
}
