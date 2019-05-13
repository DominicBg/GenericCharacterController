using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/* Comment m'utilisé : 
 * 		Dans l'inspecteur, remplir poolArray avec les informations pour chaque Object a pooler
 * 		
 * 		PoolManager.instance.GetObject("nom de l'objet");
 * 		Une faute d'orthographe va résulté a un objet null + une erreur
 * 		ENJOY LE SPAM
 * 
 * 		bisoux, 
 * 		dominqu
 */


public class PoolManager : MonoBehaviour
{
    #region singleton
    public static PoolManager instance;
    void Awake()
    {
        instance = this;
    }
    public bool isDebugMode = false;
    #endregion

    [SerializeField] PoolObject[] poolArray;

    private Dictionary<string, PoolObject> pool = new Dictionary<string, PoolObject>();

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < poolArray.Length; i++)
        {
            PoolObject obj = poolArray[i];
            obj.Initialise();
            pool.Add(obj.name, obj);
        }
        //poolArray = new PoolObject[0];
    }

    public GameObject GetObject(string name, Vector3 position, Quaternion rotation)
    {
        PoolObject poolObject;
        //Place requested Object in obj, if its null, pop error
        if (!pool.TryGetValue(name, out poolObject))
        {
            Debug.LogError("<color=red>WRONG KEY : " + name + "</color>");
            return null;
        }
        GameObject go = poolObject.GetNext();
        go.transform.position = position;
        go.transform.rotation = rotation;
        Debug.Log(go.name);
        return go;
    }

    public GameObject GetObjectAutoReturn(string name, Vector3 position, Quaternion rotation, float timeBeforeReturn, System.Action callback)
    {
        GameObject gameObject = GetObject(name, position, rotation);
        StartCoroutine(AutoReturn(name, gameObject, timeBeforeReturn, callback));
        return gameObject;
    }

    #region GetObject Overload
    public GameObject GetObject(string name, Vector3 position, Vector3 eulerAngles)
    {
        return GetObject(name, position, Quaternion.Euler(eulerAngles));
    }
    public GameObject GetObject(string name, Vector3 position)
    {
        return GetObject(name, position, Quaternion.identity);
    }
    public GameObject GetObject(string name)
    {
        return GetObject(name, Vector3.zero, Quaternion.identity);
    }
    public T GetObject<T>(string name)
    {
        return GetObject(name, Vector3.zero, Quaternion.identity).GetComponent<T>();
    }
    public T GetObject<T>(string name, Vector3 position, Vector3 eulerAngles)
    {
        return GetObject(name, position, eulerAngles).GetComponent<T>();
    }
    public T GetObject<T>(string name, Vector3 position)
    {
        return GetObject(name, position, Quaternion.identity).GetComponent<T>();
    }

    public GameObject GetObjectAutoReturn(string name, Vector3 position, Vector3 eulerAngles, float timeBeforeReturn)
    {
        return GetObjectAutoReturn(name, position, Quaternion.Euler(eulerAngles), timeBeforeReturn, null);
    }
    public GameObject GetObjectAutoReturn(string name, Vector3 position, float timeBeforeReturn)
    {
        return GetObjectAutoReturn(name, position, Quaternion.identity, timeBeforeReturn, null);
    }
    public GameObject GetObjectAutoReturn(string name, float timeBeforeReturn)
    {
        return GetObjectAutoReturn(name, Vector3.zero, Quaternion.identity, timeBeforeReturn, null);
    }

    public GameObject GetObjectAutoReturn(string name, Vector3 position, Vector3 eulerAngles, float timeBeforeReturn, System.Action callback)
    {
        return GetObjectAutoReturn(name, position, Quaternion.Euler(eulerAngles), timeBeforeReturn, callback);
    }
    public GameObject GetObjectAutoReturn(string name, Vector3 position, float timeBeforeReturn, System.Action callback)
    {
        return GetObjectAutoReturn(name, position, Quaternion.identity, timeBeforeReturn, callback);
    }
    public GameObject GetObjectAutoReturn(string name, float timeBeforeReturn, System.Action callback)
    {
        return GetObjectAutoReturn(name, Vector3.zero, Quaternion.identity, timeBeforeReturn, callback);
    }
#endregion

    public void ReturnObject(string name, GameObject gameObject)
    {
        TryReturnObject(name, gameObject);
    }

    IEnumerator AutoReturn(string name, GameObject gameObject, float timeBeforeReturn, System.Action callback)
    {
        yield return new WaitForSeconds(timeBeforeReturn);

        bool tryReturnObjectSucess = TryReturnObject(name, gameObject);

        if (tryReturnObjectSucess)
        {
            if (callback != null)
                callback.Invoke();
        }
    }

    /// <summary>
    /// Return true if the return worked, otherwise return false
    /// </summary>
    bool TryReturnObject(string name, GameObject gameObject)
    {
        if (pool.ContainsKey(name))
        {
            //If the object is already in pool, dont return it in pol
            if(pool[name].IsObjectInPool(gameObject))
            {
                if(isDebugMode)
                    Debug.LogError("<color=red>You tried to insert multiple time : " + name + " in the pool</color>");
                return false;
            }

            pool[name].ReturnObject(gameObject);
            return true;
        }
        else
        {
            Debug.LogError("<color=red>WRONG KEY : " + name + "</color>");
            return false;
        }




    }

    [ContextMenu("Generate Const File")]
    public void GenerateConstFile()
    {
        string[] poolValues = new List<PoolObject>(poolArray).Select(x => x.name).ToArray();
        ConstFileWriter.GenerateConstFile(this, "PoolConst", poolValues);
    }

    [System.Serializable]
    public class PoolObject
    {
        public string name;
        public GameObject prefab;
        public int quantity;

        private Queue<GameObject> queue;
        private Dictionary<GameObject, bool> inPoolDictionary = new Dictionary<GameObject, bool>();

        GameObject objParent;

        public void Initialise()
        {
            objParent = new GameObject(name);
            queue = new Queue<GameObject>();

            for (int i = 0; i < quantity; i++)
            {
                AddNewObject();
            }
        }

        void AddNewObject()
        {
            GameObject go = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity);

            inPoolDictionary.Add(go, true);

            go.transform.SetParent(objParent.transform);
            go.SetActive(false);
            queue.Enqueue(go);
        }

        public void ReturnObject(GameObject gameObject)
        {
            inPoolDictionary[gameObject] = true;
            queue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        public bool IsObjectInPool(GameObject gameObject)
        {
            return inPoolDictionary[gameObject];
        }

        public GameObject GetNext()
        {
            if (queue.Count == 0)
                AddNewObject();

            GameObject currentObj = queue.Dequeue();
            inPoolDictionary[currentObj] = false;
            currentObj.SetActive(true);
            return currentObj;
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(PoolManager))]
public class PoolManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PoolManager poolManager = (PoolManager)target;
        if (GUILayout.Button("Generate Const File"))
        {
            poolManager.GenerateConstFile();
        }
    }
}
#endif