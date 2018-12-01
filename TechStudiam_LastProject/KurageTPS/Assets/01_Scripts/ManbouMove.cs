using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManbouMove : MonoBehaviour
{
    public float hp = 100f;

    private Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float _damage)
    {
        hp -= _damage;

        if (hp <= 0)
        {
            anim.Play("Dead");
            return;
        }

        anim.Play("Hit");
    }
}
