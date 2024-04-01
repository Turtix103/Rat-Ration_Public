using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public bool isDead;
    public GameObject deathScreen;
    public CanvasManager levelManager;
    void Start()
    {
        StartCoroutine(BugFix());
    }

    private IEnumerator BugFix()
    {
        yield return new WaitForEndOfFrame();
        isDead = false;
        deathScreen = GameObject.Find("Canvas/Death Screen");
        levelManager = GameObject.Find("Level Manager").GetComponent<CanvasManager>();
        deathScreen.SetActive(false);
    }

    public void KillPlayer()
    {
        isDead = true;
        deathScreen.SetActive(true);
        levelManager.isAnythingActive = true;
        StopTime.Stop();
    }

    public void RespawnPlayer()
    {
        Debug.Log("Respawned");
        isDead = false;
        StopTime.Resume();
        deathScreen.SetActive(false);
        levelManager.isAnythingActive = false;
        GameObject.Find("Gamer").GetComponent<PlayerHealth>().ResetHealth();
    }
}
