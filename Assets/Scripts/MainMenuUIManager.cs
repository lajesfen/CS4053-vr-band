using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject uiMain;
    public GameObject uiInstructions;

    private void Start()
    {

    }

    public void ShowInstructionsMenu()
    {
        HideAllMenus();
        uiInstructions.SetActive(true);
    }

    public void ShowMainMenu()
    {
        HideAllMenus();
        uiMain.SetActive(true);
    }

    private void HideAllMenus()
    {
        uiMain.SetActive(false);
        uiInstructions.SetActive(false);
    }

    public void JoinGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
