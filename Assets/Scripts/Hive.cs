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
        if (collision.CompareTag("Bullet"))
        {
            Activate();
        }
    }

    public void Activate()
    {
        foreach(Chaser chaser in chasers)
            chaser.toAppear = true;
    }
}
