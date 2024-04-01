using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterNewLevel : MonoBehaviour
{
    public bool enteringLevel;
    public string newLevel;
    private bool playerNear;

    public bool isMenu;

    private void Start()
    {
        enteringLevel = false;
    }

    void Update()
    {
        if (!playerNear)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isMenu)
            {
                Destroy(GameObject.Find("Canvas"));
                Destroy(GameObject.Find("Gamer"));
            }

            if (int.TryParse(newLevel, out int levelIndex))
                SceneManager.LoadScene(levelIndex);
            else
                SceneManager.LoadScene(newLevel);
        }

        enteringLevel = true;
    }


    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<TextDisplay>().DisplayText();
            playerNear = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<TextDisplay>().UnDisplayText();
            playerNear = false;
        }
    }
}
