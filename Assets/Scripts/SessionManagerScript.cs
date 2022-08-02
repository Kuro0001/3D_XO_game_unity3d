using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SessionManagerScript : MonoBehaviour
{
    public GameObject panelInGame;
    public GameObject panelGameEnd;
    public GameObject panelPauseMenu;
    public GameObject panelSurrend;
    public GameObject buttonMenu;
    public TMP_Text textGameEnd;
    public TMP_Text textSequence;
    public RawImage imageTurn;
    public GameObject WebMenager;
    private WebScript webScript;
    public int vinner { get; set; }
    public int sequence { get; set; }
    public int SessionOption { get; set; }
    public bool PlayerTurn { get; set; }
    public bool CanMakeMove { get; set; }
    public float opponenTurnEnd = 0.0f;
    public float opponentPeriod = 180f;
    public void Start()
    {
        vinner = -1;
        SetStartSettings();
    }
    private void Update()
    {
        if (SessionOption == 2)
        {
            if (webScript.isListening)
                if (Time.time > opponenTurnEnd)
                {
                    if (sequence == 1)
                        vinner = 1;
                    else vinner = 2;
                    webScript.isListening = false;
                    GameEnd();
                }
        }
    }
    public void TurnEnd()
    {
        if (SessionOption == 1)
        {
            if (PlayerTurn)
            {
                SetInGameState(false);
                PlayerTurn = false;
            }
            else
            {
                SetInGameState(true);
                PlayerTurn = true;
            }
            CanMakeMove = true;
        }
        if (SessionOption == 2)
        {
            if (PlayerTurn)
            {
                opponenTurnEnd = Time.time + opponentPeriod;
                PlayerTurn = false;
                CanMakeMove = false;
                webScript.SendToOpponent();
                webScript.isListening = true;
                webScript.isOpponentMoveFound = false;
                SetInGameState(false);
            }
            else if (webScript.isOpponentMoveFound)
            {
                PlayerTurn = true;
                CanMakeMove = true;
                webScript.isOpponentMoveFound = false;
                webScript.isListening = false;
                SetInGameState(true);
            }
        }
    }
    public void SetStartSettings()
    {
        SessionOption = PlayerPrefs.GetInt("gameState");
        vinner = -1;
        SetPanelsState(false);
        OpenInGameMenu();
        PlayerTurn = true;
        CanMakeMove = true;
        SetInGameState(true);

        if (SessionOption == 2)
        {
            WebMenager.SetActive(true);
            webScript = WebMenager.GetComponent<WebScript>();
            webScript.Url = PlayerPrefs.GetString("serverURL");
            webScript.userId = PlayerPrefs.GetString("idUser");
            webScript.token = PlayerPrefs.GetString("token");
            webScript.GameFound = new Game(PlayerPrefs.GetInt("idGame"), PlayerPrefs.GetString("idGameUser1"), PlayerPrefs.GetString("idGameUser2"));
            if (webScript.userId.Equals(webScript.GameFound.user1Id))
                sequence = 1;
            else
                sequence = 2;

            if (webScript.GetLastState())
            {
                PlayerTurn = true;
                CanMakeMove = true;
                SetInGameState(true);
            }
            else
            {
                PlayerTurn = false;
                CanMakeMove = false;
                SetInGameState(false);
                opponenTurnEnd = Time.time + opponentPeriod;
            }
            PlayerPrefs.SetString("token", null);
        }
    }
    public void Surrend()
    {
        // todo добавить вызов метода вебскрипт - отправить на сервер что игрок сдался
        if (SessionOption == 2)
        {
            if (sequence == 1)
            {
                vinner = 2;
                webScript.SendGameResults("player 2 win");
            }
            else
            {
                vinner = 1;
                webScript.SendGameResults("player 1 win");
            }
            webScript.SendToOpponent("surrend");
        }

        if (SessionOption == 1)
            vinner = 3;
        GameEnd();
    }




    //======= Panels control ======
    public void SetPanelsState(bool state)
    {
        panelGameEnd.SetActive(state);
        panelPauseMenu.SetActive(state);
        panelSurrend.SetActive(state);
        panelInGame.SetActive(state);
        buttonMenu.SetActive(state);
        CanMakeMove = false;
    }
    public void OpenInGameMenu()
    {
        SetPanelsState(false);
        panelInGame.SetActive(true);
        buttonMenu.SetActive(true);
        if (SessionOption == 2 && PlayerTurn)
            CanMakeMove = true;
        else if (SessionOption == 1)
            CanMakeMove = true;
    }
    public void OpenPauseMenu()
    {
        SetPanelsState(false);
        panelPauseMenu.SetActive(true);
    }
    public void OpenSurrendMenu()
    {
        SetPanelsState(false);
        panelSurrend.SetActive(true);
    }
    public void GameEnd()
    {
        if (vinner > -1)
        {
            string resultText = string.Empty;
            if (SessionOption == 1)
            {
                if (vinner == 1)
                    resultText = "Win player 1";
                if (vinner == 2)
                    resultText = "Win player 2";
                if (vinner == 3)
                    resultText = "Draw. One of players surrended";
            }
            if (SessionOption == 2)
            {
                string resultWeb = string.Empty;
                if (vinner == 1)
                {
                    if (sequence == 1)
                        resultText = "You win";
                    else
                        resultText = "You lost";
                    resultWeb = "player 1 win";
                }
                if (vinner == 2)
                {
                    if (sequence == 1)
                        resultText = "You lost";
                    else
                        resultText = "You win";
                    resultWeb = "player 2 win";
                }
                if (vinner == 3)
                {
                    resultText = "Draw";
                    resultWeb = "draw";
                }
                if (vinner == 4)
                {
                    if (sequence == 1)
                        resultText = "You win,\nOpponent has committed a foul";
                    else
                        resultText = "You lost,\nYou has committed a foul";
                    resultWeb = "technical win - player 1";
                }
                if (vinner == 5)
                {
                    if (sequence == 1)
                        resultText = "You lost,\nYou has committed a foul";
                    else
                        resultText = "You win,\nOpponent has committed a foul";
                    resultWeb = "technical win - player 2";
                }
                if (vinner == 6)
                {
                    resultText = "You win,\nOpponent surrended";
                    if (sequence == 1)
                        resultWeb = "player 1 win";
                    else
                        resultWeb = "player 2 win";
                }
                webScript.isListening = false;
                webScript.SendGameResults(resultWeb);
            }
            SetPanelsState(false);
            CanMakeMove = false;
            panelGameEnd.SetActive(true);
            textGameEnd.text = resultText;
        }
    }
    public void RedirectToMainMenu()
    {
        SceneManager.LoadScene("Main_manu");
    }
    public void SetInGameState(bool state)
    {
        if(SessionOption == 1)
        {
            if (state)
            {
                textSequence.text = "1 player";
                imageTurn.color = Color.blue;
            }
            else
            {
                textSequence.text = "2 player";
                imageTurn.color = Color.red;
            }
        }
        if (SessionOption == 2)
        {
            if (state)
            {
                textSequence.text = "your turn";
                if (sequence == 1)
                    imageTurn.color = Color.blue;
                else
                    imageTurn.color = Color.red;
            }
            else
            {
                textSequence.text = "opponent turn";
                if (sequence == 1)
                    imageTurn.color = Color.red;
                else
                    imageTurn.color = Color.blue;
            }
        }
    }
}
