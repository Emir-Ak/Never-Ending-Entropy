using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chaser"))
        {
            collision.GetComponent<Chaser>().toAppear = true;
        }
    }
}
