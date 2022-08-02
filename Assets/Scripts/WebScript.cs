using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Net;
using System.IO;
using UnityEngine.Networking;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public struct GamesState
{
    /*
    {
    "id":68,
    "gameId":60,
    "userId":"6cbe267e-608c-4c9e-99ce-75e4bc03086a",
    "isReaded":true,
    "state":"000000000000000000000100000000000000010000000000000000",
    "time":"2022-06-15T12:28:57.2448085"}

*/  public int id;
    public int gameId;
    public string userId;
    public bool isReaded;
    public string state;
    public DateTime time;
}
public struct RegisterModel
{
    public string Username;
    public string Password;
    public string Email;
}
public struct GameResult
{
    public int Id;
    public string Name;
}
public struct Game
{
    public int id;
    public string user1Id;
    public string user2Id;
    public string result;
    public DateTime date;
    public List<GamesStates> states;

    public Game(int Id, string User1Id, string User2Id)
    {
        this.id = Id;
        this.user1Id = User1Id;
        this.user2Id = User2Id;
        date = DateTime.Now;
        result = "continues";
        states = new List<GamesStates>();
    }
}
public struct GamesStates
{
    public int GameId;
    public int UserId;
    public bool IsReaded;
    public string GameState;
}

public struct Token
{
    public string token;
    public string expiration;
    public string idUser;
} 

public class WebScript : MonoBehaviour
{
    public GameObject MainCube;
    public string Url { get; set; }
    public string token { get; set; }
    public string userId { get; set; }
    public bool isSearching { get; set; }
    public bool isListening { get; set; }
    public bool isGameFound { get; set; }
    public bool isOpponentMoveFound { get; set; }
    public float nextActionTime = 0.0f;
    public float period = 1f;
    public Game GameFound { get; set; }


