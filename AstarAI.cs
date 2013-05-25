using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
public class AstarAI : MonoBehaviour {
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
 
    public void Start () {
		enemy = GameObject.Find("EnemyManager").GetComponent<EnemyBehavior>();
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
		startpeed = speed;
        script = GetComponent<PlayerHealth>();
		//camera = GameObject.FindGameObjectWithTag("MainCamera");
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
		eventScript = FindClosestPath().GetComponent<PathEvent>();
		waitScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        seeker.StartPath (transform.position,FindClosestPath().transform.position, OnPathComplete);
    }
	
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
			else
			{
				closestDuck = go;
			}
        }
		
        return closestDuck;
	}
	
	public void Duck()
	{
		if(firstDuckSearch)
		{
			seeker.StartPath (transform.position,FindClosestDuck().transform.position, OnPathComplete);
			firstDuckSearch = false;
		}
		if(firstReturnSearch)
		{
			seeker.StartPath (transform.position,FindClosestPath().transform.position, OnPathComplete);
			firstReturnSearch = false;
		}
	}
	
    public void OnPathComplete (Path p) {
        Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
			
        }
    }
 
    public void FixedUpdate () {
        if (path == null) {
            //We have no path to move after yet
            return;
        }
        
		if(movingEvent)
		{
			//REACHED END OF CURRENT PATH
	        if (currentWaypoint >= path.vectorPath.Count) {
	            Debug.Log ("End Of Path Reached");
				currentWaypoint = 0;
				if(eventScript.isThisEvent)
				{
					enemy.canFire = true;
					movingEvent = false;
					waitScript.waitEvent = eventScript.waitEvent;
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
		
		if(duckEvent)
		{
			
			//First click duck
			if(beginDuck & !returnDuck)
			{
				Duck ();
				if (currentWaypoint >= path.vectorPath.Count & beginDuck) 
				{
					currentWaypoint = 0;
					lockpos = true;
					Debug.Log ("HITITITITITITITITITIT");
	       		}
				
				if(!lockpos)
				{
			        //Direction to the next waypoint
			        Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
			        dir *= duckSpeed * Time.fixedDeltaTime;
			        controller.SimpleMove (dir);
					Debug.Log ("DOINGTHIS");
					
			        //Check if we are close enough to the next waypoint
			        //If we are, proceed to follow the next waypoint
			        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) 
					{
						
			            currentWaypoint++;
			            return;
			     
					}
				}
			}
			
			if(returnDuck & !beginDuck)
			{
				Duck ();
				if (currentWaypoint >= path.vectorPath.Count) 
				{
		            Debug.Log ("End Of Duck Reached");
					currentWaypoint = 0;
					returnDuck = false;
					duckEvent = false;
					firstDuckSearch = true;
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
		
				
				
	 }

}