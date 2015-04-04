using UnityEngine;
using System.Collections;

public class GameControlScript : MonoBehaviour {

	float timeRemaining = 10f; // pre-earned time
	float timeExtension = 2f; //time to extend by colleting powerup
	float timeDeduction = 4f; //time to reduce on collecting the snag
	float totalTimeElapsed = 0; 
	public float score = 0f;			// total score
	public bool isGameOver = false; 
	public GUISkin skin; 
	public GameObject countdown;
	public GameObject player;
	private PlayerControl control;


	void Start () {
		Time.timeScale = 1;
		control = GameObject.Find("Main Camera").GetComponent<PlayerControl> ();
	}

	void FixedUpdate () {
		if (isGameOver) 
			return; // if the gameover variable is true then it will jump out of the update method

		totalTimeElapsed += Time.deltaTime;
		score = totalTimeElapsed * 10;
		timeRemaining -= Time.deltaTime;

		if (timeRemaining <= 0) {
			isGameOver = true;
			control.gameOverSound.Play ();
		}


	}

	void OnGUI()
	{
		GUI.skin = skin;
		//check if game is not over, if so, display the score and the time left
		if (!isGameOver) {
			GUI.Label (new Rect (10, 10, Screen.width / 5, Screen.height / 6), "TIME LEFT: " + ((int)timeRemaining).ToString ());
			GUI.Label (new Rect (Screen.width - (Screen.width / 6), 10, Screen.width / 6, Screen.height / 6), "SCORE: " + ((int)score).ToString ());
		}
		//if game over, display game over menu with score
		else {
			player.SetActive (false);
			countdown.GetComponent<CountDownScript> ().pauseButton.enabled = false;
			Time.timeScale = 0; //set the timescale to zero so as to stop the game world
			
			//display the final score
			GUI.Box (new Rect (Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2), "GAME OVER\nYOUR SCORE: " + (int)score);
		}
	}

	public void powerUpCollected(){
		timeRemaining += timeExtension;
		control.GetComponent<PlayerControl>().powerUpCollectSound.Play ();
	}

	public void obstacleCollected(){
		timeRemaining -= timeDeduction;
		control.GetComponent<PlayerControl>().snagSound.Play ();
	}


	public void Quit_Game(){
		Application.Quit();
	}
}
