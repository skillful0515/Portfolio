using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBox : MonoBehaviour
{
    public Vector3 pointPos;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        actMove();
    }

    void actMove()
    {
        pointPos = Input.mousePosition;

        pointPos.z = 4.5f;
    　　if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)
        || Input.GetKeyDown("s") || Input.GetKeyDown("g"))
        {
            pointPos.z = 3.0f;
        }

        transform.position = Camera.main.ScreenToWorldPoint(pointPos);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            if (Input.GetMouseButton(1))
            {
                other.GetComponent<Wall>().ChangeState(Wall.StateCell.Free);
                other.GetComponent<Wall>().IsChanged = true;
            }
            if (Input.GetMouseButton(0))
            {
                other.GetComponent<Wall>().ChangeState(Wall.StateCell.Wall);
                other.GetComponent<Wall>().IsChanged = true;
            }

            if (Input.GetKey("s"))
            {
                other.GetComponent<Wall>().ChangeState(Wall.StateCell.Start);
            }
            if (Input.GetKey("g"))
            {
                other.GetComponent<Wall>().ChangeState(Wall.StateCell.Goal);
            }

            //if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //    other.GetComponent<Wall>().ChangeArrow(Wall.StateArrows.Right);
            //}
            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    other.GetComponent<Wall>().ChangeArrow(Wall.StateArrows.Left);
            //}
            //if (Input.GetKey(KeyCode.DownArrow))
            //{
            //    other.GetComponent<Wall>().ChangeArrow(Wall.StateArrows.Up);
            //}
            //if (Input.GetKey(KeyCode.UpArrow))
            //{
            //    other.GetComponent<Wall>().ChangeArrow(Wall.StateArrows.Down);
            //}
        }
    }
}
