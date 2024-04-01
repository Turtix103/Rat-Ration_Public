using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextDisplayHandler : MonoBehaviour
{
    public TextMeshProUGUI textTemplate;
    private Dictionary<GameObject, TextMeshProUGUI> doorTexts = new Dictionary<GameObject, TextMeshProUGUI>();
    private Dictionary<GameObject, TextMeshProUGUI> chestTexts = new Dictionary<GameObject, TextMeshProUGUI>();
    private Dictionary<GameObject, TextMeshProUGUI> stationTexts = new Dictionary<GameObject, TextMeshProUGUI>();
    public Vector3 offset;
    public bool loaded = false;

    void Start()
    {
        StartCoroutine(BugFix());
    }

    public void onSceneLoaded()
    {
        foreach(Transform child in GameObject.Find("Text Displays").transform)
        {
            Destroy(child.gameObject);
        }

        doorTexts = new Dictionary<GameObject, TextMeshProUGUI>();
        loaded = false;

        StartCoroutine(BugFix());
    }


    private IEnumerator BugFix()
    {
        yield return new WaitForEndOfFrame();

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        for (int i = 0; i < doors.Length; i++)
        {
            TextMeshProUGUI text = Instantiate(textTemplate, gameObject.transform.position, gameObject.transform.rotation);
            text.transform.SetParent(GameObject.Find("Text Displays").transform, false);
            doorTexts.Add(doors[i], text);
            text.gameObject.SetActive(false);
        }
        loaded = true;
    }

    void Update()
    {
        foreach (KeyValuePair<GameObject, TextMeshProUGUI> text in doorTexts)
        {
            text.Value.transform.position = Camera.main.WorldToScreenPoint(text.Key.transform.position + offset);
        }
    }

    public void DisplayText(GameObject door)
    {
        TextMeshProUGUI text = doorTexts[door];
        text.gameObject.SetActive(true);
    }
    public void UnDisplayText(GameObject door)
    {
        TextMeshProUGUI text = doorTexts[door];
        text.gameObject.SetActive(false);
    }
}
