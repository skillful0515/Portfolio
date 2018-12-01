using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSystem : MonoBehaviour
{

    public Button selectButton;

    // Use this for initialization
    void Start()
    {
        selectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("End"))
        {
            Application.Quit();
        }
    }

    public void OnClickSceneLoad(string _name)
    {
        SceneManager.LoadScene(_name);
    }
}
