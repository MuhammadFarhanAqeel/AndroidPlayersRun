using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FBHolder : MonoBehaviour {
	
	public GameObject UIFBIsLoggedIn;
	public GameObject UIFBNotLoggedIn;
	public GameObject UIFBAvatar;
	public GameObject UIFBUserName;
	public Text ScoresDebug;
	private List<object> scoresList = null;

	private Dictionary<string,string> profile;
	
	void Awake()
	{
		FB.Init (SetInit, OnHideUnity);
	}
	
	private void SetInit()
	{
		Debug.Log("Fb init done!!");
		if (FB.IsLoggedIn) {
			Debug.Log ("FB logged in!!");
			DealWithFBMenus (true);
		} else {
			DealWithFBMenus (false);
		}
	}
	
	private void OnHideUnity(bool isGameShown) 
	{
		if (!isGameShown) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 0;		
		}
	}
	
	public void FBLogin()
	{
		FB.Login ("email", AuthCallback);
	}
	
	void AuthCallback(FBResult result)
	{
		if (FB.IsLoggedIn) {
			Debug.Log ("fb login worked");	
			DealWithFBMenus (true);
		} else {
			Debug.Log ("login failed");		
			DealWithFBMenus (false);
		}
	}
	
	void DealWithFBMenus(bool isLoggedIn)
	{
		if (isLoggedIn) {
			UIFBIsLoggedIn.SetActive (true);
			UIFBNotLoggedIn.SetActive (false);
			
			// get profile picture code
			
			FB.API(Util.GetPictureURL("me",128,128),Facebook.HttpMethod.GET, DealWithProfilePictures);
			
			// get username code
			FB.API ("/me?fields=id,first_name",Facebook.HttpMethod.GET, DealWithUserName);
			
		} else {
			UIFBIsLoggedIn.SetActive (false);
			UIFBNotLoggedIn.SetActive (true);
		}
	}
	
	
	void DealWithProfilePictures(FBResult result)
	{
		if (result.Error != null) {
			Debug.Log("problem with getting profile picture");
			FB.API(Util.GetPictureURL("me",128,128),Facebook.HttpMethod.GET, DealWithProfilePictures);
			return;
		}
		Image UserAvatar = UIFBAvatar.GetComponent<Image> ();
		UserAvatar.sprite = Sprite.Create (result.Texture, new Rect (0, 0, 128, 128), new Vector2 (0, 0)); 
	}
	
	
	void DealWithUserName(FBResult result)
	{
		if (result.Error != null) {
			Debug.Log("problem with getting username");
			FB.API ("/me?fields=id,first_name",Facebook.HttpMethod.GET, DealWithUserName);
			return;
		}
		profile = Util.DeserializeJSONProfile (result.Text);
		
		Text userMsg = UIFBUserName.GetComponent<Text> ();
		userMsg.text = "Hello, " +profile["first_name"];
	}
	
	
	public void ShareWithFriends()
	{
		FB.Feed (
			linkCaption: "I am playing this awesome Game",
			picture: "http://images.clipartpanda.com/runner-clipart-runner-hi.png",
			linkName: "Check out this game",
			link: "http://apps.facebook.com/" +FB.AppId +"/?challange_brag=" +(FB.IsLoggedIn?FB.UserId: "guest")
			);
	}
	
	public void InviteFriends()
	{
		
		FB.AppRequest (
			message: "this game is awesome!! join me, now!!!",
			title: "Invite your friends to join you"
			);
	}

	public void QueryScores(){
		FB.API ("/app/scores?fields=score,user.limit(30)",Facebook.HttpMethod.GET,ScoresCallBack);
	}

	private void ScoresCallBack(FBResult result){
		ScoresDebug.text = "";

		scoresList = Util.DeserializeScores (result.Text);

		foreach(object score in scoresList){
			var entry = (Dictionary<string,object>) score;
			var user = (Dictionary<string,object>) entry["user"];

			ScoresDebug.text = ScoresDebug.text + "UserName: " +user["name"]+" - "+entry["score"]+", ";
		}

	}

	public void SetScore(){

	}
}
