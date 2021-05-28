using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ServerManager serverManager;

    private InputManager inputManager;
    private GameplayObjectPool objectPool;

    private bool gameOver = false;

    private void Awake()
    {
        serverManager = GetComponent<ServerManager>();
        objectPool = GameplayObjectPool.instance;

        inputManager = InputManager.instance;
        inputManager.OnInputDown += OnInputDown;
    }

    private void Start()
    {
        //Create the ship, one enemy and one of each projectile instances in object pool at game start to improve performance
        objectPool.CreateInstance(ObjectTypeEnum.SHIP);
        objectPool.CreateInstance(ObjectTypeEnum.ENEMY_SHOOTER);
        objectPool.CreateInstance(ObjectTypeEnum.PROJECTILE);
        objectPool.CreateInstance(ObjectTypeEnum.ENEMY_PROJECTILE);
    }

    private void OnDestroy()
    {
        inputManager.OnInputDown -= OnInputDown;
    }

    //Update object states (position and activeness)
    public void UpdateObjects(List<ServerObject> objects)
    {
        if (IsVictory(objects) || !IsGameOver(objects))
        {
            //Reset active states for instance reuse
            objectPool.InactivateAllObjects();

            foreach (var obj in objects)
            {
                GameObject instance = objectPool.GetInstance(obj.Type);

                if (instance != null)
                {
                    instance.SetActive(true);
                    instance.GetComponent<GameplayObject>().Move(new Vector2Int((int)obj.XPos, (int)obj.YPos));
                }
                else
                {
                    Debug.LogWarning("Server object with undefined prefab type: " + obj.Type);
                }
            }
        }
    }

    //Checks for victory conditions
    private bool IsVictory(List<ServerObject> responseObjects)
    {
        //If the first response object is a final object, player won
        if(responseObjects[0].Type == ObjectTypeEnum.FINAL_OBJECT)
        {
            //Stop time to block request timer
            Time.timeScale = 0;
            gameOver = true;
            Debug.Log("Victory");
            return true;
        }
        return false;
    }

    //Checks for game over conditions
    private bool IsGameOver(List<ServerObject> responseObjects)
    {
        //If there is not a ship in current object list, game over
        if(responseObjects.FirstOrDefault(o => o.Type == ObjectTypeEnum.SHIP) == null)
        {
            objectPool.InactivateAllObjects();
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
