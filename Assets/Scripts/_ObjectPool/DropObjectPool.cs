using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectPool : MonoBehaviour {

    public static DropObjectPool instance = null;
    private ObjectPoolManager objectPool;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }


	// Use this for initialization
    void Start()
    {
        objectPool = GetComponent<ObjectPoolManager>();
    }
	
    public GameObject GetObjectFromPool()
    {
        return objectPool.GetObjectFromPool();
    }
}
