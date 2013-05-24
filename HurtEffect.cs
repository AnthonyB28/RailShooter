using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to players and causes them
/// to see a hurt effect whenever they lose health.
/// Also controls how long the player remains invulnerable after hit.
/// </summary>

public class HurtEffect : MonoBehaviour {
	
	//Variables Start_________________________________________________________
	
	public Texture hurtEffect;
	
	private float previousHealth;
	
	private PlayerHealth script;
	
	private float displayTime = 0.8f;
	
	private bool displayHurtEffect = false;
	

	//Variables End___________________________________________________________
	
	
	// Use this for initialization
	void Start () 
	{
			
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			
		    script = player.GetComponent<PlayerHealth>();
			
			previousHealth = script.health;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
			
		if(script.health < previousHealth)
		{
			previousHealth = script.health;
			
			
			//Checking if displayHurtEffect is false stops the hurt effect
			//from flickering.
			
			if(displayHurtEffect == false)
			{
				displayHurtEffect = true;
				iTween.ShakePosition(gameObject, new Vector3(1,0,0), 1);
				
				StartCoroutine(StopDisplayingEffect());
				StartCoroutine (EnableHit());
			}
			
		}
		//If player continues after death, make sure can be hit still.
		if(script.revive)
		{
			previousHealth = 3;
			script.revive = false;
		}
	}
	
	void OnGUI ()
	{
		if(displayHurtEffect == true)
		{
			//The hurt effect is displyed using a DrawTexture and the texture is stretched to fill
			//the screen.
			
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), hurtEffect, ScaleMode.StretchToFill);
		}
	}
	
	
	//Let the hurt effect texture display for a bit before
	//setting the displayHurtEffect bool to false.
	
	IEnumerator StopDisplayingEffect() 
	{
		yield return new WaitForSeconds(displayTime);	
						
		displayHurtEffect = false;
		
	}
	IEnumerator EnableHit()
	{
		yield return new WaitForSeconds(1.2f);
		script.playerHit = false;
	}
}
