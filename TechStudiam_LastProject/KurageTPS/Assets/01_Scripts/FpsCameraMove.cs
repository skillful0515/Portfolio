using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCameraMove : MonoBehaviour
{
    public float speed = 200f;
    public float slowSpeed = 0.5f;

    private float head, pitch;

    // Use this for initialization
    void Start()
    {

    }

    private void OnEnable()
    {
        Time.timeScale = slowSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        head += Input.GetAxis("CameraHorizontal") * speed * Time.deltaTime;
        pitch -= Input.GetAxis("CameraVertical") * speed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -80f, 80f);

        transform.eulerAngles = new Vector3(pitch, head, 0.0f);
    }

    public float Head
    {
        get
        {
            return head;
        }
        set
        {
            head = value;
        }
    }

    public float Pitch
    {
        get
        {
            return pitch;
        }
        set
        {
            pitch = value;
        }
    }
}
