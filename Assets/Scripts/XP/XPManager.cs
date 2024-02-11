/*
 * Contains functions to generate XP and also implements object pooling to reduce CPU load.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    public GameObject xpPrefab;

    public List<GameObject> xpPool;

    public static XPManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        xpPool = new List<GameObject>();
        GameObject newOrb;

        for (int i = 0; i < 20; i++)
        {
            newOrb = Instantiate(xpPrefab);
            xpPool.Add(newOrb);
            newOrb.SetActive(false);
        }
    }

    private GameObject GetOrb()
    {
        foreach (GameObject currentOrb in xpPool)
        {
            if (currentOrb.activeSelf)
            {
                currentOrb.SetActive(true);
                return currentOrb;
            }
        }

        GameObject newOrb = Instantiate(xpPrefab);
        xpPool.Add(newOrb);
        return newOrb;
    }

    public void SpawnXP(Vector3 spawnPos)
    {
        GetOrb().transform.position = spawnPos;
    }

    public void SpawnXP(Vector3 spawnPos, int numToSpawn)
    {

    }
}
