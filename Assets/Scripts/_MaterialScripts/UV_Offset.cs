using UnityEngine;
using System.Collections;

public class UV_Offset : MonoBehaviour {
	public float scrollSpeed = 0.5f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		var offset = Time.time * scrollSpeed; 
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-offset / 3, 0);
	}
}
