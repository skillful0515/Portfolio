using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTimeToScore : MonoBehaviour
{
    public GameSystem gameSystem;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore()
    {
        gameSystem.AddScore(Mathf.CeilToInt(gameSystem.time * 100f));
    }
}
