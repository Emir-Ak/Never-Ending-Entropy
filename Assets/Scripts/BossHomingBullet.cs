using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class BossHomingBullet : MonoBehaviour
{

    bool isHoming = false;
    public float bulletSpeed;
    Vector2 moveDir;

    public int damage;
    public Transform target;

    Rigidbody2D rb;
    private void Start()
    {
        Invoke("ResetVelocity",1);
    }

    void ResetVelocity()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        isHoming = true;
    }
    private void Update()
    {
        if (isHoming = true)
        {
            moveDir = target.position;
            rb.position = Vector2.MoveTowards(transform.position, moveDir, bulletSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().ReceiveDamage(damage);
        }   
    }
}
