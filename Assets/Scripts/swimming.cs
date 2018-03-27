using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swimming : MonoBehaviour {
	CharacterController cc;
	// Use this for initialization
	void Start () {
		RenderSettings.fog = false;
		RenderSettings.fogColor = new Color(0.2f,0.4f,0.8f,0.5f);
		RenderSettings.fogDensity = 0.04f;

		cc = gameObject.GetComponent<CharacterController>();
	}

	bool isUnderWater(){
		return gameObject.transform.position.y < 30.6f;
	}
	// Update is called once per frame
	void Update () {
		RenderSettings.fog = isUnderWater();
		if (isUnderWater()) {
			Physics.gravity = new Vector3(2f,3.2f,2f);
		} else {
			Physics.gravity = new Vector3(0,-9.81f,0);
		}
	}
}
