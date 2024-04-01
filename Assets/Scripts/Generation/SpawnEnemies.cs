using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SpawnEnemies : MonoBehaviour
{
    private GameObject[] spawns;
    public List<GameObject> enemies = new List<GameObject>();
    private System.Random gen = new System.Random();
    void Start()
    {
        GameObject parentObject =  GameObject.Find("Enemies").gameObject;
        spawns = gameObject.GetComponentsInChildren<Transform>().Where(child => child.name == "Enemy Spawn").Select(child => child.gameObject).ToArray();
        foreach (GameObject spawn in spawns)
        {
            int enemyIndex = gen.Next(0, enemies.Count);

            GameObject currentEnemy = Instantiate(enemies[enemyIndex], spawn.transform.position, spawn.transform.rotation);
            currentEnemy.transform.parent = parentObject.transform;
            if (Random.value < 0.5)
                currentEnemy.GetComponent<EnemyPatrol>().switchDirection();
        }
    }
}
