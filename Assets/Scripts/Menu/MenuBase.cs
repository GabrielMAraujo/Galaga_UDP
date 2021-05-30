using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Contains basic functions for menu option selection and pointer navigation
public abstract class MenuBase : MonoBehaviour
{
    public Image pointer;
    public List<Text> optionsList;

    protected bool inInstructions = false;
    protected int selectedOption = 0;
    protected int pointerXPos = 0;

    private InputManager inputManager;

    protected virtual void Awake()
    {
        inputManager = InputManager.instance;
        inputManager.OnInputDown += OnInputDown;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        UpdatePointer();
    }

    public void OnMouseHover(int option)
    {
        selectedOption = option;
        UpdatePointer();
    }

    protected virtual void OnInputDown(InputTypeEnum input)
    {
        if (!inInstructions)
        {
            if (input == InputTypeEnum.DOWN)
            {
                selectedOption++;
            }
            else if (input == InputTypeEnum.UP)
            {
                selectedOption--;
            }
        }

        CheckPointerWrap();
        UpdatePointer();
    }

    //Updates pointer position according to selected option
    protected void UpdatePointer()
    {
        pointer.transform.position = new Vector2(pointerXPos, optionsList[selectedOption].transform.position.y);
    }

    //Check if pointer has to wrap (e.g. down on last item goes back to first item)
    protected void CheckPointerWrap()
    {
        if (selectedOption >= optionsList.Count)
        {
            selectedOption = 0;
        }
        if (selectedOption < 0)
        {
            selectedOption = optionsList.Count - 1;
        }
    }

    //Every child class has to implement what has to be done when selecting the menu items
    protected abstract void SelectMenuItem();

}
