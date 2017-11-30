using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour {
    public float scrollspeed;
    public float tileSizedZ;
    private Vector3 startPosition;
	
	void Start ()
    {
        startPosition = transform.position;
       
	}
	
	
	void Update ()
    {
        float newPostion = Mathf.Repeat(Time.time * scrollspeed, tileSizedZ);
        transform.position = startPosition + Vector3.forward * newPostion;
    }
		
	
}
