using UnityEngine;
using System.Collections;
using System.Collections.Generic; //needed for list

public class Grid : MonoBehaviour {

	public delegate void ScoreAction (Cell.CellColors cellType) ;
	public static event ScoreAction OnScored;

	public GameObject cellPrefab ;
	public UIManager  UIMngr ;
	public GameObject storyText ;

	//grid array
	private int rows 	= 7 ;
	private int columns = 7 ;
	// Use this for initialization
	private Cell[,] cellgrid;

	Cell clickCell; 						//the cell we clicked
	private Vector3 mousePosClick 		;		
	private Vector3 mousePos 			;				
	private int inputDistance = 25		;	// distance to swipe in a direction
	private bool draggingGem = false	;		
	private bool validTurnAction = false;	

	private List<Cell> matching_cells_vert 	= new List<Cell>(); 
	private List<Cell> matching_cells_hor 	= new List<Cell>(); 
	//public List<Cell> scoring_cells = new List<Cell>(); 


	private int score = 0;
	private int scoreWin = 999999;
	private bool won = false;

	public bool loadLvl = false;

	public float exp = 0;
	public float mana = 0;
	public float health = 100;
	private float healthEnemy = 100;
	public float healthEnemyMax = 100;
	public float gold = 0;

	void Start () {	
		healthEnemy = healthEnemyMax;

		if (loadLvl)
			LoadGrid ();
		else
			CreateGrid ();
	}

	// Update is called once per frame
	void Update () {
	
		//handle movement
		//do match check
		//make the others fall if needed

		if(Input.GetMouseButtonDown(0) ) // if user clicked
		{
			Vector3 worldMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector2 rayOrigin = new Vector2 (worldMouse.x, worldMouse.y);

			RaycastHit2D hit = Physics2D.Raycast( rayOrigin ,  Vector2.zero, 0);
			if (hit && hit.transform.GetComponent<Cell>())
			{
				mousePosClick = Input.mousePosition;
				draggingGem = true;
				//Debug.Log(hit.transform.GetComponent<Cell>().CellColor);
				//select gem
				clickCell = hit.transform.GetComponent<Cell>() ;

				//lets test out the score function first
				//Debug.Log("matches"+CheckMatchingCells (clickCell, false));
				//BombRow(clickCell.row,clickCell.col) ;
			}
		}

	//	Debug.Log (cellgrid [3, 5].name);

		if (draggingGem) {
			
			mousePos = Input.mousePosition;
			Vector3 mouseDelta = mousePosClick - mousePos;

			//gem follow mouse input position
			//clickCell.targetPos = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
			//clickCell.targetPos.z = 0;

			//	Debug.Log (mouseDelta); 	// (0,-inputDistance) up     (0,inputDistance) down    (-inputDistance,0) right      (inputDistance,0) left
			//up is r+1 , down is r-1 , right is c+1 , left is c-1 
			//	int delta_x = mouseDelta.x ; //
			//	int delta_y = mouseDelta.y ;

			//Debug.Log(mouseDelta);
			if (mouseDelta.x > inputDistance) { //left
				//Debug.Log("Left");
				TrySwapLeft (clickCell);
			}
			else
			if(mouseDelta.x < -inputDistance  ) { //right
				//Debug.Log("right");
				TrySwapRight(clickCell);
			}
			else
			if (mouseDelta.y > inputDistance) { //down
				//Debug.Log("down");
				TrySwapDown (clickCell);
			}
			else
			if(mouseDelta.y < -inputDistance ) { //up
				//Debug.Log("up");
				TrySwapUp (clickCell);
			}
		}


		if(Input.GetMouseButtonUp(0) ) 	// if user released click
		{
			draggingGem = false;
			clickCell 	= null; // reset gem when click released
		}
		checkTurnEnd ();
		CheckWin();
		CheckLose();
		UpdateStats (); // updates the ui graphics

	} // end update


	protected virtual void CheckWin(){

		if (healthEnemy <= 0) {
			UIMngr.winRound = true;
		}

		if (score > scoreWin)
			won = true;
	}
	protected virtual void CheckLose(){

		if (health <= 0) {
			UIMngr.loseRound = true;
		}
	}
//	protected virtual void checkScore(){}


