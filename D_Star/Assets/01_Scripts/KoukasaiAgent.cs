using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoukasaiAgent : MonoBehaviour
{
    [SerializeField]
    public List<Wall> elementsWall = new List<Wall>();

    private float time = 0f;
    private Vector3 randomPos;
    public float randomRange;

    // Use this for initialization
    void Start()
    {
        randomPos = new Vector3(Random.Range(-randomRange, randomRange), 0.5f, Random.Range(-randomRange, randomRange));

        GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1.0f), Random.Range(0f, 1.0f), Random.Range(0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (elementsWall.Count > 1)
        {
            time += Time.deltaTime;
            transform.position = (1.0f - time) * elementsWall[0].transform.position + time * elementsWall[1].transform.position;
            transform.position += randomPos;


            if (time >= 1.0f)
            {
                elementsWall.RemoveAt(0);
                time = 0f;
                if (elementsWall.Count == 1)
                {
                    Destroy(gameObject, 1.0f);
                }
            }
        }
    }

    public void SetRoutes(Route _route)
    {
        for (int i = 0; i < _route.elementsWall.Count; i++)
        {
            elementsWall.Add(_route.elementsWall[i]);
        }
    }
}
