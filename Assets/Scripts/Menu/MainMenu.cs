using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Canvas quitMenu;
	public Canvas optionsMenu;
	public Button playButton;
	public Button optionsButton;
    public Button extrasButton;
	public Button quitButton;
	public Button ApplyB;
    //public Canvas MainM;

	void Start() {

		quitMenu = quitMenu.GetComponent<Canvas> ();
		optionsMenu = optionsMenu.GetComponent<Canvas> ();
		playButton = playButton.GetComponent<Button> ();
		quitButton = quitButton.GetComponent<Button> ();
		optionsButton = optionsButton.GetComponent<Button> ();
        extrasButton = extrasButton.GetComponent<Button>();
        ApplyB = ApplyB.GetComponent<Button> ();
		quitMenu.enabled = false;
		optionsMenu.enabled = false;

	}

	public void exitPress() {

		quitMenu.enabled = true;
		playButton.enabled = false;
		quitButton.enabled = false;
		optionsButton.enabled = false;
        extrasButton.enabled = false;
		optionsMenu.enabled = false;

	}

	public void optionsPress() {

		quitMenu.enabled = false;
		playButton.enabled = false;
		quitButton.enabled = false;
		optionsButton.enabled = false;
		optionsMenu.enabled = true;
        extrasButton.enabled = false;
        //MainM.enabled = false;

    }

	public void noPress() {
		
		quitMenu.enabled = false;
		playButton.enabled = true;
		quitButton.enabled = true;
		optionsButton.enabled = true;
		optionsMenu.enabled = false;
        extrasButton.enabled = true;
        //MainM.enabled = true;

    }

	public void startLevel(int num) {

       if(num < 0 || num >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("Can't Load Scene" + num + ",SceneManager Only Has" + SceneManager.sceneCountInBuildSettings + "Scenes In Build Settings");
            return;
        }
        LoadingScreenManager.LoadScene(num);

    }

	public void quitGame() {
	
        if (Application.isEditor)
        {
            Debug.LogError("you cant quit in the editor you idiot");
        }else
        {
            Application.Quit();
        }

	}
}
