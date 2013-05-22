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
public Transform bullet; // the bullet prefab
public Texture bulletHole;
public GUITexture bulletFX;


	// Use this for initialization
	void Start () {
	}
	

void Update(){   
		
  if (Input.GetMouseButtonDown(0))
	{ // only do anything when the button is pressed:
			
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); //Fires a ray according to where mouse is clicked
		
		//This is the Bullethole guiTexture
		GUITexture.Instantiate(
				bulletFX, 
				new Vector3((Input.mousePosition.x-16)/Screen.width,(Input.mousePosition.y-18)/Screen.height, 0),
				Quaternion.identity
				);
		//http://forum.unity3d.com/threads/123624-GUITexture-following-mouse-position-on-the-screen
		
		//If ray hits anything, do this.
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
		  Debug.DrawLine (character.position, hit.point);
		
		}
	}
		
	
 
}

	


}
