using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayObjectPool : MonoBehaviour
{
    public static GameplayObjectPool instance;

    //As there is only one ship always, it's not necessary to do a list, only save a single reference
    private GameObject ship;
    //There are 3 separate pools so it won't be necessary to perform any list search when the instance has to be fetched
    private List<GameObject> enemyPool;
    private List<GameObject> projectilePool;
    private List<GameObject> enemyProjectilePool;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        enemyPool = new List<GameObject>();
        projectilePool = new List<GameObject>();
        enemyProjectilePool = new List<GameObject>();
    }

    //Creates an object instance in the object pool. The object amount can be specified to create multiple instances
    public GameObject CreateInstance(ObjectTypeEnum type, int amount = 1)
    {
        GameObject prefab;
        GameObject result = null;
        List<GameObject> selectedPool = null;

        //Load the correct resource for instance
        switch (type)
        {
            case ObjectTypeEnum.SHIP:
                prefab = Resources.Load("Prefabs/Ship") as GameObject;
                ship = Instantiate(prefab);
                ship.SetActive(false);
                return ship;
            case ObjectTypeEnum.PROJECTILE:
                prefab = Resources.Load("Prefabs/Projectile") as GameObject;
                selectedPool = projectilePool;
                break;
            case ObjectTypeEnum.ENEMY_SHOOTER:
                prefab = Resources.Load("Prefabs/Enemy Shooter") as GameObject;
                selectedPool = enemyPool;
                break;
            case ObjectTypeEnum.ENEMY_PROJECTILE:
                prefab = Resources.Load("Prefabs/Enemy Projectile") as GameObject;
                selectedPool = enemyProjectilePool;
                break;
            case ObjectTypeEnum.FINAL_OBJECT:
                prefab = Resources.Load("Prefabs/Final Object") as GameObject;
                break;
            default:
                Debug.LogWarning("Object type not mapped to a prefab");
                return null;
        }

        for (int i = 0; i < amount; i++)
        {
            result = Instantiate(prefab);
            result.SetActive(false);
            selectedPool?.Add(result);
        }

        return result;
    }

    //Gets instance from corresponding pool
    public GameObject GetInstance(ObjectTypeEnum type)
    {
        GameObject instance = null;
        List<GameObject> selectedPool = null;

        switch (type)
        {
            case ObjectTypeEnum.SHIP:
                instance = ship;
                break;
            case ObjectTypeEnum.PROJECTILE:
                selectedPool = projectilePool;
                break;
            case ObjectTypeEnum.ENEMY_SHOOTER:
                selectedPool = enemyPool;
                break;
            case ObjectTypeEnum.ENEMY_PROJECTILE:
                selectedPool = enemyProjectilePool;
                break;
            case ObjectTypeEnum.FINAL_OBJECT:
                //As final objects will only be used once, it also doesn't make sense to store the references in a list. Just create a new instance and send it
                instance = CreateInstance(ObjectTypeEnum.FINAL_OBJECT);
                break;
            default:
                Debug.LogWarning("Object type " + type + " doesn't have a pool");
                return null;
        }

        if(selectedPool != null)
        {
            //Try and fetch an existing inactive object from the pool
            GameObject inactiveGO = selectedPool.FirstOrDefault(go => go.activeInHierarchy == false);

            //If there aren't any inactive objects to be utilized, create a new one in the pool and pass it as a response
            if(inactiveGO == null)
            {
                inactiveGO = CreateInstance(type);
            }

            instance = inactiveGO;
        }

        return instance;
    }

    //Inactivates all pool objects so they can be set again in the next update
    public void InactivateAllObjects()
    {
        ship?.SetActive(false);
        enemyPool.ForEach(o => o.SetActive(false));
        projectilePool.ForEach(o => o.SetActive(false));
        enemyProjectilePool.ForEach(o => o.SetActive(false));
    }
}
