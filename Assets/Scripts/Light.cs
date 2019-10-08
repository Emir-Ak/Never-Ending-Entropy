using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameController.timeActive)
        {
            float distance = Vector3.Distance(transform.position, collision.transform.position);
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();
            Debug.DrawRay(transform.position, direction * distance, Color.red);
            RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, distance, 1 << 9);
            if (!ray)
            {
                if (collision.CompareTag("Chaser"))
                {
                    Debug.Log("Chaser activated");
                    collision.GetComponent<Chaser>().toAppear = true;
                }
            }
        }
    }
}
