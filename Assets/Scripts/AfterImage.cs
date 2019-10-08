using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{

    [Header("References")]

    [Tooltip("Where will instances spawn")]
    public Transform parent;

    [Tooltip("Object with sprite renderer that will be used in tail")]
    public GameObject image;

    [Tooltip("Rigidbody to obtain velocity and positon from")]
    public Rigidbody2D rbRef;

    [Tooltip("Time between creation of each image")]
    public float timeBetweenImages = 0.05f;

    [Tooltip("How fast will image fade")]
    public float alphaDecrease = 0.02f;

    [Space(5)]
    [Header("Offsets from rigidbody position of where the image is created")]
    public float xOffset = 0f;
    public float yOffset = 0f;

    [Space(5)]
    [Header("Wether afterimage is active")]
    public bool activate = false;

    //Stores each instantiated image
    List<GameObject> instances = new List<GameObject>();

    //Counter to hault the spawn for Time Between Images
    private float timeCounter;
    
    //Position where the image will spawn
    Vector3 imgPos;

    //ID of the instance of an image, used in naming
    int img_ID = 0; 

    private void Awake()
    {
        //Assign parent to self if no parent is assigned
        if (parent == null)
        {
            parent = gameObject.transform;
        }

        //Assign value of timeCounter
        timeCounter = timeBetweenImages;
    }
    private void FixedUpdate()
    {
        //If after image effect is activated
        if (activate == true)
        {
            //Instantiate instances of the image when respective rigidbody is moving
            Vector2 velocity = rbRef.velocity;
            if (velocity != Vector2.zero)
            {
                InstantiateImage();
            }
        }

        //If there are instances of image, fade them
        if (instances != null)
        {
            InitializeFade();
        }
    }
    void InstantiateImage()
    {
        //If counter reached timeBetweenImages
        if (timeCounter >= timeBetweenImages)
        {
            //Assign position of where image should be instantiated with offsets
            imgPos = new Vector2(rbRef.position.x + xOffset, rbRef.position.y + yOffset);

            //Instantiate new instance of image at imgPos with same rotation under parent object and store it as new GameObject
            GameObject instance = Instantiate(image, imgPos, Quaternion.identity, parent);

            //If the instance is stored in scene and is deactivated, reactivate it
            if (instance.activeSelf == false)
            {
                instance.SetActive(true);
            }

            //ID of the image is assigned as it's name
            instance.name = img_ID.ToString();

            //Add the instance to the list of instances
            instances.Add(instance);

            //Reset the counter to 0 
            timeCounter = 0;

            //Increase the ID by 1 
            img_ID++;
        }
        //Increase the counter otherwise
        else
            timeCounter += Time.fixedDeltaTime;
    }

    void InitializeFade()
    {
        //Start FadeSprite coroutine for each of the image instances
        for (int i = 0; i < instances.Count; i++)
        {
            SpriteRenderer instanceSprite = instances[i].GetComponent<SpriteRenderer>();
            StartCoroutine(FadeSprite(instanceSprite, instances[i], i));
        } 
    }

    IEnumerator FadeSprite(SpriteRenderer sprite, GameObject destroyObject, int index)
    {
        //While image is not fully faded
        while (sprite.color.a >= 0)
        {
            //Calculate new alpha
            float newAlpha = sprite.color.a - alphaDecrease;

            //Set it to the sprites color
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, newAlpha);

            //Wait until next fixed frame
            yield return new WaitForFixedUpdate();
        }
        
        //Destroy image 0.1 seconds after it faded away
        Destroy(destroyObject, 0.1f);

        //Remove instance from list of instances
        instances.Remove(destroyObject);
    }
}
