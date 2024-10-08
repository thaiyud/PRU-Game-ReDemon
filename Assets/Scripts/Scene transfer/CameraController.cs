using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    public void SetPlayerCameraFollow()
    {
        // Find the Cinemachine camera and set it to follow the player
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (cinemachineVirtualCamera != null && PlayerController3.Instance != null)
        {
            cinemachineVirtualCamera.Follow = PlayerController3.Instance.transform;
        }
    }
}
