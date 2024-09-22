using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public Level selectedLevel;
	public string lvlTitle;
	public string lvlDescription;
//	public GameObject[] levels ;
	public GameManager gameManager ;
	public CameraManager cameraManager ;
	public UIManager uiManager ;
	public enum gameMode {World, Grid };

	public GameObject Levels;

	gameMode currentGameMode ;

	// Update is called once per frame
	void Update () {		
		if (currentGameMode == gameMode.World) WorldUpdate ();
		else
		if (currentGameMode == gameMode.Grid) GameUpdate ();
	}
	void GameUpdate(){

	}

	void WorldUpdate(){
		if (Input.GetMouseButtonDown (0)) {			
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			if (hit.collider != null ){
				Level _lvl = hit.collider.GetComponent<Level> ();		
				if (_lvl != null) {					
					SelectLevel(_lvl);
					//play when play button clicked (SELECTEDlEVEL)
					//selectedLevel = hit.collider.gameObject.GetComponent<Level>();
					Debug.Log ("Target Position: " + hit.collider.gameObject.transform.name);

					PlayLevel ();
					currentGameMode = gameMode.Grid;
					Levels.SetActive (false);
				}
			}
		}
	}

	public void PlayLevel (){
		uiManager.enterGame ();
		cameraManager.enabled = false;
		GameObject _gameRules = selectedLevel.GameRules;
		_gameRules.SetActive (true);
		//start selectedLevel
		//set win conditions
	}
	void UnlockLevel (Level _lvl){
		_lvl.unlocked = true ;
	}

	void SelectLevel (Level _lvl){
		selectedLevel = _lvl ; 	
		gameManager.selectedLevel = _lvl; 
		lvlTitle = selectedLevel.title ;
		lvlDescription = selectedLevel.description;
		//load play button etc
	}
}
