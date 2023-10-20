using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField]
    private GameObject afterImagePrefabs;
    [SerializeField]
    private int amountOfAfterImages = 10;
    
    private Queue<GameObject> availableObjects = new Queue<GameObject>();
    public static PlayerAfterImagePool Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        GrowPool();
    }
    private void GrowPool()
    {
        for (int i = 0; i < amountOfAfterImages; i++)
        {
            var instanceToAdd = Instantiate(afterImagePrefabs);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }
    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }
    public GameObject GetFromPool()
    {
        if(availableObjects.Count == 0)
        {
            GrowPool();
        }
        var instance = availableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
