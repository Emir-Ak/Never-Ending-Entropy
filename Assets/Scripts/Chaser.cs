using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    [SerializeField]
    Transform target;
   public float moveSpeed = 1;
    public bool toAppear = false;
    bool toFollow = false;
    Animator animator;
    bool appearAnimActive = false;
    public float health = 100f;
    public float followRange = 10f;

    [SerializeField]
    GameObject blood;
    [SerializeField]
    GameObject mainBody;

    Vector2 moveDir;
    bool hasAttacked = false;
    Rigidbody2D rb;
    private void Start()
    {
        animator = GetComponent<Animator>();
        float rndSpeed = Random.Range(2.5f * moveSpeed, 3.5f * moveSpeed);
        moveSpeed = rndSpeed;
        rb = GetComponent<Rigidbody2D>();
    }
      

    private void Update()
    {
        if (!hasAttacked && moveDir != (Vector2)transform.position)
        {
            moveDir = target.position;
        }
        RotateTowards(target.position);

        if (toAppear)
        {
            animator.SetBool("toAppear", true);
        }
       

        if (Vector2.Distance(target.position, transform.position) > followRange && toFollow == true)
        {
            toFollow = false;
            animator.StartPlayback();
        }
         if (Vector2.Distance(target.position, transform.position) <= followRange && toFollow == false && animator.GetBool("toFollow") == true)
        {
            toFollow = true;
            animator.StopPlayback();
        }
         if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (toFollow)
        {
            Move();
        }
    }
    private void Move()
    {
        if(!hasAttacked)
        rb.position = Vector2.MoveTowards(transform.position, moveDir, moveSpeed * Time.fixedDeltaTime);
        else
            rb.velocity = moveDir * moveSpeed * 25f * Time.fixedDeltaTime;

    }

    private void RotateTowards(Vector2 target)
    {
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle));
    }

    void ToFollow()
    {
        toFollow = true;
        animator.SetBool("toFollow", true);
    }

    public void GetDamage(float damage, Vector3 pos)
    { 
        health -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasAttacked)
        {
            PlayerController player = target.GetComponent<PlayerController>();
            player.ReceiveDamage(15f);
            Invoke("ResetAttack", 0.25f);
            hasAttacked = true;
            moveDir = (target.transform.position - transform.position)* -1;
                moveDir.Normalize();
            moveSpeed *= 3;
            animator.SetBool("toJump", true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;

    }

    void ResetAttack()
    {
        moveSpeed /= 3;
        animator.SetBool("toJump", false);
        hasAttacked = false;
        rb.velocity = Vector2.zero;
    }
}


