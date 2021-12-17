using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Poolers
{
  public class ObjectPooler : MonoBehaviour
  {
    [SerializeField] private List<GameObject> objectPrefabsList = null;
    [SerializeField, Range(0, 100)] private int numberPooledPerPrefab = 10;

    private Dictionary<string, List<GameObject>> pooledObjectsDictionary = new Dictionary<string, List<GameObject>>();

    private void Awake()
    {
      InitialiseObjectPooler();
    }

    private void InitialiseObjectPooler()
    {
      foreach (GameObject objectPrefab in objectPrefabsList)
      {
        if (objectPrefab != null)
        {
          AddObjectToPooler(objectPrefab);
        }
      }
    }

    private void AddObjectToPooler( GameObject objectPrefab )
    {
      List<GameObject> objectList = new List<GameObject>();

      for (int i = 0; i < numberPooledPerPrefab; ++i)
      {
        GameObject pooledObject = InstantiateObject(objectPrefab, i);
        objectList.Add(pooledObject);
      }

      pooledObjectsDictionary.Add(objectPrefab.name, objectList);
    }

    private GameObject InstantiateObject( GameObject objectPrefab, int number )
    {
      GameObject pooledObject = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
      pooledObject.gameObject.SetActive(false);
      pooledObject.gameObject.name = objectPrefab.name + number;

      return pooledObject;
    }

    private GameObject FindInactiveObject( GameObject objectPrefab )
    {
      List<GameObject> objectList = pooledObjectsDictionary[objectPrefab.name];

      // Find an inactive projectile in the list
      for (int i = 0; i < objectList.Count; ++i)
      {
        if (!objectList[i].gameObject.activeSelf)
        {
          return objectList[i];
        }
      }

      // If we reach here, there are no more inactive projectiles and we need to instantiate one
      GameObject pooledObject = InstantiateObject(objectPrefab, objectList.Count);
      objectList.Add(pooledObject);

      return pooledObject;
    }

    public GameObject EnablePooledObject( GameObject objectPrefab, Vector3 spawnLocation, Quaternion rotation, bool usePrefabHeight = false )
    {
      if (!pooledObjectsDictionary.ContainsKey(objectPrefab.name))
      {
        AddObjectToPooler(objectPrefab);
      }

      GameObject pooledObject = FindInactiveObject(objectPrefab);

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
