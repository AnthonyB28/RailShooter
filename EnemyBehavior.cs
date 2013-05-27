using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	
	
	/// <summary>
	/// This script controls the enemies.
	/// Enemies can only shoot if this script tells them to.
	/// Controls enemies based on event. PathEvent sends what event this is and how many enemies in the event.
	/// </summary>
	
	public bool canFire = false;
	public bool movingEvent = true;
	public int gameEvent = 0; //Set by PathEvent
	public int enemyCount;  //Set by PathEvent
	public int alive = 0; //Set by enemies spawning
	public int dead; //Set by enemies dying
	public bool spawned = false;
	public bool nextEvent = false;
	
	
	//Enemies
	public GameObject enemy1, enemy2, enemy3;
	public GameObject enemy1Spawn1, enemy2Spawn1, enemy3Spawn1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//Check what gameEvent this is, then active scripts.
		if(gameEvent == 1)
		{
			//If all enemies spawned are dead, then do this.
			if(alive == 0)
			{
				if(!spawned)
				{
					spawnWave(1);
				}
				
				//If all enemies IN THE EVENT are dead, then continue the game.
				//Personal note: I programmed this in without realizing it. I had no idea what I was doing with this comparison.
				if(dead == enemyCount)
				{
					
					//Continue command
					dead = 0;
					nextEvent = true;
					//spawned = false;
				}
			}
		}
		if(gameEvent == 1)
		{
			Debug.Log ("GOT IT");
		}
	}
	
	void spawnWave(int num)
	{
		if(gameEvent == 1 & !spawned)
		{
			if(num == 1)
			{
				spawned = true;
				Instantiate(enemy1,enemy1Spawn1.transform.position,Quaternion.identity);
				Instantiate(enemy2,enemy2Spawn1.transform.position,Quaternion.identity);
				Instantiate(enemy3,enemy3Spawn1.transform.position,Quaternion.identity);
				GameObject.Find("enemy1").GetComponent<AenemyAI>().speed = 1200;
				GameObject.Find("enemy2").GetComponent<AenemyAI>().speed = 900;
				
			}
		}
	}
}
