using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private bool entered;

    [Header("Spawnable Enemies")]
    [SerializeField]
    private GameObject[] spawnableEnemies = new GameObject[4];
    [SerializeField]
    private GameObject[] enemnySpawnLocations = new GameObject[13];

    [SerializeField]
    private List<GameObject> aliveEnemies = new List<GameObject>();
    private int enemiesPerWave = 7;
    private int maxWaves = 3;
    private int currentWave = 0;
    private bool spawnEnemies = false;
    private bool dungeonClear = true;

    [SerializeField]
    private GameObject dungeonExit;

    private void Awake()
    {
        spawnEnemies = true;
    }

    private void Update()
    {
        //spawns waves of enemies in the boss room
        if (spawnEnemies && currentWave < maxWaves)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                int spawnChoice = Random.Range(0, spawnableEnemies.Length);
                int spawnLocationChoice = Random.Range(0, enemnySpawnLocations.Length);
                GameObject e = Instantiate(spawnableEnemies[spawnChoice], enemnySpawnLocations[spawnLocationChoice].transform.position, Quaternion.identity);
                aliveEnemies.Add(e);
            }

            spawnEnemies = false;
            currentWave++;
        }
        //checks to count enemies currently alive
        aliveEnemyCount();

        //end the boss room
        if (currentWave == maxWaves && dungeonClear)
        {
            EndSequence();
        }
    }

    //method to count enemies still alive
    private void aliveEnemyCount()
    {
        int deadenemies = 0;
        foreach(GameObject i in aliveEnemies)
        {
            if (i.activeInHierarchy == false)
            {
                deadenemies++;
            }
        }

        if(deadenemies == enemiesPerWave)
        {
            spawnEnemies = true;
            enemiesPerWave += 3;
            aliveEnemies.Clear();
        }
    }

    //end the boss fight and spawn a door exit
    private void EndSequence()
    {
        Debug.Log("End Sequence");
        dungeonClear = false;
        gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
    }
}