	protected virtual void LoadGrid (){

		Debug.Log("test laoding");
		cellgrid 		= new Cell[rows,columns];

		foreach (Transform _cellTransform in transform) {
			Cell _cell = _cellTransform.GetComponent<Cell> ();
			int _r =_cell.row;
			int _c =_cell.col;
			cellgrid [_r, _c] = _cell;
		}
	//	Vector3 cellPos = new Vector3 (0,0,0);

		float row_f = rows;
		float col_f = columns;

		/*
		for (int i = 0; i < rows; i++) {
			//cellPos.x = i - row_f/2 + 0.5f ;

			for (int j = 0; j < columns; j++) {
			//	cellPos.y = j - col_f/2 ;
			//	GameObject Tempcell = Instantiate (cellPrefab, cellPos, Quaternion.identity) as GameObject;
				//set row and col
				Tempcell.GetComponent<Cell>().row = j;
				Tempcell.GetComponent<Cell>().col = i; 
				Tempcell.GetComponent<Cell>().targetPos = cellPos; 
				cellgrid[j,i] = Tempcell.GetComponent<Cell>();
				Tempcell.transform.parent = gameObject.transform;
			}
		}*/

	}

	/// <summary>
	/// Create new cells and store them in cellgrid[row,col]
	/// </summary>
	protected virtual void CreateGrid (){
		//spawn new cells at the positions
		//create cells to fill row and column

		cellgrid 		= new Cell[rows,columns];

		Vector3 cellPos = new Vector3 (0,0,0);

		float row_f = rows;
		float col_f = columns;
		for (int i = 0; i < rows; i++) {
			cellPos.x = i - row_f/2 + 0.5f ;

			for (int j = 0; j < columns; j++) {
				cellPos.y = j - col_f/2 ;
				GameObject Tempcell = Instantiate (cellPrefab, cellPos, Quaternion.identity) as GameObject;
				//set row and col
				Tempcell.GetComponent<Cell>().row = j;
				Tempcell.GetComponent<Cell>().col = i; 
				Tempcell.GetComponent<Cell>().targetPos = cellPos; 
				cellgrid[j,i] = Tempcell.GetComponent<Cell>();
				Tempcell.transform.parent = gameObject.transform;
			}
		}
	}




	bool TrySwapDown(Cell cell1){

		int r = cell1.row - 1;
		int c = cell1.col;

		if(r>=0 && c>=0 && r<rows && c<columns)
			return TrySwap (cell1, cellgrid[r,c] );
		return false;
	}
	bool TrySwapUp(Cell cell1){

		int r = cell1.row + 1;
		int c = cell1.col;

		if(r>=0 && c>=0 && r<rows && c<columns)
			return TrySwap (cell1, cellgrid[r,c] );
		return false;
	}

	bool TrySwapLeft(Cell cell1){

		int r = cell1.row ;
		int c = cell1.col - 1 ;

		if(r>=0 && c>=0 && r<rows && c<columns)
			return TrySwap (cell1, cellgrid[r,c] );
		return false;
	}
	bool TrySwapRight(Cell cell1){

		int r = cell1.row ;
		int c = cell1.col + 1 ;

		if(r>=0 && c>=0 && r<rows && c<columns)
			return TrySwap (cell1, cellgrid[r,c] );
		return false;
	}






	bool TrySwap(Cell cell1,Cell cell2){
		draggingGem = false; 

		//check if cells arent busy
		if (!cell1.Busy && !cell2.Busy) {
			//swap them

			swapCells(cell1,cell2) ;

			//wait till they arrive - done in cell itself

			//score
			if (!CheckMatchingCells (cell1, false) && !CheckMatchingCells (cell2, false)) {	//check if they score 

				swapCellsBack (cell1, cell2);									//if not swap them back
			} else {
				validTurnAction = true;
				return true;
			}

		}
		//swap animatie
		//nadat hij arrived check je of ze kunnen swappen
		//als 1 vd opties kan swappen swap je ze
		//check cell 1
		//check cell 2
		return false ;
	}





	List<Cell> CheckScoreCells() //List<Cell> scoring_cells
	{
		List<Cell> scoring_cells = new List<Cell>(); 
		if (matching_cells_vert.Count > 2) {
			foreach ( Cell c in matching_cells_vert) {
				scoring_cells.Add (c);
			}
		}
		if (matching_cells_hor.Count > 2) {
			foreach ( Cell c in matching_cells_hor) {
				scoring_cells.Add (c);
			}
		}
		if(scoring_cells.Count>3){
			Debug.Log ("create supercell");
			//scoring_cells[0]
			Cell _cellOne = scoring_cells[0];
			scoring_cells.Remove (_cellOne);
			scoring_cells.Remove (_cellOne); // execute twice because there is a duplicate
			_cellOne.superCell = true ;
		}
		StartCoroutine(ScoreCells (scoring_cells) ) ; // if you have a combo dont scorecells but replace 1 with supercell
		return scoring_cells;
	}

