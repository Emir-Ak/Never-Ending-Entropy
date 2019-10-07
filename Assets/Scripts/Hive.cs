using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    Chaser[] chasers;
    private void Start()
    {
        chasers = GetComponentsInChildren<Chaser>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerLight") || collision.CompareTag("Bullet"))
        {
            foreach (Chaser chaser in chasers)
            {
                chaser.toAppear = true;
            }
        }
    }
}
