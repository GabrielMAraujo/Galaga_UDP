using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    MainMenu mainMenu;


    // Start is called before the first frame update
    void Start()
    {
        mainMenu = FindObjectOfType<MainMenu>();
    }

    public void CloseInstructions()
    {
        mainMenu.CloseInstructions();
    }
}
