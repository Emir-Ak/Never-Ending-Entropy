using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool playerNearby = false;
    public float movingSpeed = 5f;
    public float positioning = 1; //1 top, 2, bottom, 3 left, 4 right 

    Vector3 destination;
    Vector3 initialPos;

    [SerializeField]
    GameObject door;
    public bool blue;
    bool isOpen = false;
    private void Start()
    {
        destination = transform.position;
        initialPos = transform.position;
        switch (positioning)
        {
            case 1:
                destination.x += 3;
                break;
            case 2:
                destination.x -= 3;
                break;
            case 3:
                destination.y += 3;
                break;
            case 4:
                destination.y -= 3;
                break;
        }

    }
    private void Update()
    {

        if (playerNearby)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, destination, Time.deltaTime * movingSpeed);
        }
        if (!playerNearby)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, initialPos, Time.deltaTime * movingSpeed);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            isOpen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

}