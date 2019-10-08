using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

	private float fillAmount;
	[SerializeField]
	private float lerpSpeed;

	[SerializeField]
	private Image content;

	public float MaxValue { get; set;}

	public float Value {
		set
		{
			fillAmount = Map (value, 0, MaxValue, 0, 1);
		}
	}

	// Update is called once per frame
	void Update () {
		HandleBar ();
	}

	private void HandleBar () {
        if (fillAmount != content.fillAmount)
		{

            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
		}

	}

	private float Map(float value,float inMin,float inMax,float outMin,float outMax) {
		return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		//calculates the float for fill amount based on the value of heath etc thats being iptuted
	}
}
