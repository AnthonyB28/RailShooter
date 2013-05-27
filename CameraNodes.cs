using UnityEngine;
using System.Collections;

public class CameraNodes : MonoBehaviour {
	public bool lookedAt = false;
	public float time = 0.3f;
	
	IEnumerator Die() 
	{
		yield return new WaitForSeconds(time);	
		Destroy(gameObject);
		
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 if(lookedAt)
		{
			StartCoroutine(Die ());
		}
	}
}
