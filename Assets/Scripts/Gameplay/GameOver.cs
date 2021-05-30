using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MenuBase
{
    protected override void Start()
    {
        pointerXPos = 270;
        base.Start();
    }

    protected override void OnInputDown(InputTypeEnum input)
    {
        base.OnInputDown(input);
        if (input == InputTypeEnum.SELECT)
        {
            SelectMenuItem();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    //Select actions for each menu item
    protected override void SelectMenuItem()
    {
        switch (selectedOption)
        {
            //Restart
            case 0:
                RestartGame();
                break;
            //Main menu
            case 1:
                GoToMenu();
                break;
            default:
                break;
        }
    }
}
