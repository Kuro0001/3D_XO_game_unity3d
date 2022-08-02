using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public GameObject panelMainMenu;
    public GameObject panelSettings;
    public GameObject panelRegistration;
    public GameObject panelAuthorisation;
    public GameObject panelForPvP;
    public GameObject panelMessage;
    public GameObject panelSearching;
    public TMP_InputField textUrl;
    public TMP_InputField textUsername;
    public TMP_InputField textPassword;
    public TMP_InputField textUserNameRegistrate;
    public TMP_InputField textPasswordRegistrate;
    public TMP_InputField textEmailRegistrate;
    public TMP_Text textMessage;
    public GameObject buttonLogIn;
    public GameObject buttonLogOut;
    public GameObject webMenager;
    private WebScript webScript;
    private bool isFullScreen;
    private bool isUserDataSave;



    // Start is called before the first frame update
    void Start()
    {
        SetAllPanels(false);
        panelMainMenu.SetActive(true);
        SetButtonsLogInOut(false);
        isFullScreen = Screen.fullScreen;
        isUserDataSave = true;
        webScript = webMenager.GetComponent<WebScript>();
        SetDefoltInput();
    }

    // Update is called once per frame
    void Update()
    {
        if (webScript.isGameFound)
            BeginOnlineGame();
    }


    //======= Panel Controls =======
    public void SetAllPanels(bool state)
    {
        panelMainMenu.SetActive(state);
        panelSettings.SetActive(state);
        panelRegistration.SetActive(state);
        panelAuthorisation.SetActive(state);
        panelForPvP.SetActive(state);
        panelMessage.SetActive(false);
        panelSearching.SetActive(false);
    }
    public void OpenMainMenu()
    {
        SetAllPanels(false);
        panelMainMenu.SetActive(true);
    }
    public void OpenSettings()
    {
        SetAllPanels(false);
        panelSettings.SetActive(true);
    }
    public void OpenAuthorisationPanel()
    {
        SetAllPanels(false);
        panelAuthorisation.SetActive(true);
    }
    public void OpenRegistrationPanel()
    {
        SetAllPanels(false);
        panelRegistration.SetActive(true);
    }
    public void OpenPvPPanel()
    {
        SetAllPanels(false);
        panelForPvP.SetActive(true);
    }
    public void OpenSearchingPanel()
    {
        SetAllPanels(false);
        panelSearching.SetActive(true);
    }
    public void ExitPressed()
    {
        Debug.Log("Pressed - exit");
        Application.Quit();
    }
    public void OpenMessage(string text)
    {
        panelMessage.SetActive(true);
        textMessage.text = text;
    }
    public void CloseMessage()
    {
        panelMessage.SetActive(false);
    }
    public void FullScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        if (!isFullScreen)
            Screen.SetResolution(800, 600, false);
        else
            Screen.SetResolution(1900, 1080, true);
    }
    public void ToSaveUserDataToggle()
    {
        isUserDataSave = !isUserDataSave;
        SetUserData();
    }
    public void SetUserData()
    {
        if (!isUserDataSave)
        {
            PlayerPrefs.SetString("username", null);
            PlayerPrefs.SetString("password", null);
        }
        else
        {
            PlayerPrefs.SetString("username", textUsername.text);
            PlayerPrefs.SetString("password", textPassword.text);
        }
    }
    public void SetUserData(string userName, string passWord)
    {
        if (!isUserDataSave)
        {
            PlayerPrefs.SetString("username", null);
            PlayerPrefs.SetString("password", null);
        }
        else
        {
            PlayerPrefs.SetString("username", userName);
            PlayerPrefs.SetString("password", passWord);
            textUsername.text = userName;
            textPassword.text = passWord;
        }
    }
    public void SetButtonsLogInOut(bool state)
    {
        if (state)
        {
            buttonLogIn.SetActive(false);
            buttonLogOut.SetActive(true);
        }
        else
        {
            buttonLogIn.SetActive(true);
            buttonLogOut.SetActive(false);
        }
    }

    //======= Session Controls =======
    public void SetDefoltInput()
    {
        textUrl.text = PlayerPrefs.GetString("serverURL");
        webScript.Url = textUrl.text;
        if (textUrl.text != null)
            if (PlayerPrefs.GetString("username") != "" && PlayerPrefs.GetString("username") != null)
                if (PlayerPrefs.GetString("password") != "" && PlayerPrefs.GetString("password") != null)
                {
                    textUsername.text = PlayerPrefs.GetString("username");
                    textPassword.text = PlayerPrefs.GetString("password");
                    if (webScript.LogIn(textUsername.text, textPassword.text))
                    {
                        OpenMessage("Àutomatic authorization successful");
                        SetButtonsLogInOut(true);
                        webScript.CheckActiveGames();
                        if (webScript.isGameFound)
                            BeginOnlineGame();
                    }
                    else
                        OpenMessage("Automatic authorization failed");
                }
    }
    public void LogIn()
    {
        if (!PlayerPrefs.GetString("serverURL").Equals(textUrl.text))
            SetURL(textUrl.text);
        if (textUrl.text != null)
            if (textUsername.text != null)
                if (textPassword.text != null)
                {
                    SetUserData();
                    if (webScript.LogIn(textUsername.text, textPassword.text))
                    {
                        OpenMessage("Authorization successful");
                        SetButtonsLogInOut(true);
                        if (webScript.CheckActiveGames())
                            BeginOnlineGame();
                    }
                    else
                        OpenMessage("Authorization failed");
                }
    }

    public void RegistrateUser()
    {
        if (!PlayerPrefs.GetString("serverURL").Equals(textUrl.text))
            SetURL(textUrl.text);
        if (textUrl.text != null)
            if (textUserNameRegistrate.text != null)
                if (textPasswordRegistrate.text != null)
                    if (textEmailRegistrate.text != null)
                    {
                        string result = webScript.Registrate(textUserNameRegistrate.text, textPasswordRegistrate.text, textEmailRegistrate.text);
                        if (result == "success")
                        {
                            SetUserData(textUserNameRegistrate.text, textPasswordRegistrate.text);
                            OpenMessage(result);
                            SetButtonsLogInOut(true);
                            if (webScript.CheckActiveGames())
                                BeginOnlineGame();
                        }
                        else
                            OpenMessage(result);
                    }
    }
    public void LogOut()
    {
        webScript.LogOut();
        SetButtonsLogInOut(false);
        OpenMessage("LogOut successfully");
    }




    public void SetURL(string url)
    {
        PlayerPrefs.SetString("serverURL", url);
        webScript.Url = url;
    }
    public void PlayVSPcPressed()
    {
        Debug.Log("PlayPressed!");
        SetSessionAtributes(null, 0, null, null, 0, 0, null);
        SceneManager.LoadScene("Scane_Game");
    }
    public void PlayVSPlayerPressed()
    {
        Debug.Log("Pressed - Play VS Player");
        SetSessionAtributes(null, 0, null, null, 1, 0, null);
        SceneManager.LoadScene("Scane_Game");
    }
    public void PlayVSOnlinePlayerPressed()
    {
        if (webScript.userId != null)
        {
            Debug.Log("Pressed - Play VS Online player: start search");
            webScript.StartSearch();
            OpenSearchingPanel();
        }
        else
            OpenMessage("Before online game is needed to log in");
    }
    public void BeginOnlineGame()
    {
        Debug.Log("Online game: forwarding to next scene");
        int sequence = 1;
        if (webScript.userId.Equals(webScript.GameFound.user1Id))
            sequence = 2;
        SetSessionAtributes(webScript.userId, webScript.GameFound.id, webScript.GameFound.user1Id, webScript.GameFound.user2Id, 2, sequence, webScript.token);
        SceneManager.LoadScene("Scane_Game");
    }
    public void SetSessionAtributes(string idUser, int idGame, string idGameUser1, string idGameUser2, int gameState, int sequence, string token)
    {
        PlayerPrefs.SetString("idUser", idUser);
        PlayerPrefs.SetInt("idGame", idGame);
        PlayerPrefs.SetString("idGameUser1", idGameUser1);
        PlayerPrefs.SetString("idGameUser2", idGameUser2);
        PlayerPrefs.SetInt("gameState", gameState);
        PlayerPrefs.SetInt("sequence", sequence);
        PlayerPrefs.SetString("token", token);
    }
}
