using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float lifeTime;
    List<Chaser> chasers = new List<Chaser>();
    List<float> moveSpeeds = new List<float>();
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("SetEffect", lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if (collision.CompareTag("Chaser"))
        {
            rb.velocity = Vector2.zero;
         
            Chaser chaser = collision.GetComponent<Chaser>();
             if (chaser.toAppear == true)
            {
                if (chasers.Count != 0)
                {
                    if (!chasers.Contains(chaser))
                        ReduceSpeed(chaser);

                }
                else
                {
                    ReduceSpeed(chaser);
                }
            }
        }
    }

    private void SetEffect()
    {
        for (int i = 0; i < chasers.Count; i++)
        {
            chasers[i].moveSpeed = moveSpeeds[i];
        }
        Destroy(gameObject);
    }

    void ReduceSpeed(Chaser chaser)
    {
        chasers.Add(chaser);
        moveSpeeds.Add(chaser.moveSpeed);
        chaser.moveSpeed /= 3.5f;

    }
}
