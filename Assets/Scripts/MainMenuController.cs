using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject StartMenuContainer;
    public GameObject SettingsMenuContainer;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowSettingsScreen()
    {
        StartMenuContainer.SetActive(false);
        SettingsMenuContainer.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        SettingsMenuContainer.SetActive(false);
        StartMenuContainer.SetActive(true);
    }
}
