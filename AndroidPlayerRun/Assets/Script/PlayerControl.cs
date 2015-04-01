using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

	public List<Transform> Platform;
	private float speed= 0.7f;
	public CharacterController player;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 pMov;
	public float jumpSpeed = 1.0f;
	private float gravity = 10f;
	float playerZposition;
	private bool isGrounded;
	private float pSpeed = 12f;
	public GameObject ScoreAndSpawnrate; // external object to manage dificulty
	GameControlScript control;
	//touch inputs

	private Touch initialTouch = new Touch ();	
	private float distance = 0;
	private bool hasSwiped;

	 CountDownScript count;
	 PauseMenuScript pause;

	public AudioSource powerUpCollectSound;
	public AudioSource jumpSound;
	public AudioSource snagSound;
	public AudioSource gameOverSound;




	Bounds _bounds;
	Transform lastPlatform;

	List<Transform> platforms = new List<Transform>();


	void Start () {


		count = GameObject.Find("GameController").GetComponent<CountDownScript> ();
		pause = GameObject.Find("GameController").GetComponent<PauseMenuScript> ();
		control = GameObject.Find ("GameController").GetComponent<GameControlScript> ();
		for(int i = 0;i < 20; i++){

			Transform platform = (Transform)Instantiate(Platform[Random.Range(0,Platform.Count)]);

			if(i > 0){
				Bounds prevbounds = platforms[i - 1].FindChild("Surface").GetComponent<MeshRenderer>().bounds;
				Bounds bounds = platform.FindChild("Surface").GetComponent<MeshRenderer>().bounds;

				platform.transform.position = platforms[i - 1].position + new Vector3(0,0,bounds.extents.z + prevbounds.extents.z); 

				_bounds = bounds;
			}else{
				platform.transform.position = Vector3.zero;
			}
			platforms.Add(platform);
		}
	}


	void FixedUpdate(){
		playerZposition = player.transform.position.z;
		player.transform.position += new Vector3 (0, 0, speed);
		//pMov = new Vector3 (Input.GetAxis ("Horizontal") * pSpeed , 0, 0);
		pMov = new Vector3 (Input.acceleration.x *pSpeed, 0, 0);

		if (player.isGrounded) {
			player.SimpleMove (pMov);	
			player.GetComponent<Animation> ().Play ("run");

			if (pause.paused == false){
				gameObject.GetComponent<AudioSource>().enabled = true;
			}
			else
			{
				gameObject.GetComponent<AudioSource>().enabled = false;
			}
			
		}
		
		if (Input.GetButton ("Jump") && player.isGrounded) {
		PlayerJump ();
		}
		
		player.Move (moveDirection);
		moveDirection.y -= gravity * .25f * Time.deltaTime;
		
		if (player.isGrounded)
			isGrounded = true;


		for (int i = 0; i < platforms.Count; i++) {
			if (platforms [i].position.z < transform.position.z - _bounds.extents.z) {
				Destroy (platforms [i].gameObject);
				platforms.RemoveAt (i);
				Transform platform = (Transform)Instantiate (Platform [Random.Range (0, Platform.Count)]);
				Bounds prevbounds = platforms [platforms.Count - 1].FindChild ("Surface").GetComponent<MeshRenderer> ().bounds;
				Bounds bounds = platform.FindChild ("Surface").GetComponent<MeshRenderer> ().bounds;
				platform.transform.position = platforms [platforms.Count - 1].position + new Vector3 (0, 0, bounds.extents.z + prevbounds.extents.z); 
				_bounds = bounds;
				platforms.Add (platform);
				break;
				//platforms[i].position = lastPlatform.position + new Vector3(0,0,_bounds.extents.z * 2);
				//lastPlatform = platforms[i];
			}
		}

		transform.position += new Vector3 (0, 0, speed);

		if (ScoreAndSpawnrate.GetComponent<GameControlScript> ().score > 150) {
			ScoreAndSpawnrate.GetComponent<SpawnScript> ().spawnCycle = 0.3f;
			speed = 0.7f;
		}

		if (ScoreAndSpawnrate.GetComponent<GameControlScript> ().score > 300) {
			ScoreAndSpawnrate.GetComponent<SpawnScript> ().spawnCycle = 0.25f;
			speed = 0.7f;
		} 

		if (ScoreAndSpawnrate.GetComponent<GameControlScript> ().score > 450) {
			ScoreAndSpawnrate.GetComponent<SpawnScript> ().spawnCycle = 0.2f;
			speed = 0.8f;
		} 

		if (ScoreAndSpawnrate.GetComponent<GameControlScript> ().score > 600) {
			ScoreAndSpawnrate.GetComponent<SpawnScript> ().spawnCycle = 0.15f;
			speed = 0.9f;
		} 

		if (ScoreAndSpawnrate.GetComponent<GameControlScript> ().score > 900) {
			ScoreAndSpawnrate.GetComponent<SpawnScript> ().spawnCycle = 0.1f;
			speed = 1.0f;
		} 

		if (ScoreAndSpawnrate.GetComponent<GameControlScript> ().score > 1100) {
			ScoreAndSpawnrate.GetComponent<SpawnScript> ().spawnCycle = 0.05f;
			speed = 1.1f;
		} 
		foreach (Touch t in Input.touches) {
			if (t.phase == TouchPhase.Began) {
				initialTouch = t;
			}
			else if(t.phase==TouchPhase.Moved &&!hasSwiped)
			{
				float deltaX = initialTouch.position.x - t.position.x;
				float deltaY = initialTouch.position.y - t.position.y;
				distance = Mathf.Sqrt((deltaX*deltaX)+(deltaY*deltaY));
				bool swipeSideways = Mathf.Abs(deltaX) > Mathf.Abs(deltaY);

				if(distance > 10f)
				{
					if(!swipeSideways && deltaY > 0) // swiped down
					{

					}
					else if(!swipeSideways && deltaY <= 0 && player.isGrounded) // swiped up
					{
						PlayerJump();
					}
					hasSwiped = true;
				}
			}
			else if(t.phase == TouchPhase.Ended)
			{
				initialTouch = new Touch();
				hasSwiped = false;
			}
		}
		if (control.isGameOver) {
			gameObject.GetComponent<AudioSource>().enabled = false;
		}
	}

	void PlayerJump(){
		player.GetComponent<AudioSource> ().enabled = false;
		isGrounded = false;
		player.GetComponent<Animation> ().Stop ("run");
		player.GetComponent<Animation> ().Play ("jump_pose");
		jumpSound.Play ();
		gameObject.GetComponent<AudioSource> ().enabled = false;
		moveDirection.y = jumpSpeed / 3f;
	}
}