	void ScoreCell(Cell scoring_cell ){	// only activates on force score cell ? do not confuse with scorecells which scores when swapping


		List<Cell> scoring_cells = new List<Cell>(); 
		scoring_cells.Add (scoring_cell);
		StartCoroutine(ScoreCells (scoring_cells) ) ;
	}
	IEnumerator ScoreCells(List<Cell> scoring_cells ){
		foreach ( Cell c in scoring_cells) {
			c.scoring = true;
			c.Busy = true;
			c.Shrink = 0.3f;
			c.Activate (); //activate special effects
		}
		yield return new WaitForSeconds(0.3f);

		foreach ( Cell c in scoring_cells) {
			Debug.Log ("Scorecell"); 
			//event : scored cell of type x
			if (OnScored != null)
			{	OnScored (c.CellColor);
			}

			int col = c.col;
			int row = c.row;
			//c.Busy = false;
			//move the cells above it . make them fall down
			for (int i =0 ; (i+row+1) < columns; i++) { 
				swapCells(cellgrid [row + i , col], cellgrid [row+i+1, col]);
			}

			switch (c.CellColor) {
			case Cell.CellColors.blue:
				mana++;
				break;
			case Cell.CellColors.green:
				exp++;
				break;
			case Cell.CellColors.red:
				health++;
				break;
			case Cell.CellColors.yellow:
				gold += 5 ;
				break;
			case Cell.CellColors.purple:
				healthEnemy -= 10 ;
				break;

			}
			c.Score();
			//if( (row+1) < columns)
			//swapCells(cellgrid [row  , col], cellgrid [row+1, col]);


			score += 100;
			c.scoring = false;
		}
		scoring_cells.Clear ();

	}

	void UpdateStats (){
		UIMngr.Exp = exp;
		UIMngr.Mana = mana;
		UIMngr.Health = health;
		UIMngr.Gold = gold;
		UIMngr.HealthEnemy = healthEnemy;

	} 




	// semi cleaned up functions
	//-------------------------------------------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------------------------------------------





	/// <summary> swap the position of two cells and set both to busy </summary>
	void swapCells (Cell cell1,Cell cell2) {
		//set busy


		//swap pos
		Vector3 tempPosSwap 	= cell1.targetPos;
		cell1.targetPos = cell2.targetPos ;
		cell2.targetPos = tempPosSwap ;

		swapCellsGrid (cell1, cell2);
	}
	void swapCellsBack (Cell cell1,Cell cell2)
	{
		cell1.swapback = true;
		cell2.swapback = true; 

		Vector3 p1 = cell1.targetPos;
		Vector3 p2 = cell2.targetPos;
		cell1.swapbackTargetPos = p2 ;
		cell2.swapbackTargetPos = p1 ;

		swapCellsGrid (cell1, cell2);
	}

	void swapCellsGrid (Cell cell1,Cell cell2){

		//swap in cellgrid list
		Cell tempCell = cellgrid [cell1.row, cell1.col];
		cellgrid [cell1.row, cell1.col] = cellgrid [cell2.row, cell2.col] ;
		cellgrid [cell2.row, cell2.col] = tempCell ;

		//swap individual row
		int tempInt = cell1.row;
		cell1.row = cell2.row;
		cell2.row = tempInt;

		//swap individual col
		tempInt = cell1.col;
		cell1.col = cell2.col;
		cell2.col = tempInt;

		cell1.Moving = true;
		cell2.Moving = true;
	}

