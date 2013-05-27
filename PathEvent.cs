using UnityEngine;
using System.Collections;

public class PathEvent : MonoBehaviour {
	
	public bool isThisEvent; //PlayerAI checks this, if true then game will leave wait mode.
	public bool turnCameraEvent; //PlayerAI checks this, if true then some kind of cutscene.
	public bool waitEvent = false; //PlayerAI checks this, if true then player will not be able to shoot or duck.
	public int enemyEvent; //the number of enemies in the event IN TOTAL that the player has to kill
	public int eventNum; //the event number
	private GameObject Player;
	private EnemyBehavior enemyControl;
	private float distance = Mathf.Infinity;
	private bool shootEvent = false; //activate the event parameters only once (call once)

	// Use this for initialization
	void Start () 
	{
		Player = GameObject.FindGameObjectWithTag("Player");
		enemyControl = GameObject.Find("EnemyManager").GetComponent<EnemyBehavior>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If player gets close to path, initialize event and communicate with enemies.
	    Vector3 diff = Player.transform.position - transform.position;
	    float curDistance = diff.sqrMagnitude;
		if(curDistance < distance & !shootEvent)
		{
			enemyControl.gameEvent = eventNum;
			enemyControl.enemyCount = enemyEvent;
			shootEvent = true; //dont set again
		}
	}
}
