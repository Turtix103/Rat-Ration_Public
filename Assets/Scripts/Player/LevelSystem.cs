using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    public int currentExp = 0;
    public int currentLevel = 1;

    public int[] levelRequirements = new int[] { 25, 45, 70 };

    public GameObject levelMenu;
    public GameObject expDisplay;
    
    public void AddExp(int exp)
    {
        currentExp += exp;
        if (currentExp > levelRequirements[currentLevel - 1])
        {
            currentExp = currentExp - levelRequirements[currentLevel - 1];
            currentLevel++;
            levelMenu.SetActive(true);
            StopTime.Stop();
        }
        expDisplay.GetComponent<TextMeshProUGUI>().text = $"Expirience: {currentExp}/{levelRequirements[currentLevel - 1]}";
    }
}
