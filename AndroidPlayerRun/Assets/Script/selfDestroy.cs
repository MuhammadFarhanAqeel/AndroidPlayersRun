using UnityEngine;
using System.Collections;

public class selfDestroy : MonoBehaviour {

	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.y < -5) {
			Destroy(this.gameObject);
		} 
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Obstacle") {
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "powerUp") {
			Destroy(other.gameObject);
		}

	}

}
