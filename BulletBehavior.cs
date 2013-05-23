using UnityEngine;
using System.Collections;

public class BulletBehavior : MonoBehaviour {
	
	/// <summary>
	/// Attach script to bullet projectile prefab.
	/// Will run single Start function to determine projectile hit or miss.
	/// Bullet destroys itself after waitTime, could use tweaking?
	/// Speed can be changed with velocity var in Update()
	/// </summary>
	
	private GameObject player;
	public Transform begin; //shooter
	private Transform end;
	public Material hitMat;
	public int velocityHit = 4;
	public int velocityMiss = 4;
	
	private bool notHit = false;
	private int randomX;
	private int randomY;
	
	private float timer;
	private int waitTime = 5;

	// Use this for initialization
	void Start () {
		begin = gameObject.transform;
		player = GameObject.FindWithTag("MainCamera");
		hit();
	}
	
	//Determines whether bullet is a hit or not randomly.
	//If not, then generate random projectile coordinates.
	void hit()
	{
		//End of the target line is the player.
		end = player.transform;
		if(Random.value >=0.5)
		{
			notHit = false;
			gameObject.renderer.material = hitMat;
		}
		else
		{
			notHit = true;
			randomX = Random.Range (-2,2);
			randomY = Random.Range (-1,2);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//Bullets are fired.
		//If a no-hit bullet, use the random vars to project bullet toward player.
		//Else, go right at the the player.
		//Time * # can be changed to increase speed.
		if(notHit)
		{
			transform.position = Vector3.Lerp (
				begin.transform.position,
				new Vector3(end.position.x+randomX,end.position.y+randomY,end.position.z),
				Time.deltaTime*velocityMiss
				);
		}
		else
		{
			transform.position = Vector3.Lerp(begin.transform.position, end.position, Time.deltaTime*velocityHit);
		}
		
		//Destroy the fired bullet prefabs
		timer += Time.deltaTime;
		if(timer > waitTime)
		{
			GameObject.Destroy(gameObject);
		}
	}
}
