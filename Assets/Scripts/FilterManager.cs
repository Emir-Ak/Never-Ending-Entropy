using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using System.Collections;

//Changes post processing profiles in the volume of current main camera
public class FilterManager : MonoBehaviour
{
    [Header("References")]

    [Tooltip("Profile when game is in normal state")]
    public PostProcessProfile normalProfile;

    [Tooltip("Profile when game is in state of matrix")]
    public PostProcessProfile matrixProfile;

    [Space(5)]
    [Header("How fast will profile change to the other (framed)")]
    public float filterChangeRate = 0.02f;

    [HideInInspector]
    public bool transitionFinished; //Wether transition to the profile is fully complet ed

    PostProcessVolume volume;

    //public static FilterManager instance; //Used for singletone


    private void Awake()
    {
        ////Singleton
        //if (instance == null)
        //    instance = this;
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        ////Manager is not destroyed throughout other scenes
        //DontDestroyOnLoad(gameObject);

        //Reference the volume on main camera
        volume = Camera.main.GetComponent<PostProcessVolume>();
    }
    
    //Apply matrix filter
    public void NormalFilter()
    {
        transitionFinished = false;
        StartCoroutine(DecreaseWeight(matrixProfile));
        Debug.Log("Filter is changing to Spacetime_Mastermind");
    }

    IEnumerator DecreaseWeight(PostProcessProfile newProfile)
    {
        //While weight is not 0
        while (volume.weight > 0)
        {
            //Make sure it does not fall under 0
            if(volume.weight < 0)
                volume.weight = 0;

            //Subtract the filterChangeRate from weight of the volume
            volume.weight -= filterChangeRate;

            //Wait for next fixed frame
            yield return new WaitForFixedUpdate();
        }
        
        //Assign the volume to new profile after weight is for sure 0
        volume.profile = newProfile;

        //Start increasing the weight
        StartCoroutine(IncreaseWeight());
    }

    IEnumerator IncreaseWeight()
    {
        //While weight is lower than 1
        while (volume.weight < 1)
        {
            //Make sure it does not exceed 1
            if (volume.weight > 1)
                volume.weight = 1;

            //Increase the weight by filterChangeRate
            volume.weight += filterChangeRate;

            //Wait for next frame
            yield return new WaitForFixedUpdate();
        }

        transitionFinished = true;
    }
}
