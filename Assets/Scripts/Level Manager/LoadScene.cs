using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string newLevel;

    public void SceneLoad()
    {
        if (newLevel == "Level_1" || newLevel == "MainMenu")
        {
            Destroy(GameObject.Find("Canvas"));
            Destroy(GameObject.Find("Gamer"));
        }

        StopTime.Resume();

        SceneManager.LoadScene(newLevel);
    }
}
