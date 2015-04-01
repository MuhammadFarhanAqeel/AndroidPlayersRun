 using UnityEngine;
using System.Collections;

public class CountDownScript : MonoBehaviour {

	public GameObject character;
	public GameObject backWall;
	public GameObject mainCamera;
	private int countMax =  3; 
	private int countDown; //current CountDown number
	public GUIText guiTextCountDown; // reference of GUIText
	public Canvas pauseButton;
	public AudioSource countDownSound;
	public bool isCountDown = false;

	void Start () {
		character.GetComponent<AudioSource> ().enabled = false;

		MonoBehaviour[] scriptComponentGameControl = gameObject.GetComponents<MonoBehaviour> ();
		foreach (MonoBehaviour script in scriptComponentGameControl) {
			script.enabled = false;
		}
			character.GetComponent<CollisionScript>().enabled = false;
			backWall.GetComponent<destroyObjects>().enabled = false;
			mainCamera.GetComponent<PlayerControl>().enabled = false;
			character.GetComponent<Animation>().enabled = false;
		character.SetActive (false);
			pauseButton.enabled = false;
			StartCoroutine(CountDownFunction());
		}

	IEnumerator CountDownFunction(){
		countDownSound.PlayDelayed (.4f);
		for(countDown = countMax; countDown>-1;countDown--){
			if(countDown!=0){
				//display the number to the screen via the GUIText
				guiTextCountDown.text = countDown.ToString();
				//add a one second delay
				yield return new WaitForSeconds(1);    
			}
			else{
				guiTextCountDown.text = "GO!";
				yield return new WaitForSeconds(1);

				countDownSound.Stop();
				isCountDown = true;
			}
		}
		MonoBehaviour[] scriptComponentGameControl = gameObject.GetComponents<MonoBehaviour> ();
		foreach (MonoBehaviour script in scriptComponentGameControl) {
			script.enabled = true;
		}

		character.GetComponent<CollisionScript> ().enabled = true;
		backWall.GetComponent<destroyObjects> ().enabled = true;
		mainCamera.GetComponent<PlayerControl> ().enabled = true;
		character.GetComponent<Animation> ().enabled = true;
		character.GetComponent<AudioSource> ().enabled = true;
		guiTextCountDown.enabled = false;
		character.SetActive (true);
		pauseButton.enabled = true;	
	}
}