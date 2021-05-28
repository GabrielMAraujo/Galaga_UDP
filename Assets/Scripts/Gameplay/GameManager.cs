using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ServerManager serverManager;

    private List<GameObject> currentObjects = new List<GameObject>();
    private InputManager inputManager;

    private bool gameOver = false;

    private void Awake()
    {
        serverManager = GetComponent<ServerManager>();
    }

    private void Start()
    {
        inputManager = InputManager.instance;
        inputManager.OnInputDown += OnInputDown;
    }

    private void OnDestroy()
    {
        inputManager.OnInputDown -= OnInputDown;
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

        if (!IsGameOver(objects))
        {

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

    //Checks for game over conditions
    private bool IsGameOver(List<ServerObject> responseObjects)
    {
        //If there is not a ship in current object list, game over
        if(responseObjects.FirstOrDefault(o => o.Type == ObjectTypeEnum.SHIP) == null)
        {
            //Stop time to block request timer
            Time.timeScale = 0;
            gameOver = true;
            Debug.Log("Game over");
            return true;
        }

        return false;
    }

    private void OnInputDown(InputTypeEnum input)
    {
        if (!gameOver)
        {
            Debug.Log("Input: " + input);
            //Create request with designated input
            serverManager.CreateRequest(input);
        }
    }
}
