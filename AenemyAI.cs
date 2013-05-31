using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;

public class AenemyAI : MonoBehaviour {
    //The point to move to
    public Vector3 targetPosition;
    
    private Seeker seeker;
    private CharacterController controller;
	private GameObject [] pathTarget;
	private GameObject closestPath;
	//private GameObject closestDuck;
	//private GameObject camera;
	//private PathEvent eventScript;
	//private PlayerHealth waitScript;
	//private PlayerHealth script;
 
    //The calculated path
    public Path path;
	//public Path duckPath;
	
	//public bool duckEvent, beginDuck, returnDuck, firstReturnSearch, lockpos = false;
	//public bool firstDuckSearch = true;
    
    //The AI's speed per second
    public float speed = 400;
	//public float duckSpeed = 600;
	
	public bool canMove = true;
	public bool arcade = false;
	
	public float startpeed;
	
	private EnemyBehavior enemy;
    
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
 
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
	
 
    public void Start () {
		
		enemy = GameObject.Find("EnemyManager").GetComponent<EnemyBehavior>();
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
		//script = GetComponent<PlayerHealth>();
		//eventScript = FindClosestPath().GetComponent<PathEvent>();
		//waitScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
		
		startpeed = speed; //Store the speed of movement just incase.
        
		
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        seeker.StartPath (transform.position,FindClosestPath().transform.position, OnPathComplete);
    }
	
	//Searches for the closest firingNode
	public GameObject FindClosestPath()
	{
		if(gameObject.tag == "Enemy1")
		{
		pathTarget = GameObject.FindGameObjectsWithTag("Enemy1Path");
		}
		
		else if(gameObject.tag == "Enemy2")
		{
			pathTarget = GameObject.FindGameObjectsWithTag("Enemy2Path");
		}
		
		else if(gameObject.tag == "Enemy3")
		{
			pathTarget = GameObject.FindGameObjectsWithTag("Enemy3Path");
		}
		
		else
		{}
		
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

        }
		
        return closestPath;
	}
	
	/*//Searches for the closest duckNode
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
			seeker.StartPath (transform.position,FindClosestDuck().transform.position, OnDuckPathComplete);
			firstDuckSearch = false;
		}
		if(firstReturnSearch)
		{
			seeker.StartPath (transform.position,FindClosestPath().transform.position, OnPathComplete);
			firstReturnSearch = false;
		}
	}*/
	
	//Stores the firing path
    public void OnPathComplete (Path p) {
        Debug.Log ("Yey, we got a path back FOR ENEMY. Did it have an error? "+p.error);
        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
			Destroy(closestPath);
        }
    }
	
	/*//Stores the duck path
	public void OnDuckPathComplete(Path p)
	{
		duckPath = p;
		currentWaypoint = 0;
	}*/
 
    public void FixedUpdate () {
        if (path == null) {
            //We have no path to move after yet
            return;
        }
        
		//Player is only moving inbetween gameplay
		if(!enemy.movingEvent & canMove)
		{
			//REACHED END OF CURRENT PATH
	        if (currentWaypoint >= path.vectorPath.Count) {
	            Debug.Log ("End Of ENEMY Path Reached");
				currentWaypoint = 0;
				if(!arcade) //If arcade enabled, enemy will move back and fourth continously.
				{
					canMove = false;
					Destroy(closestPath);
				}
	            return;
	        }
	        
			
			//Only move if the player is not hit and not ducking
			
	        //Direction to the next waypoint
	        Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
	        dir *= speed * Time.fixedDeltaTime;
	        controller.SimpleMove (dir);
			
	        //Check if we are close enough to the next waypoint
	        //If we are, proceed to follow the next waypoint
	        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) 
			{
	            currentWaypoint++;
	            return;
	     
			}
		 }
		
		/*//If player right clicks, enter ducking event.
		//Calls Duck(), moves to duckNode.
		//If player lets go, calls Duck() and moves back to firing node.
		//I feel this could be enhanced more, storing the paths instead of always searching.
		if(duckEvent)
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
		}*/
		
				
				
	 }

}