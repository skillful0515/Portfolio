using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpsCameraMove : MonoBehaviour
{
    public float speed = 200f;
    public Transform target;

    private float head, pitch;

    // Use this for initialization
    void Start()
    {

    }

    private void OnEnable()
    {
        Time.timeScale = 1.0f;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        transform.parent.transform.position = target.position;

        head += Input.GetAxis("CameraHorizontal") * speed * Time.deltaTime;
        pitch -= Input.GetAxis("CameraVertical") * speed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -30f, 60f);

        transform.parent.transform.eulerAngles = new Vector3(pitch, head, 0.0f);
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
