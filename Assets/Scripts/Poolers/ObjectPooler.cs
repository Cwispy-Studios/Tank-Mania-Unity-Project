using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Poolers
{
  public class ObjectPooler<T> : MonoBehaviour where T : MonoBehaviour
  {
    [SerializeField] private List<T> objectPrefabsList = null;
    [SerializeField, Range(0, 100)] private int numberPooledPerPrefab = 50;

    private Dictionary<string, List<T>> pooledObjectsDictionary = new Dictionary<string, List<T>>();

    private void Awake()
    {
      InitialiseObjectPooler();
    }

    private void InitialiseObjectPooler()
    {
      foreach (T objectPrefab in objectPrefabsList)
      {
        if (objectPrefab != null)
        {
          List<T> objectList = new List<T>();

          for (int i = 0; i < numberPooledPerPrefab; ++i)
          {
            T pooledObject = InstantiateObject(objectPrefab, i);
            objectList.Add(pooledObject);
          }

          pooledObjectsDictionary.Add(objectPrefab.name, objectList);
        }
      }
    }

    private T InstantiateObject( T objectPrefab, int number )
    {
      T pooledObject = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity, transform);
      pooledObject.gameObject.SetActive(false);
      pooledObject.gameObject.name = objectPrefab.name + number;

      return pooledObject;
    }

    private T FindInactiveObject( T objectPrefab )
    {
      List<T> objectList = pooledObjectsDictionary[objectPrefab.name];

      // Find an inactive projectile in the list
      for (int i = 0; i < objectList.Count; ++i)
      {
        if (!objectList[i].gameObject.activeSelf)
        {
          return objectList[i];
        }
      }

      // If we reach here, there are no more inactive projectiles and we need to instantiate one
      T pooledObject = InstantiateObject(objectPrefab, objectList.Count);
      objectList.Add(pooledObject);

      return pooledObject;
    }

    public T EnablePooledObject( T objectPrefab, Vector3 spawnLocation, Quaternion rotation, bool usePrefabHeight = false )
    {
      T pooledObject = FindInactiveObject(objectPrefab);

      if (usePrefabHeight)
      {
        spawnLocation.y += objectPrefab.transform.position.y;
      }

      pooledObject.transform.position = spawnLocation;
      pooledObject.transform.rotation = rotation;
      pooledObject.gameObject.SetActive(true);

      return pooledObject;
    }
  }
}
