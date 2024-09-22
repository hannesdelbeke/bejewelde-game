using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {



	//display mana
	// hp
	public GameObject manaBar ;
	public GameObject healthBar ;
	public GameObject healthBarEnemy ;
	public GameObject expBar ;
	public GameObject Levels ;
	public GameObject goldLabel ;
	public GameObject winScreen ;
	public GameObject loseScreen;


	public GameObject[] activate_Game; 
	public GameObject[] activate_World; 

	public bool winRound = false;
	public bool loseRound = false;
	private float healthEnemy = 100;
	private float maxHealthEnemy = 100;
	private float exp = 0;
	private float maxExp = 20;
	private float health = 100;
	private float maxHealth = 100;
	private float gold = 0;
	private float mana = 0;
	private float maxMana = 200;
	public float Mana				
	{
		get { 
			return mana ; 	}
		set	{ 
			if (value < 0)
				value = 0;
			mana = value ; 
			//animating = value;
		}
	}
	public float Health				
	{
		get { 
			return health ; 	}
		set	{ 
			if (value < 0)
				value = 0;
			health = value ; 
			//animating = value;
		}
	}
	public float Gold				
	{
		get { 
			return gold ; 	}
		set	{ 
			if (gold < 0)
				gold = 0;
			gold = value ; 
			//animating = value;
		}
	}
	public float Exp				
	{
		get { 
			return exp ; 	}
		set	{ 
			exp = value ; 
			//animating = value;
		}
	}
	public float HealthEnemy			
	{
		get { 
			return healthEnemy ; 	}
		set	{ 
			if (value < 0)
				value = 0;
			healthEnemy = value ; 
			//animating = value;
		}
	}
	//update stuff


	//Levels.SetActive (false);


	//display in battle ui
	//display 

//	current mode

	// Use this for initialization
	void Start () {
		winScreen.SetActive(false) ;
		loseScreen.SetActive(false) ;
		enterGame (false);
	}
	
	// Update is called once per frame
	void Update () {

		expBar.GetComponent<Bar> ().maxValue = maxExp ;
		expBar.GetComponent<Bar> ().CurrentValue = exp ;

		manaBar.GetComponent<Bar> ().maxValue = maxMana ;
		manaBar.GetComponent<Bar> ().CurrentValue = mana ;

		healthBar.GetComponent<Bar> ().maxValue = maxHealth ;
		healthBar.GetComponent<Bar> ().CurrentValue = health ;

		healthBarEnemy.GetComponent<Bar> ().maxValue = maxHealthEnemy ;
		healthBarEnemy.GetComponent<Bar> ().CurrentValue = healthEnemy ;

		goldLabel.GetComponent<TextMesh> ().text = gold.ToString ();

		if (winRound) {
			//show screen
			winScreen.SetActive(true) ;
		}
		if (loseRound) {
			loseScreen.SetActive(true) ;
		}

	}
	public void enterGame (){
		enterGame (true);
	}
	public void enterGame (bool activate) {
		//hide world stuff

		//show game stuff
		foreach (GameObject obj in activate_Game) {
			obj.SetActive (activate);
		}
	}
	public void enterWorld (){
		enterWorld (true);
	}
	public void enterWorld (bool activate) {
		foreach (GameObject obj in activate_World) {
			obj.SetActive (activate);
		}
	}
}
