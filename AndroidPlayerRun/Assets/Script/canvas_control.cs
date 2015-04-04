using UnityEngine;
using System.Collections;

public class canvas_control : MonoBehaviour {

	public Canvas facebook_canvas;
	private GameControlScript control_script;
	public GameObject Gameover_canvas;
	

	void Start () {
		control_script = GameObject.Find ("GameController").GetComponent<GameControlScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (control_script.isGameOver) {
			facebook_canvas.enabled = true;
			Gameover_canvas.SetActive(true);
		}else{
			facebook_canvas.enabled = false;
			Gameover_canvas.SetActive(false);
		}
	}
}

