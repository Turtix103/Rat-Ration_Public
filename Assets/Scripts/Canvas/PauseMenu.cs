using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public CanvasManager levelManager;
    public bool paused;
    void Start()
    {
        StartCoroutine(BugFix());
    }

    private IEnumerator BugFix()
    {
        yield return new WaitForEndOfFrame();
        pauseMenu = GameObject.Find("Pause Menu");
        levelManager = GameObject.Find("Level Manager").GetComponent<CanvasManager>();
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (!InputHandler.inputAllowed)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused && !levelManager.isAnythingActive)
                Pause();
            else if (paused && levelManager.isAnythingActive)
                UnPause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        levelManager.isAnythingActive = true;
        paused = true;
        StopTime.Stop();
    }

    public void UnPause()
    {
        pauseMenu.SetActive(false);
        levelManager.isAnythingActive = false;
        paused = false;
        StopTime.Resume();
    }
}
