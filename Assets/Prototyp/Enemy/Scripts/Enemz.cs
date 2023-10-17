using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemz : MonoBehaviour
{
    [SerializeField] private float health = 3;

    public GameObject player;
    public Animator animator;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmout)
    {
        health -= damageAmout;
        animator.SetTrigger("damage");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
