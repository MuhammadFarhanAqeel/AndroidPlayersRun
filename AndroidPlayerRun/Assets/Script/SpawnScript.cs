using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	public GameObject Obstacle;
	public GameObject powerup;
	public GameObject Player;

	float timeElapsed = 0;
	public float spawnCycle=0.4f;
	bool spawnPowerUp;

	void FixedUpdate () {
		timeElapsed += Time.deltaTime;
		Vector3 pos = Player.transform.position;

		if (timeElapsed > spawnCycle) {
			GameObject temp;
			if(spawnPowerUp){
				temp = (GameObject)Instantiate(powerup);
				temp.transform.position = new Vector3( Random.Range(-6.0f,6.0f),pos.y ,pos.z + Random.Range(150,250));
			}
				else{
					temp = (GameObject)Instantiate(Obstacle);
				temp.transform.position = new Vector3( Random.Range(-6.0f,6.0f),pos.y ,pos.z + Random.Range(150,250));
			}
			timeElapsed -= spawnCycle;
			spawnPowerUp =! spawnPowerUp;
			}
		}
}