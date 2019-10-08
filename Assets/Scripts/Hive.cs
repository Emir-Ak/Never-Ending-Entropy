using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    Chaser[] chasers;
    GameController gameController;
    private void Start()
    {
        chasers = GetComponentsInChildren<Chaser>();
        gameController = FindObjectOfType<GameController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameController.timeActive)
        {
            if (collision.CompareTag("PlayerLight") || collision.CompareTag("Bullet"))
            {
                foreach (Chaser chaser in chasers)
                {
                    chaser.toAppear = true;
                }
            }
        }
    }
}
