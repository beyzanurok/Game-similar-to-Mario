using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public PlayerController owner;

    void Start ()
    {
        Destroy(gameObject, 5.0f);
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage, owner);
        }

        Destroy(gameObject);
    }
}