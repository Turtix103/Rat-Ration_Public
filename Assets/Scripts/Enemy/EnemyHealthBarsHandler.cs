using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarsHandler : MonoBehaviour
{
    public Slider sliderTemplate;
    private Dictionary<GameObject, Slider> enemySliders = new Dictionary<GameObject, Slider>(); //chat
    public Slider[] sliders;
    public Color low;
    public Color high;
    public Vector3 offset;
    public bool loaded = false;

    void Start()
    {
        StartCoroutine(BugFix());
    }

    public void onSceneLoaded()
    {
        enemySliders = new Dictionary<GameObject, Slider>(); ;
        sliders = null;
        loaded = false;

        StartCoroutine(BugFix());
    }

    private IEnumerator BugFix()
    {
        yield return new WaitForEndOfFrame();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].GetComponent<EnemyHealth>().boss)
            {
                Slider slider = Instantiate(sliderTemplate, gameObject.transform.position, gameObject.transform.rotation);
                slider.transform.SetParent(GameObject.Find("Enemy Health Bars").transform, false);
                enemySliders.Add(enemies[i], slider);
            }
        }
        loaded = true;
    }

    void Update()
    {
        foreach (KeyValuePair<GameObject, Slider> enemy in enemySliders)
        {
            enemy.Value.transform.position = Camera.main.WorldToScreenPoint(enemy.Key.transform.position + offset);
        }
    }

    public void SetHealth (int currentHealth, int maxHealth, GameObject enemy)
    {
        Slider slider = enemySliders[enemy];
        slider.gameObject.SetActive(currentHealth < maxHealth);
        slider.value = currentHealth;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }

    public void Disable(GameObject enemy)
    {
        Slider slider = enemySliders[enemy];
        slider.gameObject.SetActive(false);
    }
}
