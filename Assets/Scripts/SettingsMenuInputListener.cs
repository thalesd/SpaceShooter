using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuInputListener : MonoBehaviour
{
    public MainMenuController mainMenuController;

    private void Awake()
    {
        mainMenuController = GameObject.Find("MainMenuControllerUI").GetComponent<MainMenuController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            mainMenuController.CloseSettingsMenu();
        }
    }
}
