using UnityEngine;
using System.Collections;

public class FBHolder : MonoBehaviour {

	public void Awake(){
		FB.Init (SetInit, onHideUnity);

	}


	public void FBLogin(){

		FB.Login ("user_about_me, user_birthday", AuthCallBack);
	}

	private void SetInit(){
		Debug.Log ("Fb init done");
		if (FB.IsLoggedIn) {
			Debug.Log ("Fb logged in");
		} else {
			//FBLogin();
		}
	}


	private void onHideUnity(bool isGameShown){
		if (!isGameShown) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}



	void AuthCallBack(FBResult result){
		if (FB.IsLoggedIn) {
			Debug.Log ("FB login worked");
		} else {
			Debug.Log("FB Login failure");
		}
	}
}