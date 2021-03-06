using UnityEngine;
using System.Collections;

public class EnemyFire : MonoBehaviour {
	
	/// <summary>
	/// Attach script to enemy.
	/// Assign bullet projectile Prefab.
	/// Controls how and how often bullets are fired from enemy at player.
	/// </summary>
	
	public Transform bullet; // the bullet prefab
	public float timer;
	public int waitTime = 2;
	private EnemyBehavior behave;
	

	// Use this for initialization
	void Start () 
	{
		behave = GameObject.Find("EnemyManager").GetComponent<EnemyBehavior>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(behave.canFire)
		{
			timer += Time.deltaTime;
			if(timer > waitTime)
			{
					GameObject.Instantiate(bullet,transform.position,Quaternion.Euler(270,-10,0));
					timer = 0;
			}
		}
	}
	
	
	
	
	
	
}
