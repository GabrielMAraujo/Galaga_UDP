using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MenuBase
{

    protected override void OnInputDown(InputTypeEnum input)
    {
        base.OnInputDown(input);

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
