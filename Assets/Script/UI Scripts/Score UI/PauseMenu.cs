using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private PlayerControls inputActions; // Input actions asset reference
    public GameObject pauseScene; // Reference to the pause menu GameObject
    public bool isPaused = false; // Flag to track the paused state

    private void Awake()
    {
        inputActions = new PlayerControls(); // Initialize the input actions
    }

    private void Start()
    {
        // Map the pause action to the TogglePause function
        inputActions.Pause.Keyboard.performed += _ => TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pauseScene.SetActive(true);  // Show pause menu
            Time.timeScale = 0;          // Stop game time
        }
        else
        {
            pauseScene.SetActive(false); // Hide pause menu
            Time.timeScale = 1;          // Resume game time
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
}
