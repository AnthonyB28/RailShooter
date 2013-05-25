using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	
	/// <summary>
	/// Player health script. Attach to PlayerTarget object.
	/// Player can only lose health if canBeHit is true.
	/// If health = 0, then game over.
	/// 
	/// Also controls the ducking mechanic.
	/// </summary>
	
	public int health = 3;
	public int credits = 2;
	public bool canBeHit = true;
	public bool playerHit = false; //Controls invulnerability
	public bool revive = false;
	public bool canShootEnemy = true;
	private int countdownmax;
	private int countdownmin;
	private int countdown;
	private bool enableGUI = false;
	public bool waitEvent = true;
	
	private AstarAI duckScript;
	
	// Use this for initialization
	void Start () {
	duckScript = GameObject.Find("PlayerTarget").GetComponent<AstarAI>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!waitEvent)
		{
			//Duck down position
			if(Input.GetMouseButtonDown(1))
			{
				canBeHit = false;
				canShootEnemy = false;
				duckScript.duckEvent = true;
				duckScript.beginDuck = true;
			}
			
			//Firing position
			if(Input.GetMouseButtonUp(1))
			{
				canShootEnemy = true;
				StartCoroutine(EnableHit ());
				duckScript.beginDuck = false;
				duckScript.firstDuckSearch = false;
				duckScript.firstReturnSearch = true;
				duckScript.returnDuck = true;
				duckScript.lockpos = false;
			}
		}
		
		//Player dies, activate GUI, but prepare for revive
		if(health == 0)
		{
			Time.timeScale = 0;
			enableContinueGUI();
			health = 3;
			revive = true;
			playerHit = false;
		}
	}
	
	//When player health = 0, activate GameOver GUI
	void enableContinueGUI()
	{
		enableGUI = true;
		canShootEnemy = false;
		countdownmin = (int) Time.realtimeSinceStartup;
	}

	
	//Handles the GUI when the player has no health.
	//Continue or quit buttons do as named. A countdown from 15 will quit when 0
	//Continue only appears if credits > 0
	void OnGUI()
	{
		countdownmax = (int)Time.realtimeSinceStartup - countdownmin; //timer
		countdown = 15 - countdownmax;
		
		if(enableGUI)
		{
			GUI.Box(new Rect(0,0,Screen.width,Screen.height), ""); //BG
			GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 25),"" + countdown); //Timer
			GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 75, 100, 25),"CREDITS: " + credits); //Credits
			
			//Continue only appears if credits > 0
			if(credits > 0)
			{
				if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50),"CONTINUE ?"))
				{
					enableGUI = false;
					canShootEnemy = true;
					credits -= 1;
					Time.timeScale = 1;
				}
			}
			
			if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 75 , 200, 50),"GIVE UP ?"))
			{
				enableGUI = false;
				canShootEnemy = true;
				Time.timeScale = 1;
				Application.LoadLevel(Application.loadedLevelName); //Quit
			}
			
			
			if(countdown == 0)
			{
				enableGUI = false;
				canShootEnemy = true;
				Time.timeScale = 1;
				Application.LoadLevel(Application.loadedLevelName); //Quit
			}
		}
	}
	
	//Invulnerable after RMB release
	IEnumerator EnableHit()
	{
		yield return new WaitForSeconds(0.35f);
		canBeHit = true;
	}
}
