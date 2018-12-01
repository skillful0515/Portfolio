using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MiniJSON;
using System;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultSystem : MonoBehaviour
{
    public Text inputName;
    public TextMeshProUGUI scoreText;
    public Text messageBox;
    public Button sendButton;

    public InputField selectField;

    // Use this for initialization
    void Start()
    {
        scoreText.text = GameMaster.score.ToString();

        selectField.Select();
    }

    // Update is called once per frame
    void Update()
    {
        // インプットに入力が無い状態ではSendできないように
        if (inputName.text == "")
        {
            if (sendButton.interactable == true)
            {
                sendButton.interactable = false;
            }
        }
        else
        {
            if (sendButton.interactable == false)
            {
                sendButton.interactable = true;
            }
        }
    }

    public void OnEndEditSelectButton(Button _button)
    {
        _button.Select();
    }

    private IEnumerator LoadRanking()
    {
        yield return new WaitForSeconds(3.0f);
    }

    public void OnClickSetMessageApi()
    {
        messageBox.text = "wait...";
        SetJsonFromWww();
    }

    private void SetJsonFromWww()
    {
        string sTgtURL = "http://localhost/kuragerankingsystem/kurageranking/setMessage";

        string name = inputName.text;
        string score = GameMaster.score.ToString();

        StartCoroutine(SetMessage(sTgtURL, name, score, CallbackApiSuccess, CallbackWwwFailed));
    }

    private void CallbackWwwFailed()
    {
        messageBox.text = "Www Failed";
    }

    private void CallbackApiSuccess(string response)
    {
        messageBox.text = response;

        // シーン遷移
        SceneManager.LoadScene("RankingScene");
    }

    private IEnumerator ResponceCheckForTimeOutWWW(WWW www, float timeout)
    {
        float requestTime = Time.time;

        while (!www.isDone)
        {
            if (Time.time - requestTime < timeout)
            {
                yield return null;
            }
            else
            {
                Debug.LogWarning("TimeOut");
                break;
            }
        }
        yield return null;
    }

    private IEnumerator SetMessage(string urlTarget, string name, string score, Action<string> cbkSuccess = null, Action cbkFailed = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("score", score);

        WWW www = new WWW(urlTarget, form);

        yield return StartCoroutine(ResponceCheckForTimeOutWWW(www, 5.0f));

        if (www.error != null)
        {
            Debug.LogError(www.error);
            if (null != cbkFailed)
            {
                cbkFailed();
            }
        }
        else if (www.isDone)
        {
            Debug.Log(string.Format("Success:{0}", www.text));
            if (null != cbkSuccess)
            {
                cbkSuccess(www.text);
            }
        }
    }

    public void OnClickSceneLoad()
    {
        SceneManager.LoadScene("RankingScene");
    }
}
