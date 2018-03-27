using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public bool spawn = false;
	private int maxPlayer = 1;
	public GameObject SpawnSpot;
	private Room[] game;
	private string roomName = "IDTIL Game Room";
	bool connecting = false;
	List<string> chatMessages;
	public GameObject MenuCamera;
	public GameObject Hud;
	int maxChatMessages = 5;
	private string maxPlayerString = "2";
	public string Version = "Version 1";
	private Vector3 up;
	private Vector2 scrollPosition;

	void Start (){
		PhotonNetwork.player.name = PlayerPrefs.GetString("Username", "My Player name");
		chatMessages = new List<string>();
	}

	void OnDestroy(){
		PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
	}

	public void AddChatMessage(string m){
		GetComponent<PhotonView>().RPC("AddChatMessage_RPC", PhotonTargets.AllBuffered, m);
	}

	[PunRPC]
	void AddChatMessage_RPC(string m){
		while(chatMessages.Count >= maxChatMessages){
			chatMessages.RemoveAt(0);
		}
		chatMessages.Add(m);
	}

	void Connect(){
		PhotonNetwork.ConnectUsingSettings(Version);
	}

	void OnGUI(){


		GUI.color = Color.white;
		if(PhotonNetwork.connected == false && connecting == false ) {
			GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Username: ");
			PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
			GUILayout.EndHorizontal();

			if( GUILayout.Button("Play") ) {
				connecting = true;
				Connect ();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		if(PhotonNetwork.connected == true && connecting == false) {
			GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();

			foreach(string msg in chatMessages) {
				GUILayout.Label(msg);
			}

			GUILayout.EndVertical();
			GUILayout.EndArea();

		}

		if (PhotonNetwork.insideLobby == true   && connecting==true) {

			GUI.Box(new Rect(Screen.width/3f,0 , 400, 450),"");
			GUI.color = Color.white;
			GUILayout.BeginArea (new Rect(Screen.width/3f , 0 , 400, 400));
			GUI.color = Color.cyan;
			GUILayout.Box ("Lobby");
			GUI.color = Color.white;

			GUILayout.Label("Session Name:");
			roomName = GUILayout.TextField(roomName);
			GUILayout.Label ("Max amount of players 1 - 20:");
			maxPlayerString = GUILayout.TextField (maxPlayerString,2);
			if (maxPlayerString != "") {

				maxPlayer = int.Parse (maxPlayerString);

				if (maxPlayer > 20) maxPlayer = 20;
				if (maxPlayer == 0) maxPlayer = 1;
			}
			else
			{
				maxPlayer = 1;
			}

			if ( GUILayout.Button ("Create Room ") ) {
				if (roomName != "" && maxPlayer > 0) {
					PhotonNetwork.CreateRoom(roomName);
				}
			}

			GUILayout.Space (20);
			GUI.color = Color.yellow;
			GUILayout.Box ("Open Rooms");
			GUI.color = Color.red;
			GUILayout.Space (20);

			scrollPosition = GUILayout.BeginScrollView(scrollPosition, false,true,GUILayout.Width(400), GUILayout.Height(200));


			foreach (RoomInfo game in PhotonNetwork.GetRoomList ())
			{
				GUI.color = Color.green;
				GUILayout.Box (game.name + " " + game.playerCount + "/" + game.maxPlayers + " " + game.visible);
				if ( GUILayout.Button ("Join Session") ) {
					PhotonNetwork.JoinRoom(game.name);
				}
			}
			GUILayout.EndScrollView ();
			GUILayout.EndArea ();
		}
	}

	void OnJoinedLobby(){
		Debug.Log("OnJoinedLobby");
	}

	void OnPhotonRandomJoinFailed(){
		Debug.Log("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom( null );
	}

	void OnJoinedRoom(){
		Debug.Log("OnJoinedRoom");
		connecting = false;
		spawn = true;
	}

	void Update(){
		SettingsManager gs = new SettingsManager ();
		if(spawn == true){
			SpawnMyPlayer();
		}
	}
    void SpawnMyPlayer()
	{
		spawn = false;
		if (SpawnSpot == null) {
			Debug.LogError ("WTF?!?!?");
			return;
		}
		SettingsManager gs = new SettingsManager ();
		/*SpawnSpot mySpawnSpot = spawnSpots [Random.Range (0, spawnSpots.Length)];*/
		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate ("Player1", SpawnSpot.transform.position, SpawnSpot.transform.rotation, 0);
		GameObject crosshairs = (GameObject)PhotonNetwork.Instantiate ("crosshairs", SpawnSpot.transform.position, SpawnSpot.transform.rotation, 0);
		MenuCamera.SetActive (false);

		myPlayerGO.GetComponentInChildren<Camera> ().enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent ("PlayerMovement")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent ("PlayerShooting")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent ("RandomStuff")).enabled = true;
		Hud.active = gs.HudT;

        
	}
}
