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

public class RankingManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI firstName;
    [SerializeField] public TextMeshProUGUI firstScore;
    [SerializeField] public TextMeshProUGUI secondName;
    [SerializeField] public TextMeshProUGUI secondScore;
    [SerializeField] public TextMeshProUGUI thirdName;
    [SerializeField] public TextMeshProUGUI thirdScore;

    public GameObject darkMask;
    public Button selectButton;

    // Use this for initialization
    void Start()
    {
        darkMask.SetActive(true);

        GetJsonFromWww();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickClearDisplay()
    {
        //DisplayField.text = " ";
    }

    public void OnClickGetMessagesApi()
    {
        //DisplayField.text = "wait...";
        GetJsonFromWww();
    }

    public void OnClickSetMessageApi()
    {
        //DisplayField.text = "wait...";
        SetJsonFromWww();
    }

    public void OnClickSceneLoad()
    {
        SceneManager.LoadScene("Title");
    }

    private void GetJsonFromWww()
    {
        string sTgtURL = "http://localhost/kuragerankingsystem/kurageranking/getMessage";

        StartCoroutine(GetMessages(sTgtURL, CallbackWwwSuccess, CallbackWwwFailed));
    }

    private void CallbackWwwSuccess(string response)
    {
        List<RankingData> rankingList = RankingDataModel.DesirializeFromJson(response);

        string sStrOutPut = "";
        foreach (RankingData ranking in rankingList)
        {
            sStrOutPut += string.Format("Name:{0}\n", ranking.Name);
            sStrOutPut += string.Format("Score:{0}\n", ranking.Score);
            sStrOutPut += string.Format("Date:{0}\n", ranking.Date);
        }

        firstName.text = rankingList[0].Name;
        firstScore.text = "" + rankingList[0].Score;
        secondName.text = rankingList[1].Name;
        secondScore.text = "" + rankingList[1].Score;
        thirdName.text = rankingList[2].Name;
        thirdScore.text = "" + rankingList[2].Score;

        //DisplayField.text = sStrOutPut;
    }

    private void CallbackWwwFailed()
    {
        //DisplayField.text = "Www Failed";
    }

    private void CallbackApiSuccess(string response)
    {
        //DisplayField.text = response;
    }

    private IEnumerator GetMessages(string sTgtURL, Action<string> cbkSuccess = null, Action cbkFailed = null)
    {
        WWW www = new WWW(sTgtURL);

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

                // 暗幕削除
                darkMask.SetActive(false);

                selectButton.Select();
            }
        }
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

    private void SetJsonFromWww()
    {
        string sTgtURL = "http://localhost/kuragerankingsystem/kurageranking/setMessage";

        //string name = InputName.text;
        string score = GameMaster.score.ToString();

        StartCoroutine(SetMessage(sTgtURL, name, score, CallbackApiSuccess, CallbackWwwFailed));
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
}
