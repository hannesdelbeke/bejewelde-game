using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour {

	public string[] text ;
	public GameObject[] images ; //support several images / emotions
	private int counter = 0 ; 

	void OnEnable()
	{
		Grid.OnScored += ScoreCellEvent;
	}

	void OnDisable()
	{
		Grid.OnScored -= ScoreCellEvent;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}



	void ScoreCellEvent (Cell.CellColors bob){
		ScoreCell (bob);
	}

	protected virtual void ScoreCell (Cell.CellColors bob) {

		Debug.Log ("bob");

	}

	/*
	void nextLine() {
		text[counter].;
		images [counter];

		counter++;
	}*/

	//get next label
	//swap between images
}
