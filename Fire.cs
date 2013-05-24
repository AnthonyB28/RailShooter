using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	/// <summary>
	/// Main player shooting functionality.
	/// Player left clicks and a ray is cast. This is the hit detection.
	/// Like firing a bullet.
	/// Uppon click, a GUI Texture bullet hole appears.
	/// </summary>
		
	public Transform character; //main character
	//public Transform bullet; // the bullet prefab
	private PlayerHealth playerHP;
	public GUITexture bulletFX;
	public GameObject target;
	public Material hitMat;
	public EnemyHealth targetHP;
	
	public int ammo = 9;
	
	// Use this for initialization
	void Start () {
	playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}

	void Update()
	{
	//Player can only fire if in firing position
	if(playerHP.canBeHit)
		{
		  if (Input.GetMouseButtonDown(0) & ammo != 0)
			{ // only do anything when the button is pressed:
					
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); //Fires a ray according to where mouse is clicked
				
				//This is the Bullethole guiTexture
				GUITexture.Instantiate(
						bulletFX, 
						new Vector3((Input.mousePosition.x-16)/Screen.width,(Input.mousePosition.y-18)/Screen.height, 0),
						Quaternion.identity
						);
				//http://forum.unity3d.com/threads/123624-GUITexture-following-mouse-position-on-the-screen
				if(playerHP.canShootEnemy)
				{
					ammo -=1;
					//If ray hits anything, do this.
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit))
					{
					  Debug.DrawLine (character.position, hit.point);
						
						
					  //Enemy targeting. If the beam hits, gets gameobject it hit.
					  //Compare the tag with enemy types and do damage when hit.		
					  target = hit.collider.gameObject;
					  targetHP = target.GetComponent<EnemyHealth>();
						
					   if(target.tag == "Enemy1")
							{
								targetHP.health -= 100;
							}
						
						if(target.tag == "Enemy2")
							{
					  			target.renderer.material = hitMat;
								targetHP.health -= 50;
								if(targetHP.health <= 0)
								{
									Destroy(target);
								}
							}
						
						//If target hit is an enemy and has 0 health, kill it.
						if(target.tag != "Untagged" & target.tag != "Ground")
						{
						  if(targetHP.health <= 0 )
								{
									Destroy(target);
								}
						}
					}
				}
			}
		}
		
		else
		{
			ammo = 9;
		}
			
	}
	
	
	
	
	
	
	
	
}
