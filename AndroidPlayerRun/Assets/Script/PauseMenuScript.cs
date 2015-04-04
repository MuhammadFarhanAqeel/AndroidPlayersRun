using UnityEngine;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {


	public GUISkin mySkin;
	public string levelToLoad;
	public bool paused = false;
	public GameObject pause_button;
	GameControlScript control;
	public GameObject player;
	public GameObject paused_canvas;
	// Use this for initialization
	void Start () {
		control = GetComponent<GameControlScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (control.isGameOver == true) {
			this.GetComponent<PauseMenuScript>().enabled = false;
		}
		if (paused) {
			pause_button.SetActive(false);

			player.GetComponent<AudioSource>().enabled = false;
		} else {
			pause_button.SetActive(true);
			player.GetComponent<AudioSource>().enabled = true;
		}

		if (Input.GetKeyDown(KeyCode.Escape)){

			if (paused == true)
			{
				paused = false;
				pause_button.SetActive(true);
			} 
			else
			{
				paused = true; 
				pause_button.SetActive(false);

			}
		}
		if (paused) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}

		if (paused) {
			paused_canvas.SetActive (true);
		} else {
			paused_canvas.SetActive(false);
		}
	}

	public void onClickMouse(){
		if (paused) {
			paused = false;
		} 
		else{
			paused = true;
		}
}

public void Resume_Game(){
		paused = false;
			}
			
public void Restart_Game(){
		Application.LoadLevel (Application.loadedLevel);
	}
			
public void loadMain_menu(){
		 Application.LoadLevel("MainMenuScene");
	}
}