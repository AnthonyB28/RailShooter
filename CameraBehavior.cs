using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {
	
	/// <summary>
	/// Attached to the Main Camera.
	/// Controlls how the camera will search for CameraNodes.
	/// Shoots a raycast to find cameraNodes.
	/// Sets the nodes to self destruct after looking at them.
	/// Smooths out the animation with damp float.
	/// </summary>

	public GameObject target;
	public float damp = 2.0f;
	private GameObject [] exclude;
	private PlayerHealth eventScript;
	private CameraNodes node;
	
	// Use this for initialization
	void Start () {
	eventScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position=GameObject.FindGameObjectWithTag("Player").transform.position;
		//Only change camera angles if player is moving
		if(eventScript.waitEvent)
		{
			target = FindClosestCamera();
			
			//Prevent errors and then set node to self destruct. Look at node.
			if(target != null & target.gameObject.name != "Main Camera")
			{
				//iTween.RotateTo(gameObject,Quaternion.LookRotation(target.transform.position),1.0f);
				//transform.LookAt(target.transform);
				node = target.GetComponent<CameraNodes>();
				node.lookedAt = true;
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation(target.transform.position-transform.position),damp*Time.deltaTime);
			}
		}
	}
	
	//Searches for the closest Camera node after raycast.
	public GameObject FindClosestCamera()
	{
		GameObject [] pathTarget = GameObject.FindGameObjectsWithTag("Camera");
		
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
		GameObject closestCam = gameObject;
        foreach (GameObject go in pathTarget) 
		{
			//Only grab the nodes that can be hit with a raycast. As they are not behind walls.
			if(!Physics.Linecast(transform.position,go.transform.position))
			{
	            Vector3 diff = go.transform.position - position;
	            float curDistance = diff.sqrMagnitude;
	            if (curDistance < distance) 
				{
	                closestCam = go;
	                distance = curDistance;
	            }
			}
        }
		
        	return closestCam;
		
	}
}
