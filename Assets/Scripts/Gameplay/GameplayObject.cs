using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all game objects defined by the UDP server
public class GameplayObject : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Move towards a fixed direction
    public void MoveTowards(Vector2Int direction)
    {
        transform.position = new Vector2((int)transform.position.x + direction.x, (int)transform.position.y + direction.y);
    }

    //Moves to the specified coordinates
    public void Move(Vector2Int position)
    {
        transform.position = (Vector2)position;
    }
}
