using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testExtendClass : MonoBehaviour {
	SimpleChat sc;
	
	void Start(){
		string[] names = new string[]{"me","myself","you","nobody","somebody","hotdog","cupcake","Mr.Roboto","Phil","Sour Sally"};
		
		sc = new SimpleChat("default",gameObject.GetComponent<MonoBehaviour>(),names[Random.Range(0,names.Length)]);
	}
	
	void OnGUI(){
		sc.draw();
	}
}

public class SimpleChat:SimChat{
	//gui variables
	public bool show = true;
	public Rect chatRect = new Rect(Screen.width*0.6f,Screen.height*0.6f,Screen.width*0.4f,Screen.height*0.4f);
	public float messageTime = 3f;
	protected float rt = 0f;
	public int textSize = 17;
	public Color myColor = Color.red,theirColor = Color.green;
	protected Vector2 sp = Vector2.zero;
	protected Color c;
	protected List<string> pending = new List<string>();
	
	public SimpleChat(string identifier,MonoBehaviour currentMonoBehaviour,string senderName):base(identifier,currentMonoBehaviour,senderName){
		continueCheckMessages();
		rt = -messageTime;
		setReceiveFunction(receive);
	}
	
	protected void receive(SimpleMessage[] sma){
		//check if the last message is from me
		if(allMessages[allMessages.Count-1].sender != senderName)
			rt = Time.time;
		sp.y = Mathf.Infinity; //set the scroll
		pending = new List<string>(); //reset the pending message array
	}
	
	public void draw(){
		//display new message
		if(Time.time - rt < messageTime ){
			GUI.skin.label.fontSize = textSize;
			GUILayout.Label("New Message: "+allMessages[allMessages.Count-1].sender+": "+allMessages[allMessages.Count-1].message);
		}
		
		//show chat area
		if(show){
			GUI.skin.textField.fontSize = GUI.skin.button.fontSize = GUI.skin.label.fontSize = textSize;
			GUI.skin.label.wordWrap = false;
			GUILayout.BeginArea(chatRect);
			GUILayout.BeginVertical("box");
			
				GUILayout.BeginVertical("box");
					sp = GUILayout.BeginScrollView(sp);
					GUILayout.FlexibleSpace();
					c = GUI.contentColor;
					//loop through each of the messages contained in allMessages
					foreach(SimpleMessage  sm in allMessages){
						GUILayout.BeginHorizontal();
							if(sm.sender==senderName){
								GUI.contentColor = myColor;
								GUILayout.FlexibleSpace();
								GUILayout.Label(sm.message);
							}else{
								GUI.contentColor = theirColor;
								GUILayout.Label(sm.sender+": "+sm.message);
								GUILayout.FlexibleSpace();
							}
						GUILayout.EndHorizontal();
					}
					//display the pending messages
					GUI.contentColor = myColor;
					foreach(string p in pending){
						GUILayout.BeginHorizontal();
							GUILayout.FlexibleSpace();
							GUILayout.Label(p);
						GUILayout.EndHorizontal();
					}
					GUI.contentColor = c;
					GUILayout.EndScrollView();
				GUILayout.EndVertical();
				
				GUILayout.BeginHorizontal();
					//send a new message
					message = GUILayout.TextField(message);
					if(GUILayout.Button("Send") || (Event.current.isKey && Event.current.keyCode == KeyCode.Return) ){
						sendMessage();
						pending.Add(message);
						message = "";
					}
				GUILayout.EndHorizontal();
			
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}