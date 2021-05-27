using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<GameObject> currentObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Temporary function - instantiates new objects and deletes old ones
    public void InstantiateObjects(List<ServerObject> objects)
    {
        if (currentObjects.Count > 0)
        {
            foreach (var co in currentObjects)
            {
                Destroy(co);
            }
        }

        currentObjects.Clear();

        foreach (var obj in objects)
        {
            GameObject instance = null;
            switch (obj.Type)
            {
                case ObjectTypeEnum.SHIP:
                    instance = Instantiate(Resources.Load("Prefabs/Ship")) as GameObject;
                    break;
                case ObjectTypeEnum.PROJECTILE:
                    instance = Instantiate(Resources.Load("Prefabs/Projectile")) as GameObject;
                    break;
                case ObjectTypeEnum.ENEMY_SHOOTER:
                    instance = Instantiate(Resources.Load("Prefabs/Enemy Shooter")) as GameObject;
                    break;
                case ObjectTypeEnum.ENEMY_PROJECTILE:
                    instance = Instantiate(Resources.Load("Prefabs/Enemy Projectile")) as GameObject;
                    break;
                default:
                    break;
            }

            if (instance != null)
            {
                instance.GetComponent<GameplayObject>().Move(new Vector2Int((int)obj.XPos, (int)obj.YPos));
                currentObjects.Add(instance);
            }
            else
            {
                Debug.LogWarning("Server object with undefined prefab type: " + obj.Type);
            }
        }
    }
}
