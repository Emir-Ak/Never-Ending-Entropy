using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public float delay = 5;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, delay);
    }

}
