using UnityEngine;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {


	public GUISkin mySkin;
	public string levelToLoad;
	public bool paused = false;
	public GameObject button;
	GameControlScript control;
	public GameObject player;

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
			button.SetActive(false);
			player.GetComponent<AudioSource>().enabled = false;
		} else {
			button.SetActive(true);
			player.GetComponent<AudioSource>().enabled = true;
		}

		if (Input.GetKeyDown(KeyCode.Escape)){

			if (paused == true)
			{
				paused = false;
				button.SetActive(true);
			
			} 
			else
			{
				paused = true; 
				button.SetActive(false);


			}
		}
		if (paused) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
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


	private void OnGUI()
	{
		GUI.skin=mySkin;   //use the custom GUISkin
		
		if (paused){    
			
			GUI.Box(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2), "PAUSED");
			//GUI.Label(new Rect(Screen.width/4+10, Screen.height/4+Screen.height/10+10, Screen.width/2-20, Screen.height/10), "YOUR SCORE: "+ ((int)score));
			
			if (GUI.Button(new Rect(Screen.width/4+10, Screen.height/4+Screen.height/10+10, Screen.width/2-20, Screen.height/10), "RESUME")){
				paused = false;
			}
			
			if (GUI.Button(new Rect(Screen.width/4+10, Screen.height/4+2*Screen.height/10+10, Screen.width/2-20, Screen.height/10), "RESTART")){
				Application.LoadLevel(Application.loadedLevel);
			}
			
			if (GUI.Button(new Rect(Screen.width/4+10, Screen.height/4+3*Screen.height/10+10, Screen.width/2-20, Screen.height/10), "MAIN MENU")){
				Application.LoadLevel("MainMenuScene");
			} 
		}
	}
}