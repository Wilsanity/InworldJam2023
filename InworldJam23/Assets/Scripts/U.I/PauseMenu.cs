using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    private PlayerInput playerInput;

    Controls playerControls;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerControls = new Controls();

        
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Pause.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
        playerControls.Player.Pause.performed -= OnPausePerformed;

    }


    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }



}
