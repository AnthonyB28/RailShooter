using UnityEngine;
using System.Collections;

public class Firebk : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
public Transform character; //main character
public Transform bullet; // the bullet prefab
GameObject spawnPt; // holds the spawn point object

void Update(){   
  if (Input.GetMouseButtonDown(1)){ // only do anything when the button is pressed:
    Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit)){
      Debug.DrawLine (character.position, hit.point);
      // cache oneSpawn object in spawnPt, if not cached yet
      if (!spawnPt) spawnPt = GameObject.Find("oneSpawn");
      Transform projectile = Instantiate(bullet, spawnPt.transform.position, Quaternion.identity) as Transform; 
      // turn the projectile to hit.point
      projectile.LookAt(hit.point); 
      // accelerate it
      projectile.rigidbody.velocity = projectile.forward * 10;
    }
  }
}

}
