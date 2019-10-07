using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedUpItemReaction : MonoBehaviour
{
    public Item item;
    private Inventory inventory;
    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    private void ImmediateReaction()
    {
        inventory.AddItem(item);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (Item item in inventory.items)
            {
                if (item == null || this.item.assignedID == item.assignedID)
                {
                    Destroy(gameObject);
                    ImmediateReaction();
                    return;
                }
            }

        }

    }



}
