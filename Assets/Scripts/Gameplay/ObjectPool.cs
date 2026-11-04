using System.Collections.Generic;
using UnityEngine;

public class ObjectPool 
{
    private readonly GameObject _prefab;
    private readonly Transform _container;
    private readonly List<GameObject> _items = new List<GameObject>();

    public ObjectPool(GameObject prefab, int prewarm = 0)
    {
        _prefab = prefab;
        _container = new GameObject(prefab.name + " Pool").transform;

        for (int i = 0; i < prewarm; i++)
        {
            GameObject obj = Object.Instantiate(_prefab, _container);
            obj.SetActive(false);
            _items.Add(obj);
        }
    }

    public GameObject Get(Vector3 position)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            GameObject item = _items[i];
            if (!item.activeSelf)
            {
                item.transform.position = position;
                item.SetActive(true);
                return item;
            }
        }

        GameObject obj = Object.Instantiate(_prefab, position, Quaternion.identity, _container);
        _items.Add(obj);
        return obj;
    }

    public int CountActive()
    {
        int count = 0;
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].activeSelf) count++;
        }
        return count;
    }
}
