using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject uiMain;
    public GameObject uiJoin;
    public GameObject uiCreate;
    public GameObject uiInstructions;

    [Header("Create Room UI ")]
    public TMP_Text createRoomCodeText;

    [Header("Join Room UI ")]
    public TMP_InputField joinCodeInput;
    public TMP_Text joinErrorText;
    public float errorDisplayDuration = 2f;

    private TouchScreenKeyboard keyboard;

    private void Start()
    {
        joinErrorText?.gameObject.SetActive(false);
    }

    public void ShowJoinMenu()
    {
        HideAllMenus();
        uiJoin.SetActive(true);
    }

    public void ShowCreateMenu()
    {
        HideAllMenus();
        uiCreate.SetActive(true);
        GenerateAndDisplayRoomCode();
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

    public void ShowKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    private void HideAllMenus()
    {
        uiMain.SetActive(false);
        uiJoin.SetActive(false);
        uiCreate.SetActive(false);
        uiInstructions.SetActive(false);
    }

    private void GenerateAndDisplayRoomCode()
    {
        string roomCode = GenerateRandomRoomCode(5);
        createRoomCodeText.text = roomCode;
    }

    private string GenerateRandomRoomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char[] code = new char[length];
        for (int i = 0; i < length; i++)
        {
            code[i] = chars[Random.Range(0, chars.Length)];
        }
        return new string(code);
    }

    private void JoinRoom(string sessionCode)
    {
        NetworkManager.Instance.JoinSession(sessionCode);
        Debug.Log("Joining room...");
        SceneManager.LoadScene("MainScene");
    }

    public void CreateAndJoinRoom()
    {
        NetworkManager.Instance.CreateSession(createRoomCodeText.text);
        JoinRoom(createRoomCodeText.text);
        Debug.Log("Creating and joining room...");
    }

    public void TryJoinRoom()
    {
        string code = joinCodeInput.text;

        if (IsCodeValid(code))
        {
            JoinRoom(code);
        }
        else
        {
            StartCoroutine(ShowJoinError("Invalid room code. Please enter a valid 5 character code."));
        }
    }

    private bool IsCodeValid(string code)
    {
        return code.Length == 5 && System.Text.RegularExpressions.Regex.IsMatch(code, "^[A-Z]+$");
    }

    private IEnumerator ShowJoinError(string message)
    {
        joinErrorText.text = message;
        joinErrorText.gameObject.SetActive(true);

        yield return new WaitForSeconds(errorDisplayDuration);

        joinErrorText.gameObject.SetActive(false);
        joinErrorText.text = "";
    }
}
