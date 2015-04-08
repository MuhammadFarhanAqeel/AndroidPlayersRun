using UnityEngine;
using System.Collections;

public class destroyObjects : MonoBehaviour {

	void OnCollisionEnter(Collision other){
		Destroy (other.gameObject);
	}
}
