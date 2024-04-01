using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    public GameObject text;
    public bool shouldTriggerBySelf;

    void Start()
    {
        text = gameObject.GetComponentInChildren<TextMeshPro>().gameObject;
        text.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shouldTriggerBySelf)
            return;

        if (collision.gameObject.CompareTag("Player"))
            DisplayText();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (shouldTriggerBySelf)
            return;

        if (collision.gameObject.CompareTag("Player"))
            UnDisplayText();
    }

    public void DisplayText()
    {
        text.gameObject.SetActive(true);
    }
    public void UnDisplayText()
    {
        text.gameObject.SetActive(false);
    }
}
