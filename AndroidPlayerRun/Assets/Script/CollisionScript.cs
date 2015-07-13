using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour {
	public GameControlScript control;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "powerup") {
            Debug.Log("Powerup Collected! ");
			control.powerUpCollected();
			Destroy(other.gameObject);
		}
		if (other.gameObject.tag == "obstacle") {
			control.obstacleCollected();
            Debug.Log("Obstacle collected!");
			Destroy(other.gameObject);
		}
	}
}
