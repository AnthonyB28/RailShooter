using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
public class AstarAI : MonoBehaviour {
	
	/// <summary>
	/// Crazy playerAI script.
	/// Admiteddly don't know how much of this I understood when writing it.
	///
	/// Looks for a path to follow. Upon reaching it, checks pathNode for event information.
	/// Relays event information to enemy manager to spawn/control enemies.
	/// 
	/// Controls the duck mechanic movement.
	/// PlayerHealth tells this script when the player presses/releases duck button.
	/// Duck() will search for the closest duckNode and move player to it on press.
	/// Duck() will also search for the closest pathNode and move player to it on release.
	/// Throws all kinds of bools around to accomplish movement.
	/// 
	/// Manages speed of player movement and duck.
	/// 
	/// Must have Seeker component attached, critical.
	/// </summary>
	
    //The point to move to
    public Vector3 targetPosition;
    
    private Seeker seeker;
    private CharacterController controller;
	private GameObject [] pathTarget;
	private GameObject closestPath;
	private GameObject closestDuck;
	//private GameObject camera;
	private PathEvent eventScript;
	private PlayerHealth waitScript;
 
    //The calculated path
    public Path path;
	public Path duckPath;
	
	public bool movingEvent = true;
	public bool duckEvent, beginDuck, returnDuck, firstReturnSearch, lockpos = false;
	public bool firstDuckSearch = true;
    
    //The AI's speed per second
    public float speed = 400;
	public float duckSpeed = 600;
	
	public bool canMove = false;
	
	public float startpeed;
	
	private EnemyBehavior enemy;
    
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
 
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
	
	private PlayerHealth script;
	
	private bool destroyedEvent = false;
 
    public void Start () {
		
		enemy = GameObject.Find("EnemyManager").GetComponent<EnemyBehavior>();
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
		script = GetComponent<PlayerHealth>();
		waitScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
		
		startpeed = speed; //Store the speed of movement just incase.
        
		
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        seeker.StartPath (transform.position,FindClosestPath().transform.position, OnPathComplete);
    }
	
	//Searches for the closest firingNode
	public GameObject FindClosestPath()
	{
		pathTarget = GameObject.FindGameObjectsWithTag("Path");
		
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in pathTarget) 
		{
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) 
			{
                closestPath = go;
                distance = curDistance;
            }
			else
			{
				closestPath = go;
			}
        }
		
        return closestPath;
	}
	
	public void nextEvent ()
	{
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
		eventScript = FindClosestPath().GetComponent<PathEvent>();
		waitScript.waitEvent = eventScript.waitUpToEvent;
		enemy.gameEvent = eventScript.eventNum;
        seeker.StartPath (transform.position,FindClosestPath().transform.position, OnPathComplete);
		movingEvent = true;
		firstDuckSearch = true;
		
	}
	
	//Searches for the closest duckNode
	public GameObject FindClosestDuck()
	{
		pathTarget = GameObject.FindGameObjectsWithTag("Duck");
		
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in pathTarget) 
		{
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) 
			{
                closestDuck = go;
                distance = curDistance;
            }
        }
		
        return closestDuck;
	}
	
	public void Duck()
	{
		if(firstDuckSearch)
		{
			seeker.StartPath (transform.position,FindClosestDuck().transform.position, OnDuckPathComplete);
			firstDuckSearch = false;
		}
		if(firstReturnSearch)
		{
			seeker.StartPath (transform.position,FindClosestPath().transform.position, OnPathComplete);
			firstReturnSearch = false;
		}
	}
	
	//Stores the firing path
    public void OnPathComplete (Path p) {
        Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
			
        }
    }
	
	//Stores the duck path
	public void OnDuckPathComplete(Path p)
	{
		duckPath = p;
		currentWaypoint = 0;
	}
 
    public void FixedUpdate () {
        if (path == null) {
            //We have no path to move after yet
            return;
        }
		
		//if(enemy.nextEvent)
		//{
		//	nextEvent();
		//	enemy.nextEvent = false;
	//	}
        
		//Player is only moving inbetween gameplay
		if(movingEvent)
		{
			//REACHED END OF CURRENT PATH
	        if (currentWaypoint >= path.vectorPath.Count) {
	            Debug.Log ("End Of Path Reached");
				currentWaypoint = 0;
				eventScript = FindClosestPath().GetComponent<PathEvent>();
				if(eventScript.isThisEvent)
				{
					enemy.canFire = true;
					movingEvent = false;
					enemy.movingEvent = false;
					waitScript.waitEvent = eventScript.waitEvent;
					enemy.nextEvent = false;
					enemy.spawned = false;
					
				}
	            return;
	        }
	        
			
			//Only move if the player is not hit and not ducking
			if(script.playerHit == false  & script.canShootEnemy & canMove)
			{
	        //Direction to the next waypoint
	        Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
	        dir *= speed * Time.fixedDeltaTime;
	        controller.SimpleMove (dir);
			}
			
	        //Check if we are close enough to the next waypoint
	        //If we are, proceed to follow the next waypoint
	        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) 
			{
	            currentWaypoint++;
	            return;
	     
			}
		 }
		
		//If player right clicks, enter ducking event.
		//Calls Duck(), moves to duckNode.
		//If player lets go, calls Duck() and moves back to firing node.
		//I feel this could be enhanced more, storing the paths instead of always searching.
		if(duckEvent & !script.playerHit)
		{
			
			//Begin ducking to duckNode
			if(beginDuck & !returnDuck)
			{
				Duck ();
				//If player reaches end of path
				if (currentWaypoint >= duckPath.vectorPath.Count & beginDuck) 
				{
					currentWaypoint = 0;
					lockpos = true;
	       		}
				
				//As long as player is not at the end of the path
				if(!lockpos)
				{
			        //Direction to the next waypoint
			        Vector3 dir = (duckPath.vectorPath[currentWaypoint]-transform.position).normalized;
			        dir *= duckSpeed * Time.fixedDeltaTime;
			        controller.SimpleMove (dir);
					
			        //Check if we are close enough to the next waypoint
			        //If we are, proceed to follow the next waypoint
			        if (Vector3.Distance (transform.position,duckPath.vectorPath[currentWaypoint]) < nextWaypointDistance) 
					{
						
			            currentWaypoint++;
			            return;
			     
					}
				}
			}
			
			//End ducking to firing position
			if(returnDuck & !beginDuck)
			{
				Duck ();
				
				//Reaching end of firing path
				if (currentWaypoint >= path.vectorPath.Count) 
				{
		            Debug.Log ("End Of Duck Reached");
					currentWaypoint = 0;
					returnDuck = false;
					duckEvent = false;
					//firstDuckSearch = true;
	       		 }
				
				//Direction to the next waypoint
		        Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		        dir *= duckSpeed * Time.fixedDeltaTime;
		        controller.SimpleMove (dir);
				
		        //Check if we are close enough to the next waypoint
		        //If we are, proceed to follow the next waypoint
		        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) 
				{
		            currentWaypoint++;
		            return;
		     
				}
			}
		}
		
		if(enemy.nextEvent)
		{
			Debug.Log ("nextEvent FIRING");
			if(!destroyedEvent)
			{
				Destroy(FindClosestPath());
				destroyedEvent = true;
			}
			
			FindClosestPath();
			nextEvent();
			
		}
				
	 }

}