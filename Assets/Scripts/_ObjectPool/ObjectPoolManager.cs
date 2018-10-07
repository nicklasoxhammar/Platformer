using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager: MonoBehaviour
{
    
    [SerializeField] GameObject objectPrefab;
    [SerializeField] int poolSize = 10;
    [SerializeField] bool expandeblePoolSize = true;
    private List<GameObject> pool;

    // Use this for initialization
    void Start()
    {
        InitPool();
    }


    private void InitPool()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newObject = Instantiate(objectPrefab, transform);
            newObject.SetActive(false);
            pool.Add(newObject);
        }
    }


    public GameObject GetObjectFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i] != null && !pool[i].gameObject.activeInHierarchy)
            {
                return pool[i];
            }
        }
        if (expandeblePoolSize)
        {
            GameObject newObject = Instantiate(objectPrefab, transform);
            pool.Add(newObject);
            newObject.SetActive(false);
            return newObject;
        }
        return null;
    }


}
