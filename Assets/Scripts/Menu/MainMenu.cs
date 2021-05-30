using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image pointer;
    public List<Text> optionsList;

    private InputManager inputManager;

    private int selectedOption = 0;
    private bool inInstructions = false;

    private void Awake()
    {
        inputManager = InputManager.instance;
        inputManager.OnInputDown += OnInputDown;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdatePointer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseHover(int option)
    {
        selectedOption = option;
        UpdatePointer();
    }

    private void OnInputDown(InputTypeEnum input)
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

        if (input == InputTypeEnum.SELECT)
        {
            if (!inInstructions)
            {
                SelectMenuItem();
            }
            else
            {
                CloseInstructions();
            }
        }

        CheckPointerWrap();
        UpdatePointer();

    }

    public void GoToGameplay()
    {
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
    }

    public void GoToInstructions()
    {
        pointer.gameObject.SetActive(false);
        inInstructions = true;
        SceneManager.LoadScene("Instructions", LoadSceneMode.Additive);
    }

    public void CloseInstructions()
    {
        pointer.gameObject.SetActive(true);
        inInstructions = false;
        SceneManager.UnloadSceneAsync("Instructions");
    }

    //Check if pointer has to wrap (e.g. down on last item goes back to first item)
    private void CheckPointerWrap()
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

    //Updates pointer positin according to selected option
    private void UpdatePointer()
    {
        pointer.transform.position = new Vector2(40, optionsList[selectedOption].transform.position.y);
    }

    //Select actions for each menu item
    private void SelectMenuItem()
    {
        switch (selectedOption)
        {
            //Play button
            case 0:
                GoToGameplay();
                break;
            //Instructions button
            case 1:
                GoToInstructions();
                break;
            default:
                break;
        }
    }
}
