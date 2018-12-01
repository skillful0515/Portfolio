using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    public TextMeshProUGUI countDownText;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI timeToScoreText;

    public GameObject gameOverText;

    public float time = 100f;

    public GameObject blackEye01;
    public GameObject blackEye02;

    private int score;

    public bool isClear = false;

    // Use this for initialization
    void Start()
    {
        GameMaster.score = 0;
        scoreText.text = "SCORE : " + GameMaster.score;
        timeText.text = "TIME : " + time;

        gameOverText.SetActive(false);

        timeToScoreText.gameObject.SetActive(false);

        countDownText.gameObject.SetActive(true);

        StartCoroutine("CountDown");
    }

    // Update is called once per frame
    void Update()
    {
        if (isClear == false)
        {
            DecreaseTime();
        }
        else
        {
            if (timeToScoreText.gameObject.activeSelf == false)
            {
                string addTimeStr = time.ToString("f2");
                float addTime = float.Parse(addTimeStr);
                timeToScoreText.text = " + " + addTime + " * 100";
                timeToScoreText.gameObject.SetActive(true);
            }
        }

        if (Input.GetButtonDown("End"))
        {
            SceneManager.LoadScene("Title");
        }
    }

    private void DecreaseTime()
    {
        if (countDownText.gameObject.activeSelf == false)
        {
            time -= Time.deltaTime;

            if (time < 0) time = 0;

            timeText.text = "TIME : " + time.ToString("f2");

            if (time <= 0)
            {
                if (gameOverText.activeSelf == false)
                {
                    gameOverText.SetActive(true);

                    blackEye01.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
                    blackEye02.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);

                    StartCoroutine("RetryCoroutine");
                }
            }
        }
    }

    public void AddScore(int _value)
    {
        GameMaster.score += _value;

        scoreText.text = "SCORE : " + GameMaster.score;
    }

    private IEnumerator RetryCoroutine()
    {
        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator CountDown()
    {
        countDownText.text = "3";

        yield return new WaitForSeconds(1.0f);

        countDownText.text = "2";

        yield return new WaitForSeconds(1.0f);

        countDownText.text = "1";

        yield return new WaitForSeconds(1.0f);

        countDownText.text = "START";

        yield return new WaitForSeconds(1.0f);

        countDownText.gameObject.SetActive(false);
    }
}
