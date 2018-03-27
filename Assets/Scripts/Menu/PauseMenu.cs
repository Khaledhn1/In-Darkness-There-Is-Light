using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public GameObject pausePanel;
	//public GameObject gun;
	public static bool isPaused;
	public Button optionsButton;
	public Canvas optionsMenu;
    //public static bool exit = false;
    public Canvas AYSTE;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button yes;
    public Button no;



    void Start () {
		isPaused = false;
        //exit = false;
        optionsMenu.enabled = false;
        AYSTE.enabled = false;
        button1.enabled = true;
        button2.enabled = true;
        button3.enabled = true;
        yes.enabled = false;
        no.enabled = false;
    }

	// Update is called once per frame
	void Update () {
		if (isPaused) {
			pauseGame (true);
		} else {
			pauseGame (false);
		}

		if (Input.GetButtonDown ("Cancel")) {
			switchPause ();
            if (optionsMenu.isActiveAndEnabled)
            {
                optionsMenu.enabled = false;
            }
		}
	}
	void pauseGame(bool state){
		if (state) {
			//Time.timeScale = 0.0f;

			//gun.GetComponent<AudioSource> ().enabled = false;
		} else {
			//Time.timeScale = 1.0f;
			pausePanel.SetActive (false);
			//gun.GetComponent<AudioSource> ().enabled = true;
		}
		pausePanel.SetActive (state);
	}
	public void switchPause(){
		if (isPaused) {
			isPaused = false;
		} else {
			isPaused = true;
		}
	}
    public void exitToMainMenu(int num) {
        /*if (num < 0 || num >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("Can't Load Scene" + num + ",SceneManager Only Has" + SceneManager.sceneCountInBuildSettings + "Scenes In Build Settings");
            return;
        }
        LoadingScreenManager.LoadScene(num);*/
        AYSTE.enabled = true;
        button1.enabled = false;
        button2.enabled = false;
        button3.enabled = false;
        yes.enabled = true;
        no.enabled = true;
    }

    public void exit1()
    {
            SceneManager.LoadScene(0);
    }
    public void exit2()
    {
        AYSTE.enabled = false;
        button1.enabled = true;
        button2.enabled = true;
        button3.enabled = true;
        yes.enabled = false;
        no.enabled = false;
    }

	public void optionsPress() {

		//gun.GetComponent<AudioSource> ().enabled = false;
		optionsButton.enabled = false;
		optionsMenu.enabled = true;

	}
	public void noPress() {

		//gun.GetComponent<AudioSource> ().enabled = false;
		optionsButton.enabled = true;
		optionsMenu.enabled = false;

	}

}