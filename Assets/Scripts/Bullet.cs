using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    Component[] toDisable;
    [SerializeField]
    GameObject explosion;

    public float damage = 10f;
    public float destroyDelay;
    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Contains("Player"))
        {
            if (collision.GetComponent<Collider2D>().isTrigger == false)
            {
                Instantiate(explosion, transform);

                if (collision.CompareTag("Chaser"))
                {
                    Chaser chaser = collision.GetComponent<Chaser>();
                        if (chaser.toAppear == false) 
                        chaser.toAppear = true;

                    chaser.GetDamage(damage, transform.position);
                }

                foreach (Component obj in toDisable)
                {
                    if (obj is Collider2D)
                        (obj as Collider2D).enabled = false;
                    if (obj is SpriteRenderer)
                        (obj as SpriteRenderer).enabled = false;
                    if (obj is Rigidbody2D)
                        (obj as Rigidbody2D).velocity = Vector2.zero;
                }
            }
        }
    }
}