	public bool CheckMatchingCells (Cell cell )//, out List<Vector2> scoringCells) //, out List<Cell>
	{
		return CheckMatchingCells (cell, true);
	}
	/// <summary>Checks the matching cells. Add matches to list</summary>
	public bool CheckMatchingCells (Cell cell, bool score )//, out List<Vector2> scoringCells) //, out List<Cell>
	{
		matching_cells_vert.Clear();
		matching_cells_hor.Clear();
		matching_cells_vert.Add(cell);
		matching_cells_hor.Add(cell);

		int[] matchingCells = CheckMatch (cell.row, cell.col, cell.CellColor);
		//check if it scores
		if(matchingCells[0]>2 || matchingCells[1]>2 ) 
		{
			Debug.Log ("swap succes - score");
			if (score) {
				List<Cell> scoring_cells = new List<Cell> ();
				scoring_cells = CheckScoreCells ();
				Debug.Log (scoring_cells.Count);
				int chainLength = scoring_cells.Count ;
				if (chainLength > 3)
					Debug.Log ("CHAIN" + scoring_cells[0].name); 
				//create supercell at 0 index pos

			}
			//ScoreCells ( scoring_cells);	
			return true;
		}
		//	scoringCells = new List<Vector2>();
		return false;
	}

	int[] CheckMatch(int row, int column, Cell.CellColors cellcolor)
	{ 
		int hor_match 	= 1;
		int vert_match 	= 1;

		hor_match += CheckRow (row,column,true,cellcolor);	//check left
		hor_match += CheckRow (row,column,false,cellcolor);	//check right
		vert_match += CheckCol (row,column,true,cellcolor);	//check left
		vert_match += CheckCol (row,column,false,cellcolor);	//check right

		//Vector2 match = new Vector2 (hor_match, vert_match);
		int[] match = new int[2];
		match[0] = hor_match ;
		match[1] = vert_match ;

		return match;
	}

	int CheckRow(int r, int c,bool checkLeft, Cell.CellColors cellcolor){
		//Debug.Log("r"+r+"c"+c);
		int match = 0;
		if(checkLeft)
			c -= 1;
		if(checkLeft==false)
			c += 1;
		
		if (c >= 0 && c <columns ) {
			if (cellcolor == cellgrid [r, c].CellColor && 
				cellgrid [r, c].Busy ==false ) {
				//Debug.Log (cellgrid [r, c].name + " " + cellgrid [r, c].CellColor);
				matching_cells_hor.Add (cellgrid [r, c] );
				match += 1;
				match += CheckRow (r, c, checkLeft,cellcolor);
			}
		}
		return match;
	}

	int CheckCol(int r, int c,bool checkUp, Cell.CellColors cellcolor){
		//Debug.Log("r"+r+"c"+c);
		int match = 0;
		if(checkUp)
			r += 1;
		if(checkUp==false)
			r -= 1;

		if (r >= 0 && r < rows ) {
			if (cellcolor == cellgrid [r, c].CellColor && 
				cellgrid [r, c].Busy==false ) {
				//Debug.Log (cellgrid [r, c].name + " " + cellgrid [r, c].CellColor );
				matching_cells_vert.Add (cellgrid [r, c]);
				match += 1;
				match += CheckCol (r, c, checkUp,cellcolor);
			}
		}
		return match;
	}

	void checkTurnEnd () {
		//activate enemy turn stuff
		if (validTurnAction) {
			Debug.Log (health);
			health -= 5;
		}
		//activate passive effects on end of turn etc

		validTurnAction = false;
	}

	public void Bomb (int row, int column){
		int r = row;
		int c = column;
		r = row-1;
		forceScoreCell (r,c);
		c = column - 1;
		forceScoreCell (r,c);
		c = column + 1;
		forceScoreCell (r,c);
		r = row;
		forceScoreCell (r,c);
		c = column - 1;
		forceScoreCell (r,c);
		r = row+1;
		forceScoreCell (r,c);
		c = column ;
		forceScoreCell (r,c);
		c = column + 1;
		forceScoreCell (r,c);
	}

	public void BombRow (int row, int column){
		int r = row;
		int c = column;
		for (c = 0; c < columns; c++) {
			forceScoreCell (r, c);
		}
	}
	public void BombColumn (int row, int column){
		int r = row;
		int c = column;
		for (r = 0; r < rows; r++) {
			forceScoreCell (r, c);
		}
	}

	void displayText () {
		/*check for conditions when to display text
		 *eg when you collected 50 watergems 
		 * when you are below 50 hp
		 * when enemy died
		 * */

		//send text and characterGFX to UI mngr
		//GFX can be gameobj(unique) or material/texture (generic head)
	}

	bool forceScoreCell (int row, int column){	
		if(row>=0 && row<rows && column>=0 && column<columns && cellgrid [row, column].scoring==false ){ //&& cellgrid [row, column].Busy==false
			ScoreCell (cellgrid [row, column]);
			return true;
		}
		return false;
	}

}

