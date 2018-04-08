using UnityEngine;
using System.Collections;

public class Explosion01a : MonoBehaviour {

	/*public float timerToDestroy;
	public bool DestroyParticle = true;*/

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
			
		if(gameObject.GetComponent<ParticleSystem>().isPlaying == false){
			Destroy(gameObject);
		}

	}
}
