using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	
	/// <summary>
	/// Enemy health
	/// Increase alive count if spawned.
	/// Decrease alive count if killed.
	/// Increase dead count if killed.
	/// </summary>
	/// 
	public int health = 100;
	private EnemyBehavior script;

	// Use this for initialization
	void Start () {
	script = GameObject.Find("EnemyManager").GetComponent<EnemyBehavior>();
	script.alive++;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(health <= 0)
		{
		 	script.alive--;
			script.dead++;
			Destroy(gameObject);
		}
	}
}
