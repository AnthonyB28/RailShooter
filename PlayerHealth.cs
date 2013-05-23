using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	
	/// <summary>
	/// Player health script. Attach to PlayerTarget object.
	/// Player can only lose health if canBeHit is true.
	/// If health = 0, then game over.
	/// </summary>
	
	public int health = 3;
	public bool canBeHit = true;
	public bool playerHit = false; //Controls invulnerability
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButtonDown(1))
		{
			canBeHit = false;
		}
		if(Input.GetMouseButtonUp(1))
		{
			canBeHit = true;
		}
		
		if(health == 0)
		{
			//GameOver
		}
	}
}
