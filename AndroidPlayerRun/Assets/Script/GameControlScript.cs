using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameControlScript : MonoBehaviour {

	float timeRemaining = 10f; // pre-earned time
	float timeExtension = 2f; //time to extend by colleting powerup
	float timeDeduction = 4f; //time to reduce on collecting the snag
	float totalTimeElapsed = 0; 
	public float score = 0f;			// total score
	public bool isGameOver = true; 
	public GUISkin skin; 
	public GameObject countdown;
	public GameObject player;
	private PlayerControl control;
	public GameObject Gameover_canvas;
	//public GameObject facebook_canvas;
    public Canvas mainCanvas;
    public Text ScoreGui;
    public Text TimeLeftGui;
    public Text gameOverTitle;
    
	void Start () {
		Time.timeScale = 1;
		control = GameObject.Find("Main Camera").GetComponent<PlayerControl> ();
       
    }

	void FixedUpdate () {
		if (Application.loadedLevel == 1) {
			if (isGameOver) 
				return; // if the gameover variable is true then it will jump out of the update method

			totalTimeElapsed += Time.deltaTime;
			score = totalTimeElapsed * 10;
			timeRemaining -= Time.deltaTime;
        

			if (timeRemaining <= 0) {
				timeRemaining = 0;
				isGameOver = true;
				control.gameOverSound.Play ();
			}
		}
	}

	void OnGUI()
	{
		GUI.skin = skin;
		//check if game is not over, if so, display the score and the time left
		if (!isGameOver) {
           TimeLeftGui.text = ((int)timeRemaining).ToString();
           ScoreGui.text = ((int)score).ToString();

            Gameover_canvas.SetActive(false);
			//facebook_canvas.SetActive(false);

		}
		//if game over, display game over menu with score
		else {
			player.SetActive (false);
			countdown.GetComponent<CountDownScript> ().pauseButton.enabled = true;
			Time.timeScale = 0; //set the timescale to zero so as to stop the game world
			
			//display the final score
            gameOverTitle.text = "Your Score: " + (int)score;
            
            Gameover_canvas.SetActive(true);
			//facebook_canvas.SetActive(true);
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
