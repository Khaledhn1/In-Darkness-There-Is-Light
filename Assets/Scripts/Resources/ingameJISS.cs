using UnityEngine;
using System.Collections;

public class ingameJISS : MonoBehaviour {

    public Canvas FPSC;
    public GameObject Scripts;

	// Use this for initialization
	void Start () {

        FPSC.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetButtonDown("FPS"))
        {
            if (FPSC.enabled == false)
            {
                FPSC.enabled = true;
            }
            else
            {
                FPSC.enabled = false;
            }
        }

        if (Input.GetButtonDown("Send"))
        { 
            if (((MonoBehaviour)Scripts.GetComponent("InRoomChat")).enabled == true)
            {
                ((MonoBehaviour)Scripts.GetComponent("InRoomChat")).enabled = false;
            }
            else
            {
                ((MonoBehaviour)Scripts.GetComponent("InRoomChat")).enabled = true;
            }
        }
	}
}
