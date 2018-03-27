#pragma strict

//SimChat
private var sc:SimChat;
//names
private var names:String[] = ["me","myself","you","nobody","somebody","hotdog","cupcake","Mr.Roboto","Phil","Sour Sally"];
//current chat name
public var chatName:String = "";
//show the chat
public var show:boolean = true;
//GUI variables
private var sp:Vector2 = Vector2.zero;
private var chatRect:Rect = Rect(Screen.width*0.6,Screen.height*0.6,Screen.width*0.4,Screen.height*0.4);
private var c:Color;
//show new chat message
private var rt:float = -3;
//pending messages
private var sendingMessages = new Array();

function Start(){
	//select chat name if one was not already added in the editor
	if(chatName=="")
		chatName = names[Random.Range(0,names.length)];
	//initialize the SimChat with the chat room "default" and the name chatName
	sc = SimChat("default",gameObject.GetComponent("MonoBehaviour"),chatName);
	//tell the SimChat object to continuously check for messages.
	sc.continueCheckMessages();
	//set the functions to call when a new message is received
	sc.setReceiveFunction(receiveMessage);
}

function receiveMessage(){
	//check if the last message is from me
	if(sc.allMessages[sc.allMessages.Count-1].sender != chatName)
		rt = Time.time;
	sp.y = Mathf.Infinity; //set the scroll
	sendingMessages = new Array(); //reset the pending message array
}

function OnGUI (){
	//display new message
	if(Time.time - rt < 3 ){
		GUI.skin.label.fontSize = 17;
		GUILayout.Label("New Message: "+sc.allMessages[sc.allMessages.Count-1].sender+": "+sc.allMessages[sc.allMessages.Count-1].message);
	}

	//show chat area
	if(show){
		GUI.skin.textField.fontSize = GUI.skin.button.fontSize = GUI.skin.label.fontSize = 17;
		GUI.skin.label.wordWrap = false;
		GUILayout.BeginArea(chatRect);
		GUILayout.BeginVertical("box");
		
		GUILayout.BeginVertical("box");
			sp = GUILayout.BeginScrollView(sp);
			GUILayout.FlexibleSpace();
			c = GUI.contentColor;
			//loop through each of the messages contained in allMessages
			for(var sm:int = 0;sm<sc.allMessages.Count;sm++){
				GUILayout.BeginHorizontal();
				//check if the sender had the same name as me, and change the color
				if(sc.allMessages[sm].sender == chatName){
					GUI.contentColor = Color.red;
					GUILayout.FlexibleSpace();
					GUILayout.Label(sc.allMessages[sm].message);
				}else{
					GUI.contentColor = Color.green;
					GUILayout.Label(sc.allMessages[sm].sender+": "+sc.allMessages[sm].message);
					GUILayout.FlexibleSpace();
				}
				
				GUILayout.EndHorizontal();
			}
			//display the pending messages
			GUI.contentColor = Color.red;
			for(var snm:int = 0;snm<sendingMessages.length;snm++){
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label(sendingMessages[snm] as String);
				GUILayout.EndHorizontal();
			}
			GUI.contentColor = c;
			GUILayout.EndScrollView();
		GUILayout.EndVertical();
		
		GUILayout.BeginHorizontal();
			//send a new message
			sc.message = GUILayout.TextField(sc.message);
			if(GUILayout.Button("Send") || (Event.current.isKey && Event.current.keyCode == KeyCode.Return) ){
				sc.sendMessage();
				sendingMessages.Add(sc.message);
				sc.message = "";
			}
		GUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}