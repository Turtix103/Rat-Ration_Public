using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static GameObject player;
    public static GameObject canvas;

    public float[] healthMultipliers = new float[] { 1, 2, 5, 12};
    public float[] damageMultipliers = new float[] { 1, 1.5f , 2.5f, 4 };

    public static bool first;

    public int levelDifficulty;

    private void OnEnable()
    {
        levelDifficulty--;
        player = GameObject.Find("Gamer");
        canvas = GameObject.Find("Canvas");

        DontDestroyOnLoad(player);
        DontDestroyOnLoad(canvas);


        if (first == true)
        {
            player.GetComponent<PlayerMovement>().onSceneLoaded();
            canvas.GetComponent<EnemyHealthBarsHandler>().onSceneLoaded();
            canvas.GetComponent<TextDisplayHandler>().onSceneLoaded();
        }

        if (first == false)
            first = true;
    }

}
