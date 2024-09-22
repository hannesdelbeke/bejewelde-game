using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Cell : MonoBehaviour {
	public enum CellColors {red, blue, green, yellow, purple,orange, grey };
	public CellColors CellColor = CellColors.red;
	public bool ForceRespawnColor = false;
	public CellColors CellColorAfterDeath = CellColors.red;


	public int row;
	public int col;

	public Material bluemat;
	public Material redmat;
	public Material greymat;
	public Material greenmat;
	public Material yellowmat;
	public Material purplemat;
	public Material orangemat;

	public string dead; //temp

	public float scale = 1f;

	public Grid currentGrid;
	public float fallspeed = 10;
	public Vector3 targetPos; 
	private float timerGravity = 0;	
	public Vector3 swapbackTargetPos; 	

	private float shrinkTimer = 0 ;
	private float shrinkTime = 0 ;
	private bool shrinking = false ;

	public bool scoring = false;
	public bool superCell = false ; 	
	[SerializeField]
	private bool busy = false ; 			//busy swap aniamtion or falling , is used by gridmanager to check if the cell is in use
	//public bool stayBusy = false ; 
	public bool isMoving = false ; 			
	private bool animating = true ; 	//this acts as a buffer incase of very small timedifference . not sure if needed	
	public bool swapback = false;
	public bool Busy				
	{
		get { 
			return busy || isMoving; 	}
		set	{ busy = value ; 
			//animating = value;
		}
	}
	public bool Moving				
	{
		get { return isMoving; 	}
		set	{ isMoving = value ; 
			animating = value;}
	}
	public float Shrink				
	{
		get { return shrinkTimer; 		}
		set	{	shrinkTimer = value ;
				shrinkTime 	= value ;
				shrinking 	= true	; 	}
	}


	// Use this for initialization
	void Start () {
		currentGrid = GameObject.Find ("GameManager").GetComponent<GameManager>().selectedLevel.GameRules.GetComponent<Grid>() ;
		ResetMat();
	}
	
	// Update is called once per frame
	void Update () {
		SetMat (); //debugging


	//	name = "Cell r" + row + " c" + col; 													//debuggin purpose name 
	//	transform.GetChild (0).GetComponent<TextMesh> ().text = ("r" + row + " c" + col + dead);	//debuggin purpose name 

		timerGravity += Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, targetPos, timerGravity * fallspeed);
		//transform.position = targetPos;
		if (Vector3.Distance (transform.position, targetPos) < 0.05 && isMoving && !swapback) {
			isMoving = false;
		}
		if (transform.position == targetPos && animating) {
			if (!swapback) {
				if (currentGrid) currentGrid.CheckMatchingCells (this); //if currentGrid is needed to make it update in editor mode //  if (Application.isPlaying)
				animating = false;
			} else {
				swapback = false;
				targetPos = swapbackTargetPos;
			}
		}
		if (transform.position == targetPos)
			timerGravity = 0.01f;


	//	if (superCell)
	//		transform.Rotate (new Vector3(0,0,Time.realtimeSinceStartup ));

		if (shrinking && shrinkTimer>0 )
		{
			shrinkTimer -= Time.deltaTime;
			transform.localScale = new Vector3 (scale,scale, scale) * shrinkTimer / (shrinkTime+0.1f);
		}
		else {
			shrinkTimer = 0 ; 
			shrinking 	= false ;
			transform.localScale = new Vector3 (scale, scale, scale);
		}
	} 

	public void Score() {
		//mark for delete
		ResetMat();
		Vector3 pos = transform.position;
		pos.y +=  11;
		pos.x = col - 4 ;
		transform.position = pos;
		transform.rotation = Quaternion.identity;
		busy = false;
		Moving = true;
		//dead = "dead" ;
		//Destroy(gameObject);
		superCell = false;
	}
	public void Activate(){ 							//activates the special power of this cell 
		if (superCell) {
			currentGrid.Bomb (row,col);
		}
	}

	void ResetMat(){
		if (ForceRespawnColor) {
			CellColor = CellColorAfterDeath;
		}
		else{
			CellColor = (CellColors)Random.Range(0, 5);//6
		}
		SetMat ();
	}
	void SetMat(){
		if(CellColor==CellColors.blue)
			GetComponent<Renderer>().material = bluemat;
		if(CellColor==CellColors.red)
			GetComponent<Renderer>().material = redmat;
		if(CellColor==CellColors.green)
			GetComponent<Renderer>().material = greenmat;
		if(CellColor==CellColors.yellow)
			GetComponent<Renderer>().material = yellowmat;
		if(CellColor==CellColors.purple)
			GetComponent<Renderer>().material = purplemat;
		if(CellColor==CellColors.orange)
			GetComponent<Renderer>().material = orangemat;
		if(CellColor==CellColors.grey)
			GetComponent<Renderer>().material = greymat;
	}
}
