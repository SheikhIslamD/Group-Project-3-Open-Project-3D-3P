using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    [Tooltip("The prefab to pool.")]
    [SerializeField] private PoolableObject prefabObject;
    [Tooltip("The number of prefabs to create on startup.")]
    [SerializeField] private int defaultPoolDepth;
    [Tooltip("Whether or not more prefabs can be created beyond the initial Pool Depth.")]
    [SerializeField] private bool canGrow = true;
    [Tooltip("The transform the Objects will be parented under. (Defaults to the scene.)")]
    [SerializeField] private Transform parent = null;
    [Tooltip("How long an instance will last until it is automatically disabled, set to -1 to never auto disable.")]
    [SerializeField] private float autoDisableTime = -1;
    [Tooltip("Whether or not the Objects in this pool will disappear if the pool is destroyed.")]
    [SerializeField] private bool deleteObjectsOnDestroy = true;
    
    private readonly List<PoolableObject> poolList = new List<PoolableObject>();
    private int currentActiveObjects = 0;
    private int currentPooledObjects = 0;
    private int currentSelection = 0;
    private bool initialized;
    
    public static ObjectPool Create(GameObject @object, PoolableObject prefabObject, int defaultPoolDepth, bool canGrow = true, Transform parent = null, float autoDisableTime = -1, bool deleteObjectsOnDestroy = true)
    {
        ObjectPool pool = @object.AddComponent<ObjectPool>();
        pool.prefabObject = prefabObject;
        pool.defaultPoolDepth = defaultPoolDepth;
        pool.canGrow = canGrow;
        pool.parent = parent;
        pool.autoDisableTime = autoDisableTime;

        return pool;
    }
    
    
    void OnEnable() => Initialize();

    void Update()
    {
        if(autoDisableTime > 0)
        {
            for (int i = 0; i < poolList.Count; i++)
            {
                if (poolList[i].Active) poolList[i].timeExisting += Time.deltaTime;
                if (poolList[i].timeExisting >= autoDisableTime) poolList[i].gameObject.SetActive(false);
            }
        }
    }

    void Initialize(){

        if(initialized) return;
        for (int i = 0; i < defaultPoolDepth; i++) NewInstance();
        initialized = true;
    }


    public PoolableObject Pump()
    {
        FindNextInstance();
        PoolableObject instance = ActivateInstance(poolList[currentSelection]);
        IncrementSelection();
        return instance;
    }
    
    void NewInstance(){
        PoolableObject pooledObject = Instantiate(prefabObject);
        pooledObject.transform.parent = parent;
        pooledObject.pool = this;
        pooledObject.onDeactivate += OnDeActivate;
        poolList.Add(pooledObject);
        currentPooledObjects++;
        currentActiveObjects++;
        pooledObject.gameObject.SetActive(false);
    }
    void FindNextInstance(){  
        if (!poolList[currentSelection].Active) return;
        if (currentActiveObjects >= currentPooledObjects) 
        {
            if (!canGrow) return;

            NewInstance();
            currentSelection = currentPooledObjects-1;
        }
        while(poolList[currentSelection].Active) IncrementSelection();
    }
    void IncrementSelection() => currentSelection = (currentSelection == currentPooledObjects-1)? 0 : currentSelection+1; 
    PoolableObject ActivateInstance(PoolableObject instance)
    {
        instance.gameObject.SetActive(true);
        instance.Active = true;
        currentActiveObjects++;
        instance.timeExisting = 0;
        return instance;
    }
    
    void OnDeActivate(PoolableObject instance){  
        currentActiveObjects--;
        instance.Active = false;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            poolList[i].Disable(false);
            poolList[i].onDeactivate -= OnDeActivate;
            if(deleteObjectsOnDestroy) if(poolList[i] != null) Destroy(poolList[i].gameObject);
        }
        poolList.Clear();
    }

    public int ActiveObjects() => currentActiveObjects;
}