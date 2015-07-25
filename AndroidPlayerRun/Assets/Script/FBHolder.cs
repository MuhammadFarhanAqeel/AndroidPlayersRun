using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FBHolder : MonoBehaviour
{

    
    public GameObject UIFBAvatar;
    public GameObject UIFBUserName;
    public GameObject UIFBLoginButton;
    private List<object> scoresList = null;

    private Dictionary<string, string> profile;
    public GameObject scoreEntryPanel;
    public GameObject scoreScrollList;
    public GameControlScript GameController;
    public GameObject FbScoreButton;
    public GameObject GameOverPanel;
    public GameObject FBScorePanel;

    void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    void Start()
    {

        if (FB.IsLoggedIn)
        {
            UIFBLoginButton.SetActive(false);
            FbScoreButton.SetActive(true);

            if (Application.loadedLevel == 1)
            {
                GameController = this.GetComponent<GameControlScript>();
                FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, DealWithProfilePictures);

                // get username code
                FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, DealWithUserName);

            }
        }
        else {
            FbScoreButton.SetActive(false);
            UIFBAvatar.SetActive(false);
            UIFBUserName.SetActive(false);
        }
    }

    private void SetInit()
    {
        Debug.Log("Fb init done!!");
        if (FB.IsLoggedIn)
        {
            Debug.Log("FB logged in!!");
            DealWithFBMenus(true);
        }
        else
        {
            DealWithFBMenus(false);
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void FBLogin()
    {
        FB.Login("email,publish_actions", AuthCallback);
        UIFBLoginButton.SetActive(false);
    }

    void AuthCallback(FBResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("fb login worked");
            DealWithFBMenus(true);
        }
        else
        {
            Debug.Log("login failed");
            DealWithFBMenus(false);
        }
    }

    void DealWithFBMenus(bool isLoggedIn)
    {
        if (FB.IsLoggedIn)
        {

            // get profile picture code
            if (Application.loadedLevel == 1)
            {
                FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, DealWithProfilePictures);

                // get username code
                FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, DealWithUserName);
            }
        }
        else
        {
            UIFBLoginButton.SetActive(true);
        }
    }


    void DealWithProfilePictures(FBResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("problem with getting profile picture");
            FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, DealWithProfilePictures);
            return;
        }
        Image UserAvatar = UIFBAvatar.GetComponent<Image>();
        UserAvatar.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2(0, 0));
    }

    void DealWithUserName(FBResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("problem with getting username");
            FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, DealWithUserName);
            return;
        }
        profile = Util.DeserializeJSONProfile(result.Text);

        Text userMsg = UIFBUserName.GetComponent<Text>();
        userMsg.text = profile["first_name"];
    }


    public void ShareWithFriends()
    {
        if (FB.IsLoggedIn)
        {
            FB.Feed(
            linkCaption: "I am playing this awesome Game",
            picture: "http://images.clipartpanda.com/runner-clipart-runner-hi.png",
            linkName: "Check out this game",
            link: "http://apps.facebook.com/" + FB.AppId + "/?challange_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
            );
        }
    }

    public void InviteFriends()
    {
        if (FB.IsLoggedIn)
        {
            FB.AppRequest(
                message: "this game is awesome!! join me, now!!!",
                title: "Invite your friends to join you"
                );
        }
    }

    public void QueryScores()
    {
        FB.API("/app/scores?fields=score,user.limit(30)", Facebook.HttpMethod.GET, ScoresCallBack);
        GameOverPanel.SetActive(false);
        FBScorePanel.SetActive(true);
    }

    private void ScoresCallBack(FBResult result)
    {
        scoresList = Util.DeserializeScores(result.Text);

        foreach (Transform child in scoreScrollList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (object score in scoresList)
        {
            var entry = (Dictionary<string, object>)score;
            var user = (Dictionary<string, object>)entry["user"];

            GameObject scorePanel;
            scorePanel = Instantiate(scoreEntryPanel) as GameObject;
            scorePanel.transform.parent = scoreScrollList.transform;

            Transform thisScoreName = scorePanel.transform.Find("FriendName");
            Transform thisScoreScore = scorePanel.transform.Find("FriendScore");

            Text scoreName = thisScoreName.GetComponent<Text>();
            Text scoreScore = thisScoreScore.GetComponent<Text>();

            scoreName.text = user["name"].ToString();
            scoreScore.text = entry["score"].ToString();

            Transform theUserAvatar = scorePanel.transform.Find("FriendAvatar");
            Image UserAvatar = theUserAvatar.GetComponent<Image>();

            FB.API(Util.GetPictureURL(user["id"].ToString(), 128, 128), Facebook.HttpMethod.GET, delegate(FBResult pictureResult)
            {
                if (pictureResult.Error != null) // if there is an error!
                {
                    Debug.Log(pictureResult.Error);
                }
                else
                {
                    UserAvatar.sprite = Sprite.Create(pictureResult.Texture, new Rect(0, 0, 128, 128), new Vector2(0, 0));
                }

            });

        }
    }

    public void SetScore()
    {
        var scoreData = new Dictionary<string, string>();
        scoreData["score"] = (GameController.GetComponent<GameControlScript>().score).ToString();
        FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult result)
        {
            Debug.Log("score submit result: " + result.Text);
        }, scoreData);
    }

    public void closeClick() 
    {
        GameOverPanel.SetActive(true);
        FBScorePanel.SetActive(false);
    }

}
