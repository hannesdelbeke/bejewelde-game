using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour {


	public bool useShader = false;
	public Renderer rend;


	private Vector3 startScale;
	private float startWidth;

	private float currentValueAnimated = 1;

	public float maxValue = 1;
	private float currentValue = 0;
	private float previousValue = 0;
	public float CurrentValue				
	{
		get { 
			return currentValue; 	}
		set	{ 
			if (value > maxValue)
				currentValue = maxValue;
			else
				currentValue = value ; 
			//animating = value;
		}
	}

	void Start () {
		startScale = transform.localScale;
		startWidth = startScale.x ;
		rend = GetComponent<Renderer>();
	}
		
	void Update () {
		float percent = currentValue / maxValue;

		if (currentValueAnimated > percent) {
			currentValueAnimated -= Time.deltaTime * 0.5f;
			if (currentValueAnimated < 0)
				currentValueAnimated = 0;
		}
		if (currentValueAnimated < percent){
			currentValueAnimated += Time.deltaTime * 0.5f; 
			if (currentValueAnimated > percent)
				currentValueAnimated = percent;
		}

	/*	if (previousValue < currentValue)
			previousValue += Time.deltaTime * 0.5f;
		if (previousValue > currentValue)
			previousValue = currentValue;*/

		if (useShader == false) {
			Vector3 _scale = transform.localScale;
			_scale.x = startWidth / maxValue * currentValue;
			transform.localScale = _scale;
		} else {
			if (currentValueAnimated > percent) {
				rend.material.SetFloat ("_Percent", percent); 
				rend.material.SetFloat ("_Percent2", currentValueAnimated);
			}
			if (currentValueAnimated <= percent) {
				rend.material.SetFloat ("_Percent", currentValueAnimated); 
				rend.material.SetFloat ("_Percent2", percent);
			}


		}
	}
}
