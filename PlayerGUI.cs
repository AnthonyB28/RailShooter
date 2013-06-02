using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {
	
	/// <summary>
	/// The player GUI script.
	/// Handles the HP & Ammo GUI textures.
	/// Will also handle score too.
	/// Attach to camera.
	/// </summary>
	
	public Texture hp;
	public Texture ammo;
	
	private PlayerHealth healthScript;
	private Fire ammoScript;
	
	//X and Y locations for the ammo
	private float loc1, loc2, loc3, loc4, loc5, loc6, loc7, loc8, loc9;
	private float locy1= 200f, locy2= 200f, locy3= 200f, locy4= 200f, locy5= 200f, locy6= 200f, locy7= 200f, locy8= 200f, locy9 = 200f;
	
	private float widthPos;
	//private bool enableGUI;
	
	// Use this for initialization
	void Start () {
	healthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	ammoScript = gameObject.GetComponent<Fire>();
	widthPos = Screen.width/4;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		if(!healthScript.enableGUI)
		{
			//Draw the ammo
			GUI.DrawTexture(new Rect(widthPos-loc1,Screen.height-locy1,35,80),ammo); //1
			GUI.DrawTexture(new Rect(widthPos-loc2,Screen.height-locy2,35,80),ammo); //2
			GUI.DrawTexture(new Rect(widthPos-loc3,Screen.height-locy3,35,80),ammo); //3
			GUI.DrawTexture(new Rect(widthPos-loc4,Screen.height-locy4,35,80),ammo); //4
			GUI.DrawTexture(new Rect(widthPos-loc5,Screen.height-locy5,35,80),ammo); //5
			GUI.DrawTexture(new Rect(widthPos-loc6,Screen.height-locy6,35,80),ammo); //6
			GUI.DrawTexture(new Rect(widthPos-loc7,Screen.height-locy7,35,80),ammo); //7
			GUI.DrawTexture(new Rect(widthPos-loc8,Screen.height-locy8,35,80),ammo); //8
			GUI.DrawTexture(new Rect(widthPos-loc9,Screen.height-locy9,35,80),ammo); //9
			
			//Draw the health
			if(healthScript.health == 3)
			{
				GUI.DrawTexture(new Rect(Screen.width/2-100,Screen.height-100,75,75),hp);
				GUI.DrawTexture(new Rect(Screen.width/2,Screen.height-100,75,75),hp);
				GUI.DrawTexture(new Rect(Screen.width/2+100,Screen.height-100,75,75),hp);
			}
			if(healthScript.health == 2)
			{
				GUI.DrawTexture(new Rect(Screen.width/2-100,Screen.height-100,75,75),hp);
				GUI.DrawTexture(new Rect(Screen.width/2,Screen.height-100,75,75),hp);
			}
			if(healthScript.health == 1)
			{
				GUI.DrawTexture(new Rect(Screen.width/2-100,Screen.height-100,75,75),hp);
			}
			
			//Ammo will lerp to position
			if(ammoScript.ammo == 9)
			{
				loc1 = 450;
				loc2 = 415;
				loc3 = 380;
				loc4 = 345;
				loc5 = 310;
				loc6 = 275;
				loc7 = 240;
				loc8 = 200;
				loc9 = 155;
				locy1 = Mathf.Lerp(locy1,200,Time.deltaTime*9);
				locy2 = Mathf.Lerp(locy2,200,Time.deltaTime*9);
				locy3 = Mathf.Lerp(locy3,200,Time.deltaTime*9);
				locy4 = Mathf.Lerp(locy4,200,Time.deltaTime*9);
				locy5 = Mathf.Lerp(locy5,200,Time.deltaTime*9);
				locy6 = Mathf.Lerp(locy6,200,Time.deltaTime*9);
				locy7 = Mathf.Lerp(locy7,200,Time.deltaTime*9);
				locy8 = Mathf.Lerp(locy8,200,Time.deltaTime*9);
				locy9 = Mathf.Lerp(locy9,200,Time.deltaTime*9);
			}
			if(ammoScript.ammo == 8)
			{
				loc1 = Mathf.Lerp(loc1,415,Time.deltaTime*4);
				loc2 = Mathf.Lerp(loc2,380,Time.deltaTime*4);
				loc3 = Mathf.Lerp(loc3,345,Time.deltaTime*4);
				loc4 = Mathf.Lerp(loc4,310,Time.deltaTime*4);
				loc5 = Mathf.Lerp(loc5,275,Time.deltaTime*4);
				loc6 = Mathf.Lerp(loc6,240,Time.deltaTime*4);
				loc7 = Mathf.Lerp(loc7,200,Time.deltaTime*4);
				loc8 = Mathf.Lerp(loc8,155,Time.deltaTime*4);
				locy9 = Mathf.Lerp(locy9,-50,Time.deltaTime*8);
			}
			if(ammoScript.ammo == 7)
			{
				loc1 = Mathf.Lerp(loc1,380,Time.deltaTime*4);
				loc2 = Mathf.Lerp(loc2,345,Time.deltaTime*4);
				loc3 = Mathf.Lerp(loc3,310,Time.deltaTime*4);
				loc4 = Mathf.Lerp(loc4,275,Time.deltaTime*4);
				loc5 = Mathf.Lerp(loc5,240,Time.deltaTime*4);
				loc6 = Mathf.Lerp(loc6,200,Time.deltaTime*4);
				loc7 = Mathf.Lerp(loc7,155,Time.deltaTime*4);
				locy8 = Mathf.Lerp(locy8,-50,Time.deltaTime*8);
			}
			if(ammoScript.ammo == 6)
			{
				loc1 = Mathf.Lerp(loc1,345,Time.deltaTime*4);
				loc2 = Mathf.Lerp(loc2,310,Time.deltaTime*4);
				loc3 = Mathf.Lerp(loc3,275,Time.deltaTime*4);
				loc4 = Mathf.Lerp(loc4,240,Time.deltaTime*4);
				loc5 = Mathf.Lerp(loc5,200,Time.deltaTime*4);
				loc6 = Mathf.Lerp(loc6,155,Time.deltaTime*4);
				locy7 = Mathf.Lerp(locy7,-50,Time.deltaTime*8);
			}
			if(ammoScript.ammo == 5)
			{
				loc1 = Mathf.Lerp(loc1,310,Time.deltaTime*4);
				loc2 = Mathf.Lerp(loc2,275,Time.deltaTime*4);
				loc3 = Mathf.Lerp(loc3,240,Time.deltaTime*4);
				loc4 = Mathf.Lerp(loc4,200,Time.deltaTime*4);
				loc5 = Mathf.Lerp(loc5,155,Time.deltaTime*4);
				locy6 = Mathf.Lerp(locy6,-50,Time.deltaTime*8);
			}
			if(ammoScript.ammo == 4)
			{
				loc1 = Mathf.Lerp(loc1,275,Time.deltaTime*4);
				loc2 = Mathf.Lerp(loc2,240,Time.deltaTime*4);
				loc3 = Mathf.Lerp(loc3,200,Time.deltaTime*4);
				loc4 = Mathf.Lerp(loc4,155,Time.deltaTime*4);
				locy5 = Mathf.Lerp(locy5,-50,Time.deltaTime*8);
			}
			if(ammoScript.ammo == 3)
			{
				loc1 = Mathf.Lerp(loc1,240,Time.deltaTime*4);
				loc2 = Mathf.Lerp(loc2,200,Time.deltaTime*4);
				loc3 = Mathf.Lerp(loc3,155,Time.deltaTime*4);
				locy4 = Mathf.Lerp(locy4,-50,Time.deltaTime*8);
			}
			if(ammoScript.ammo == 2)
			{
				loc1 = Mathf.Lerp(loc1,200,Time.deltaTime*4);
				loc2 = Mathf.Lerp(loc2,155,Time.deltaTime*4);
				locy3 = Mathf.Lerp(locy3,-50,Time.deltaTime*8);
			}
			if(ammoScript.ammo == 1)
			{
				loc1 = Mathf.Lerp(loc1,155,Time.deltaTime*4);
				locy2 = Mathf.Lerp(locy2,-50,Time.deltaTime*8);
			}
			if(ammoScript.ammo == 0)
			{
				locy1 = Mathf.Lerp(locy1,-50,Time.deltaTime*8);
			}
		}
			
	}
}
