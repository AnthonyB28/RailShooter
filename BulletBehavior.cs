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
	private Vector3 playerPos;
	public Material hitMat;
	
	public int velocityHit = 4;
	public int velocityMiss = 4;
	private float rotateY;
	private float rotateX;
	
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
		playerPos = end.position;
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
			transform.LookAt(playerPos);
			transform.Rotate(new Vector3(270,rotateY,transform.rotation.z));
			transform.position = Vector3.Lerp (
				begin.transform.position,
				new Vector3(playerPos.x+randomX,playerPos.y+randomY,playerPos.z-0.9f),
				Time.deltaTime*velocityMiss
				);
		}
		
		//PlayerHit
		else
		{
			rotateY = transform.rotation.y;
			Vector3 targetPos = new Vector3(playerPos.x,playerPos.y,playerPos.z);
			transform.position = Vector3.Lerp(begin.transform.position, targetPos, Time.deltaTime*velocityHit);
			transform.LookAt(playerPos);
			transform.Rotate(new Vector3(270,rotateY,transform.rotation.z));
			
			//Destroy the bullet if it reaches target, but does not collide.
			float distance = 0.2f;
			Vector3 diff = transform.position - targetPos;
	    	float curDistance = diff.sqrMagnitude;
			if(curDistance < distance)
			{
				Destroy(gameObject);
			}
			/*If bullet hits the player and can receive damage, subtract health.
			if(transform.position.z > player.transform.position.z-0.5
				& transform.position.z  < player.transform.position.z+0.5
				& transform.position.x > player.transform.position.x-1
				& transform.position.x < player.transform.position.x+1
				& transform.position.y > player.transform.position.y-1
				& transform.position.y < player.transform.position.y+1
				& playerHP.canBeHit
				& playerHP.playerHit == false)
			{
				playerHP.health -= 1;
				playerHP.playerHit = true;
			}*/
			
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
		Debug.Log (Target.gameObject.name);
		if(Target.gameObject.tag == "Obstacles")
		{
		Destroy(gameObject);
		}
		if(Target.gameObject.tag == "Player" || Target.gameObject.tag=="MainCamera")
		{
			if(playerHP.canBeHit & playerHP.playerHit == false & !notHit)
			{
				
				playerHP.health -= 1;
				playerHP.playerHit = true;
				Destroy(gameObject);
			}
		}
	}
}
