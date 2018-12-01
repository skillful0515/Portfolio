using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUIMove : MonoBehaviour
{
    public float speed;

    private Text damageText;
    private float alpha;
    private float fadeOutSpeed = 1f;

    private Transform nowCamera;

    // Use this for initialization
    void Start()
    {
        damageText = GetComponentInChildren<Text>();

        alpha = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        alpha -= fadeOutSpeed * Time.deltaTime;
        damageText.color = new Color(1f, 0f, 0f, alpha);

        transform.rotation = nowCamera.transform.rotation;
        transform.position += Vector3.up * speed * Time.deltaTime;

        if (alpha < 0f)
        {
            Destroy(gameObject);
        }
    }

    public Transform SetNowCamera
    {
        set
        {
            nowCamera = value;
        }
    }
}
