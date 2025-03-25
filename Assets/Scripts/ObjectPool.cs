using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    private Dictionary<T, Queue<T>> _poolDict = new Dictionary<T, Queue<T>>();

    public T GetFromPool(T prefab, Vector3 position, Quaternion rotation)
    {
        if (!_poolDict.ContainsKey(prefab))
            _poolDict[prefab] = new Queue<T>();

        Queue<T> pool = _poolDict[prefab];
        T instance;

        if (pool.Count > 0)
        {
            instance = pool.Dequeue();
        }
        else
        {
            instance = Instantiate(prefab, transform);
        }

        instance.transform.SetPositionAndRotation(position, rotation); 
        instance.gameObject.SetActive(true);    
        return instance;    
    }

    public void ReturnToPool(T prefab, T instance)
    {
        instance.gameObject.SetActive(false);
        _poolDict[prefab].Enqueue(instance);
    }
}
