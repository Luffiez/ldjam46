using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    CanvasGroup cg;

    bool isPaused = false;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    public void TogglePause(InputAction.CallbackContext contex)
    {
        isPaused = !isPaused;

        if(isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        MusicManager.Instance.PauseSfx(true);
        Time.timeScale = 0;
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void Resume()
    {
        MusicManager.Instance.PauseSfx(false);
        Time.timeScale = 1;
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
    
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        Transition.instance.Menu();
        
    }
}
