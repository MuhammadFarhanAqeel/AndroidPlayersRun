using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	public GUISkin mySkin;
	public string gameLevel;
	public string optionsLevel;


	public void Play_Game(){
		Application.LoadLevel ("Game");
	}
	public void Credit_scene(){
		Application.LoadLevel ("Credits");
	}

	public void Exit_game(){
		Application.Quit ();
	}

}
