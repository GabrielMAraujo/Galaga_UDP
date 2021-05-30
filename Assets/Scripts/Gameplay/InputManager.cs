using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Callback for input events
public delegate void InputCallback(InputTypeEnum input);

public class InputManager : MonoBehaviour
{
    public event InputCallback OnInputDown;

    public static InputManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Detecting input
        if (Input.GetButtonDown("Left"))
        {
            OnInputDown?.Invoke(InputTypeEnum.LEFT);
        }

        if (Input.GetButtonDown("Right"))
        {
            OnInputDown?.Invoke(InputTypeEnum.RIGHT);
        }

        if (Input.GetButtonDown("Shoot"))
        {
            OnInputDown?.Invoke(InputTypeEnum.SHOOT);
        }

        //Menu inputs
        if (Input.GetButtonDown("Up"))
        {
            OnInputDown?.Invoke(InputTypeEnum.UP);
        }

        if (Input.GetButtonDown("Down"))
        {
            OnInputDown?.Invoke(InputTypeEnum.DOWN);
        }

        if (Input.GetButtonDown("Select"))
        {
            OnInputDown?.Invoke(InputTypeEnum.SELECT);
        }
    }
}
