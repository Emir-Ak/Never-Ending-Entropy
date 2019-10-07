using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static GameObject gameCanvas;
     
    public const int numItemSlots = 4;

    [SerializeField]
    GameObject whiteBorder;

    #region Arrays_For_items
    public Image[] itemImages = new Image [numItemSlots];
    public Item[] items = new Item[numItemSlots];
    public GameObject[] itemSlots = new GameObject[numItemSlots];

    #endregion

    public float slotScaleFactor = 1.1f;
    //private static bool created = false;
    private int selectionIndex = 0;

    private InteractionController interactionController;

    public static bool isFull = false;

    void Awake()
    {
        if (gameCanvas == null)
        {
            DontDestroyOnLoad(gameObject);
            gameCanvas = this.gameObject;
        }
        else if(gameCanvas != this.gameObject)
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        interactionController = FindObjectOfType<InteractionController>();
        whiteBorder.SetActive(true);
    }

    private void Update()
    {
        GetSelectionIndex();
        Interact();
    }

    public bool Contains(Item itemToCheck, int itemAmount)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].assignedID == itemToCheck.assignedID)
            {
                return true;
            }
        }
        
        return false;
    }

    public void AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
             if (items[i] == null)
            {
                items[i] = itemToAdd;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                return;
            }
        }
    }

    public void RemoveItem()
    {
        int i = selectionIndex;
        items[i] = null;
        itemImages[i].sprite = null;
        itemImages[i].enabled = false;
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToRemove)
            {
                    items[i] = null;
                    itemImages[i].sprite = null;
                    itemImages[i].enabled = false;
            }
        }
    }



    private void GetSelectionIndex()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if (selectionIndex == 3)
            {
                selectionIndex = 0;
            }
            else
            {
                selectionIndex++;
            }

            SelectItemSlot(); 
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (selectionIndex == 0)
            {
                selectionIndex = 3;
            }
            else
            {
                selectionIndex--;
            }
            
            SelectItemSlot();
        }
    }

    
    private void SelectItemSlot()
    {
        whiteBorder.transform.position = itemSlots[selectionIndex].transform.position;
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (items[selectionIndex] != null)
            {
                int i = selectionIndex;
                interactionController.ApplyInteraction(items[i]);
            }
        }
    }
}
