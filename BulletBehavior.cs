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
	private PlayerHealth playerHP;
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
	
	public double chance;
	
	// Use this for initialization
	void Start () {
		begin = gameObject.transform;
		player = GameObject.FindWithTag("Player");
		playerHP = player.GetComponent<PlayerHealth>();
		hit();
	}
	
	//Determines whether bullet is a hit or not randomly.
	//If not, then generate random projectile coordinates.
	void hit()
	{
		//End of the target line is the player.
		end = player.transform;
		if(Random.value >=chance)
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
		
		//PlayerHit
		else
		{
			transform.position = Vector3.Lerp(begin.transform.position, end.position, Time.deltaTime*velocityHit);
			//If bullet hits the player and can receive damage, subtract health.
			if(transform.position.z > player.transform.position.z-0.5
				& transform.position.z  < player.transform.position.z+0.5
				& playerHP.canBeHit
				& playerHP.playerHit == false)
			{
				playerHP.health -= 1;
				playerHP.playerHit = true;
			}
		}
		
		//Destroy the fired bullet prefabs
		timer += Time.deltaTime;
		if(timer > waitTime)
		{
			GameObject.Destroy(gameObject);
		}
		if(transform.position.z > player.transform.position.z-0.5
				& transform.position.z  < player.transform.position.z+0.5)
		{
			GameObject.Destroy(gameObject);
		}
	}
	
	//Destroy bullets if they hit any obstacles
	void OnCollisionEnter( Collision Target)
	{
		if(Target.gameObject.tag == "Obstacles")
		{
		Destroy(gameObject);
		}
	}
}
