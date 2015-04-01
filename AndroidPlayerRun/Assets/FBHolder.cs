using UnityEngine;
using System.Collections;

public class FBHolder : MonoBehaviour {

	void Awake(){
		FB.Init (SetInit, onHideUnity);
	}

	private void SetInit(){
		Debug.Log ("Fb init done");
		if (FB.IsLoggedIn) {
			Debug.Log ("Fb logged in");
		} else {
			FBLogin();
		}
	}


	private void onHideUnity(bool isGameShown){
		if (!isGameShown) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}

	void FBLogin(){
		FB.Login ("user_about_me, user_birthday", AuthCallBack);
	}

	void AuthCallBack(FBResult result){
		if (FB.IsLoggedIn) {
			Debug.Log ("FB login worked");
		} else {
			Debug.Log("FB Login failure");
		}
	}
}