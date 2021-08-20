using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Pooler is a dynamic way to pool objects using static methods, without having to reference anything. 
/// It must be reset on scene load as old refs will be no longer relevant and will be null
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    public static Dictionary<GameObject, List<GameObject>> AllPooledObjects = new Dictionary<GameObject, List<GameObject>>();

    /// <summary>
    /// Get an object, will be added to pool if necessary and returned
    /// </summary>
    /// <param name="prefabObject"></param>
    /// <returns></returns>
    public static GameObject GetObject(GameObject prefabObject)
    {
        if (AllPooledObjects.ContainsKey(prefabObject))
        {
            GameObject obj = AllPooledObjects[prefabObject].Find(x => x.activeInHierarchy == false);
            if (obj != null)
            {
                obj.SetActive(true);
                return obj;
            } else
            {
                return GetNewObject(prefabObject);
            }
        } else
        {
            AllPooledObjects.Add(prefabObject, new List<GameObject>());
            GameObject obj = GetNewObject(prefabObject);
            AllPooledObjects[prefabObject].Add(obj);
            return obj;
        }
    }

    public static GameObject GetNewObject(GameObject obj)
    {
        GameObject newObject = Instantiate(obj, Vector3.zero, Quaternion.identity);
        AllPooledObjects[obj].Add(newObject);
        return newObject;
    }

    public static void ClearAll()
    {
        Debug.Log("Clearing Object Pooler");
        AllPooledObjects = new Dictionary<GameObject, List<GameObject>>();
    }
}
