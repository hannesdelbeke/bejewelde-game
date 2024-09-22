using UnityEngine;
using System.Collections;

public class lvl01 : Story {


	//fade and text
	//user swaps and scores

	// Use this for initialization
	void Start () {

		images[0].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected override  void ScoreCell (Cell.CellColors bob) {

		Debug.Log ("bob2");
		images[0].SetActive(false);
	}
}
