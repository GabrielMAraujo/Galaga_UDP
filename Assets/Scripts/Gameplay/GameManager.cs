using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

//Callback for game events
public delegate void GameOverCallback(bool winner);

public class GameManager : MonoBehaviour
{
    public event GameOverCallback OnGameOver;
    public static GameManager instance;
    public bool gameOver = false;

    private ServerManager serverManager;

    private InputManager inputManager;
    private GameplayObjectPool objectPool;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        serverManager = GetComponent<ServerManager>();

        objectPool = GameplayObjectPool.instance;
        //Create the ship, one enemy and one of each projectile instances in object pool at game start to improve performance
        objectPool.CreateInstance(ObjectTypeEnum.SHIP);
        objectPool.CreateInstance(ObjectTypeEnum.ENEMY_SHOOTER);
        objectPool.CreateInstance(ObjectTypeEnum.PROJECTILE);
        objectPool.CreateInstance(ObjectTypeEnum.ENEMY_PROJECTILE);

        inputManager = InputManager.instance;
        inputManager.OnInputDown += OnInputDown;
    }

    private void Start()
    {

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
            gameOver = true;
            OnGameOver?.Invoke(true);
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
            gameOver = true;
            OnGameOver?.Invoke(false);
            Debug.Log("Game over");
            //Instantiate game over scene
            SceneManager.LoadScene("Game Over", LoadSceneMode.Additive);
            return true;
        }

        return false;
    }

    private void OnInputDown(InputTypeEnum input)
    {
        if (!gameOver)
        {
            //Only valid game inputs are from 0 through 3(shoot)
            if(input <= InputTypeEnum.SHOOT)
            {
                Debug.Log("Input: " + input);
                //Create request with designated input
                serverManager.CreateRequest(input);
            }
        }
    }
}