    private void Update()
    {
        if (isSearching)
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                Search();
            }
        if (isListening)
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                ListenOpponentAction();
            }
        }
    }
    private void Start()
    {
        if (GameFound.id > 0)
            isGameFound = true;
        else
        {
            isSearching = false;
            isListening = false;
            isGameFound = false;
            isOpponentMoveFound = false;
        }
    }
    public void LogOut()
    {
        token = null;
        userId = null;
    }
    public bool LogIn(string username, string password)
    {
        HttpWebResponse httpResponse = new HttpWebResponse();
        try
        {
            string url = String.Format("{0}/api/authenticate/login-in-game", Url);
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/xml";
            string data = $"<LoginModel><Username>{username}</Username><Password>{password}</Password></LoginModel>";
            byte[] byteArray = Encoding.UTF8.GetBytes(data.ToString());

            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(byteArray, 0, byteArray.Length);
            requestStream.Close();

            string result = null;
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Debug.Log(httpResponse.StatusCode);
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    {
                        return false;
                    }
            }
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                Token jTokent = JsonUtility.FromJson<Token>(result);
                token = String.Empty;
                token = jTokent.token.ToString();
                userId = jTokent.idUser.ToString();
                Debug.Log("login: successe");
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
            return false;
        }
        finally
        {
            httpResponse.Close();
        }
    }
    public bool StartSearch()
    {
        HttpWebResponse httpResponse = new HttpWebResponse();
        try
        {
            string url = String.Format("{0}/SearchingUser/start-search-2/{1}", Url, userId);
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/xml";
            httpRequest.ContentLength = 0;
            httpRequest.Headers.Add("Authorization", "Bearer " + token);
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Debug.Log(httpResponse.StatusCode);
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        string result = string.Empty;
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                        }
                        GameFound = JsonUtility.FromJson<Game>(result);
                        if (GameFound.id < 0)
                            Debug.Log("!!!!! error in answer (search start) !!!!");
                        isSearching = false;
                        isGameFound = true;
                        Debug.Log("search game: started, game is fouund.");
                        return true;
                    }
                    case HttpStatusCode.NoContent:
                    {
                        isSearching = true;
                        Debug.Log("search game: started active search");
                        return false;
                    }
                default:
                    {
                        isSearching = true;
                        Debug.Log("search game: started active search. Error: " + httpResponse.StatusCode);
                        return false;
                    }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
            return false;
        }
        finally
        {
            httpResponse.Close();
        }
    }
    public void Search()
    {
        HttpWebResponse httpResponse = new HttpWebResponse();
        try
        {
            isSearching = false;
            string url = String.Format("{0}/searchinguser/game-search-2/{1}", Url, userId);
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/xml";
            httpRequest.ContentLength = 0;
            httpRequest.Headers.Add("Authorization", "Bearer " + token);
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        string result = string.Empty;
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                        }
                        GameFound = JsonUtility.FromJson<Game>(result);
                        if (GameFound.id < 0)
                            Debug.Log("!!!!! error in answer (search continues) !!!!");
                        Debug.Log("search game: ended - successfuly");
                        isSearching = false;
                        isGameFound = true;
                        break;
                    }
                case HttpStatusCode.NoContent:
                    {
                        Debug.Log("search game: no content");
                        isSearching = true;
                        break;
                    }
                default:
                    {
                        Debug.Log("search game: error: " + httpResponse.StatusCode);
                        isSearching = true;
                        break;
                    }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
        finally
        {
            httpResponse.Close();
        }
    }

    public void ListenOpponentAction()
    {
        string url = String.Format("{0}/gamesstate/get-state/{1}&{2}", Url, GameFound.id, userId);
        HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse httpResponse = new HttpWebResponse();
        try
        {
            httpRequest.Method = "GET";
            httpRequest.ContentType = "application/xml";
            httpRequest.ContentLength = 0;
            httpRequest.ProtocolVersion = HttpVersion.Version10;
            httpRequest.Timeout = 5000;
            httpRequest.Headers.Add("Authorization", "Bearer " + token);
            string result = string.Empty;
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            //HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Debug.Log(httpResponse.StatusCode);
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        isListening = false;
                        isOpponentMoveFound = true;
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                        }
                        httpResponse.Close();

                        GamesState state = JsonUtility.FromJson<GamesState>(result);

                        if (state.state.Equals("surrend"))
                        {
                            Debug.Log("listen opponent: opponent surrended");
                            MainCube.GetComponent<CubeManager>().ToGameEnd(6);
                        }
                        else if (state.state.Equals("technical win - player 1"))
                        {
                            Debug.Log("listen opponent: player 1 has committed a foul");
                            MainCube.GetComponent<CubeManager>().ToGameEnd(4);
                        }
                        else if (state.state.Equals("technical win - player 2"))
                        {
                            Debug.Log("listen opponent: player 2 has committed a foul");
                            MainCube.GetComponent<CubeManager>().ToGameEnd(5);
                        }
                        else
                        {
                            if (IsTurnValidMark(MainCube.GetComponent<CubeManager>().CubePack(), state.state))
                            {
                                MainCube.GetComponent<CubeManager>().CubeUnpack(state.state);
                            }
                            else
                            {
                                SendReportAboutViolation();
                                if (GameFound.user1Id.Equals(userId))
                                {
                                    SendToOpponent("technical win - player 1");
                                    MainCube.GetComponent<CubeManager>().ToGameEnd(4);
                                }
                                else
                                {
                                    SendToOpponent("technical win - player 2");
                                    MainCube.GetComponent<CubeManager>().ToGameEnd(5);
                                }
                            }
                        }
                        break;
                    }
                case HttpStatusCode.NoContent:
                    {
                        Debug.Log("lissten opponent: no content");
                        break;
                    }
                default:
                    {
                        Debug.Log("lissten opponent: error");
                        break;
                    }
            }

        }
        catch (WebException ex)
        {
            using (var stream = ex?.Response?.GetResponseStream())
                if (stream != null)
                    using (var reader = new StreamReader(stream))
                    {
                        Debug.Log(reader.ReadToEnd());
                    }
            // todo...
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        finally
        {
            httpResponse.Close();
        }
    }
    /// <summary>
    /// </summary>
    /// <returns>
    /// true - ход этого игрока;
    /// false - ход оппонента.
    /// </returns>
    public bool GetLastState()
    {
        HttpWebResponse httpResponse = new HttpWebResponse();
        try
        {
            string url = String.Format("{0}/gamesstate/get-last-state/{1}&{2}", Url, GameFound.id, userId);
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "GET";
            httpRequest.ContentType = "application/xml";
            httpRequest.ContentLength = 0;
            httpRequest.Headers.Add("Authorization", "Bearer " + token);
            string result = string.Empty;
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Debug.Log(httpResponse.StatusCode);
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                        }
                        Debug.Log("last state: readed - successfuly");
                        GamesState state = JsonUtility.FromJson<GamesState>(result);
                        if (state.state.Contains("start"))
                        {
                            if (state.userId.Equals(userId))
                            {
                                isListening = false;
                                isOpponentMoveFound = false;
                                return true;
                            }
                            else
                            {
                                isListening = true;
                                isOpponentMoveFound = false;
                                return false;
                            }
                        }
                        else
                        {
                            MainCube.GetComponent<CubeManager>().CubeUnpackStart(state.state);
                            if (state.Equals(userId))
                            {
                                isListening = true;
                                isOpponentMoveFound = false;
                                return false;
                            }
                            else
                            {
                                isListening = false;
                                isOpponentMoveFound = false;
                                return true;
                            }
                            break;
                        }
                    }
                default:
                    {
                        Debug.Log("last state: error");
                        return false;
                    }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
            return false;
        }
        finally
        {
            httpResponse.Close();
        }
    }
    public void SendReportAboutViolation()
    {
        HttpWebResponse httpResponse = new HttpWebResponse();
        try
        {
            string url = String.Format("{0}/api/authenticate/set-violation/{1}", Url, PlayerPrefs.GetString("idOpponent"));
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "PUT";
            httpRequest.ContentType = "application/xml";
            httpRequest.ContentLength = 0;
            httpRequest.Headers.Add("Authorization", "Bearer " + token);
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Debug.Log(httpResponse.StatusCode);
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        Debug.Log("Send violation: success");
                        break;
                    }
                default:
                    {
                        Debug.Log("Error: " + httpResponse.StatusCode);
                        break;
                    }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
        finally
        {
            httpResponse.Close();
        }
    }
    public void SendGameResults(string gameresult)
    {
        HttpWebResponse httpResponse = new HttpWebResponse();
        try
        {
            string url = String.Format("{0}/game/end-game/{1}&{2}", Url, GameFound.id, gameresult);
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "PUT";
            httpRequest.ContentType = "application/xml";
            httpRequest.ContentLength = 0;
            httpRequest.Headers.Add("Authorization", "Bearer " + token);
            string result = string.Empty;
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Debug.Log(httpResponse.StatusCode);
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        Debug.Log("Send game result: successfuly");
                        break;
                    }
                default:
                    {
                        Debug.Log("Error: " + httpResponse.StatusCode);
                        break;
                    }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
        finally
        {
            httpResponse.Close();
        }
    }

    public bool IsTurnValidMark(string stateCurrent, string stateNew)
    {
        int changes = 0;
        if (CalculateMarks(stateCurrent)[0] != CalculateMarks(stateNew)[0] || CalculateMarks(stateCurrent)[1] != CalculateMarks(stateNew)[1])
        {
            for (int i = 0; i < stateCurrent.Length; i++)
            {
                if (!stateCurrent[i].Equals(stateNew[i]))
                    changes++;
                if (changes > 1)
                    return false;
            }
        }
        return true;
    }
    public int[] CalculateMarks(string state)
    {
        int[] result = new int[2];
        foreach (char c in state)
            if (c.Equals(1))
                result[0]++;
            else if (c.Equals(2))
                result[1]++;
        return result;
    }
    public void SendToOpponent()
    {
        string url = String.Format("{0}/GamesState/set-state/{1}&{2}&{3}", Url, GameFound.id, userId, MainCube.GetComponent<CubeManager>().CubePack());
        HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse httpResponse = new HttpWebResponse();
        try
        {
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/xml";
            httpRequest.ContentLength = 0;
            httpRequest.Headers.Add("Authorization", "Bearer " + token);
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        Debug.Log("send to opponent: successful");
                        break;
                    }
                default:
                    {
                        Debug.Log("send to opponent: error - " + httpResponse.StatusCode);
                        break;
                    }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
        finally
        {
            httpResponse.Close();
        }
    }
    public void SendToOpponent(string state)
    {
        string url = String.Format("{0}/GamesState/set-state/{1}&{2}&{3}", Url, GameFound.id, userId, state);
        HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse httpResponse = new HttpWebResponse();
        try
        {
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/xml";
            httpRequest.ContentLength = 0;
            httpRequest.Headers.Add("Authorization", "Bearer " + token);
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        Debug.Log("send to opponent: successful");
                        break;
                    }
                default:
                    {
                        Debug.Log("send to opponent: error - " + httpResponse.StatusCode);
                        break;
                    }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
        finally
        {
            httpResponse.Close();
        }
    }
    public string Registrate(string username, string password, string email)
    {
        string url = String.Format("{0}/api/authenticate/register", Url);
        HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
        httpRequest.Method = "POST";
        httpRequest.ContentType = "application/xml";

        XmlDocument doc = new XmlDocument();
        doc.CreateXmlDeclaration("1.0", "utf-8", "no");
        RegisterModel model = new RegisterModel()
        {
            Username = username,
            Password = password,
            Email = email
        };
        XmlElement workNode = PackToXmlRegisterModel(doc, model);
        doc.AppendChild(workNode);
        //получаем строку в формате XML
        string xml = doc.OuterXml;
        //преобразуем строку в массив байт
        byte[] byteArray = Encoding.UTF8.GetBytes(xml);
        Stream requestStream = httpRequest.GetRequestStream();
        requestStream.Write(byteArray, 0, byteArray.Length);
        requestStream.Close();


        string result = null;
        HttpWebResponse httpResponse = new HttpWebResponse();
        try
        {
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Debug.Log(httpResponse.StatusCode);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        LogIn(username, password);
                        return "success";
                    }
                default:
                    {
                        return "Error";
                    }
            }
        }
        catch
        {
            Debug.Log(httpResponse.StatusCode);
            return "Error";
        }
        finally
        {
            httpResponse.Close();
        }
    }
    public bool CheckActiveGames()
    {
        try
        {
            string url = String.Format("{0}/game/start-active-game-search/{1}", Url, userId);
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "GET";
            httpRequest.ContentType = "application/xml";
            httpRequest.ContentLength = 0;

            httpRequest.Headers.Add("Authorization", "Bearer " + token);
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        string result = string.Empty;
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                        }
                        GameFound = JsonUtility.FromJson<Game>(result);
                        if (result.Contains("wait"))
                            Debug.Log("wait");
                        isGameFound = true;
                        Debug.Log("search game: ended - successfuly");
                        return true;
                    }
                case HttpStatusCode.NoContent:
                    {
                        Debug.Log("search game: no content");
                        return false;
                    }
                default:
                    {
                        string result = string.Empty;
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                        }
                        Debug.Log("search game: error - " + result);
                        return false;
                    }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
            return false;
        }
    }



    private XmlElement PackToXmlRegisterModel(XmlDocument doc, RegisterModel note)
    {
        XmlElement workNode = doc.CreateElement("RegisterModel");

        XmlElement Node1 = doc.CreateElement("Username");
        Node1.InnerText = note.Username;
        workNode.AppendChild(Node1);

        XmlElement Node2 = doc.CreateElement("Email");
        Node2.InnerText = note.Email;
        workNode.AppendChild(Node2);

        XmlElement Node3 = doc.CreateElement("Password");
        Node3.InnerText = note.Password;
        workNode.AppendChild(Node3);

        return workNode;
    }

    private XmlElement PackToXmlGamesStateModel(XmlDocument doc, GamesState note)
    {
        XmlElement workNode = doc.CreateElement("GamesState");

        XmlElement Node0 = doc.CreateElement("Id");
        Node0.InnerText = note.id.ToString();
        workNode.AppendChild(Node0);

        XmlElement Node1 = doc.CreateElement("GameId");
        Node1.InnerText = note.gameId.ToString();
        workNode.AppendChild(Node1);

        XmlElement Node2 = doc.CreateElement("UserId");
        Node2.InnerText = note.userId;
        workNode.AppendChild(Node2);

        XmlElement Node3 = doc.CreateElement("IsReaded");
        Node3.InnerText = note.isReaded.ToString();
        workNode.AppendChild(Node3);

        XmlElement Node4 = doc.CreateElement("State");
        Node4.InnerText = note.state;
        workNode.AppendChild(Node4);

        XmlElement Node5 = doc.CreateElement("Time");
        Node5.InnerText = GetDate(DateTime.Now);
        workNode.AppendChild(Node5);

        return workNode;
    }

    public static string GetDate(DateTime date)
    {
        string year = string.Empty;
        string result = string.Empty;
        string month = string.Empty;
        string day = string.Empty;
        string hour = string.Empty;
        string minute = string.Empty;
        string second = string.Empty;

        if (date.Year < 10)
            year = "000" + date.Year.ToString();
        else
            if (date.Year < 100)
            year = "00" + date.Year.ToString();
        else if (date.Year < 1000)
            year = "0" + date.Year.ToString();
        else
            year = date.Year.ToString();

        if (date.Month < 10)
            month = '0' + date.Month.ToString();
        else
            month = date.Month.ToString();
        if (date.Day < 10)
            day = '0' + date.Day.ToString();
        else
            day = date.Day.ToString();
        if (date.Hour < 10)
            hour = '0' + date.Hour.ToString();
        else
            hour = date.Hour.ToString();
        if (date.Minute < 10)
            minute = '0' + date.Minute.ToString();
        else
            minute = date.Minute.ToString();
        if (date.Second < 10)
            second = '0' + date.Second.ToString();
        else
            second = date.Second.ToString();

        // format: 1997-06-10T00:00:00
        result += year + '-' + month + '-' + day + "T" + hour + ':' + minute + ':' + second;
        return result;
    }
}
