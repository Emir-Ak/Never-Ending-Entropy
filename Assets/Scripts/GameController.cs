using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public bool timeActive = false;
     bool spaceActive = false;
    FilterManager filterManger;


    [SerializeField]
    GameObject enemies;
    [SerializeField]
    GameObject level;

    Chaser[] chasersToEnable;
    Door[] doorsToEnable;

    [SerializeField]
    GameObject UIToEnable;

    [SerializeField]
    PlayerController player;

    public bool hasIntelligence = false;


    // Start is called before the first frame update
    void Start()
    {
        chasersToEnable = enemies.GetComponentsInChildren<Chaser>();
        doorsToEnable = level.GetComponentsInChildren<Door>();     
        filterManger = FindObjectOfType<FilterManager>();

        AudioListener.pause = true;
        AudioListener.volume = 0;
    }

    public void ActivateComponent(string moduleName, GameObject module)
    {
        if(!spaceActive && moduleName == "Space")
        {
            spaceActive = true;
            Destroy(module);

        }
        else if (spaceActive)
        {
            switch (moduleName)
            {
                case "Time":
                    timeActive = true;
                    filterManger.NormalFilter();
                    Invoke("EnableComponents", 1f);

                    break;
                case "Entropy":
                    player.hasEntropy = true;
                    break;
                case "Sound":
                    AudioListener.pause = false;
                    AudioListener.volume = 1;
                    break;
                case "Light":
                    player.hasLight = true;
                    break;
                case "UI":
                    UIToEnable.SetActive(true);
                    break;
                case "Weapon":
                    player.hasWeapon = true;
                    break;
                case "Intelligence":
                    hasIntelligence = true;
                    break;
                case "Wisdom":
                    break;
                case "Accelerator":
                    break;
            }

            Destroy(module);
        }
    }

    void EnableComponents ()
    {
        foreach (Chaser chaser in chasersToEnable)
        {
            if (chaser.enabled == false)
            {
                chaser.enabled = true;
                chaser.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
        foreach (Door door in doorsToEnable)
        {
            if (door.enabled == false)
            {
                door.enabled = true;
            }
        }
    }
}
