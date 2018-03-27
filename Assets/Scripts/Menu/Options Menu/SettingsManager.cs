using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class SettingsManager : MonoBehaviour {

	public Toggle FullscreenT;
    public Toggle HudT;
	public Dropdown ResD;
	public Dropdown QualityD;
	public Dropdown AAD;
	public Dropdown VSyncD;
	public Slider VolumeS;
	public Button ApplyB;
    public GameObject graphicso;
    public GameObject audioso;
    public GameObject gameso;
    bool graphics;
    bool audios;
    bool games;
    public GameObject Hud;

    public AudioSource music;
	public Resolution[] resolutions;
	public OptionsMenu GS;
    public Canvas optionsMenu;

    void OnEnable(){
	
		GS = new OptionsMenu();
        graphicsOn();

        optionsMenu = optionsMenu.GetComponent<Canvas>();

        FullscreenT.onValueChanged.AddListener (delegate {OnFullscreenToggle();});
        HudT.onValueChanged.AddListener(delegate { OnHudToggle(); });
        ResD.onValueChanged.AddListener (delegate {OnResChange();});
		QualityD.onValueChanged.AddListener (delegate {OnQualityChange();});
		AAD.onValueChanged.AddListener (delegate {OnAAChange();});
		VSyncD.onValueChanged.AddListener (delegate {OnVSyncChange();});
		VolumeS.onValueChanged.AddListener (delegate {OnVolumeChange();});
		ApplyB.onClick.AddListener (delegate {OnApply();});
		HudT.onValueChanged.AddListener (delegate {OnHudToggle();});
		Hud.active = HudT;

		resolutions = Screen.resolutions;
		foreach (Resolution resolution in resolutions) {
			ResD.options.Add (new Dropdown.OptionData (resolution.ToString()));
		}

		LoadSettings ();
	}
    public void graphicsOn()
    {
		games = false;
        audios = false;
        graphics = true;
        graphicso.SetActive(true);
		audioso.SetActive(false);
		gameso.SetActive(false);
    }
    public void audioOn()
    {
		games = false;
        graphics = false;
        audios = true;
        graphicso.SetActive(false);
		audioso.SetActive(true);
		gameso.SetActive(false);
    }
    public void gameplayOn()
    {
        games = true;
        graphics = false;
        audios = false;
        graphicso.SetActive(false);
        audioso.SetActive(false);
        gameso.SetActive(true);
    }
    public void OnFullscreenToggle(){

            GS.Fullscreen = Screen.fullScreen = FullscreenT.isOn;
            Debug.Log("Fullscreen?");
        
	}

	public void OnResChange(){

        if (graphics == true)
        {
            Screen.SetResolution(resolutions[ResD.value].width, resolutions[ResD.value].height, Screen.fullScreen);
            GS.Resolution = ResD.value;
            optionsMenu.enabled = false;
            optionsMenu.enabled = true;
            //SaveSettings();
            graphicsOn();
        }
		else if(audios == true)
         {
			Screen.SetResolution(resolutions[ResD.value].width, resolutions[ResD.value].height, Screen.fullScreen);
			GS.Resolution = ResD.value;
			optionsMenu.enabled = false;
			optionsMenu.enabled = true;
			//SaveSettings();
            audioOn();
        }
		else
		{
			Screen.SetResolution(resolutions[ResD.value].width, resolutions[ResD.value].height, Screen.fullScreen);
			GS.Resolution = ResD.value;
			optionsMenu.enabled = false;
			optionsMenu.enabled = true;
			//SaveSettings();
			gameplayOn();
		}


    }

	public void OnQualityChange(){

        if (graphics == true)
        {
            QualitySettings.masterTextureLimit = GS.Quality = QualityD.value;
            optionsMenu.enabled = false;
            optionsMenu.enabled = true;
            //SaveSettings();
            graphicsOn();
        }
		else if(audios == true)
        {
            QualitySettings.masterTextureLimit = GS.Quality = QualityD.value;
            optionsMenu.enabled = false;
            optionsMenu.enabled = true;
            //SaveSettings();
            audioOn();
		}
		else
		{
			QualitySettings.masterTextureLimit = GS.Quality = QualityD.value;
			GS.Resolution = ResD.value;
			optionsMenu.enabled = false;
			optionsMenu.enabled = true;
			//SaveSettings();
			gameplayOn();
		}


    }

	public void OnAAChange(){

        if (graphics == true)
        {
            QualitySettings.antiAliasing = GS.AA = (int)Mathf.Pow(2f, AAD.value);
            optionsMenu.enabled = false;
            optionsMenu.enabled = true;
            //SaveSettings();
            graphicsOn();
        }
		else if(audios == true)
        {
            QualitySettings.antiAliasing = GS.AA = (int)Mathf.Pow(2f, AAD.value);
            optionsMenu.enabled = false;
            optionsMenu.enabled = true;
            //SaveSettings();
            audioOn();
		}
		else
		{
			QualitySettings.antiAliasing = GS.AA = (int)Mathf.Pow(2f, AAD.value);
			GS.Resolution = ResD.value;
			optionsMenu.enabled = false;
			optionsMenu.enabled = true;
			//SaveSettings();
			gameplayOn();
		}


    }

	public void OnVSyncChange(){
        if (graphics == true)
        {
            //SaveSettings();
            QualitySettings.vSyncCount = GS.vSync = VSyncD.value;
            optionsMenu.enabled = false;
            optionsMenu.enabled = true;
            graphicsOn();
        }
		else if(audios == true)
        {
            //SaveSettings();
            QualitySettings.vSyncCount = GS.vSync = VSyncD.value;
            optionsMenu.enabled = false;
            optionsMenu.enabled = true;
            audioOn();
		}
		else
		{
			QualitySettings.vSyncCount = GS.vSync = VSyncD.value;
			GS.Resolution = ResD.value;
			optionsMenu.enabled = false;
			optionsMenu.enabled = true;
			//SaveSettings();
			gameplayOn();
		}
       

	}

	public void OnVolumeChange(){

        if (graphics == true)
        {
            music.volume = GS.Volume = VolumeS.value;
            //SaveSettings();
            graphicsOn();
        }
		else if(audios == true)
        {
            music.volume = GS.Volume = VolumeS.value;
            //SaveSettings();
            audioOn();
		}
		else
		{
			music.volume = GS.Volume = VolumeS.value;
			GS.Resolution = ResD.value;
			optionsMenu.enabled = false;
			optionsMenu.enabled = true;
			//SaveSettings();
			gameplayOn();
		}
        
	}

	public void OnApply(){
            SaveSettings();
    }
    public void OnHudToggle()
    {
		if(graphics == true){
            GS.Hud = Hud.active = HudT.isOn;
			graphicsOn();
		}
		else if(audios == true){
			GS.Hud = Hud.active = HudT.isOn;
			audioOn();
		}
		else{
			GS.Hud = Hud.active = HudT.isOn;
			gameplayOn();
		}
    }

    public void SaveSettings(){

		string jsonData = JsonUtility.ToJson (GS, true);
		File.WriteAllText (Application.persistentDataPath + "/gamesettings.json", jsonData);

	}

	public void LoadSettings(){
	
		GS = JsonUtility.FromJson<OptionsMenu> (File.ReadAllText (Application.persistentDataPath + "/gamesettings.json"));

		VolumeS.value = GS.Volume; 
		AAD.value = GS.AA;
		VSyncD.value = GS.vSync;
		QualityD.value = GS.Quality;
		ResD.value = GS.Resolution;
		FullscreenT.isOn = GS.Fullscreen;
        Screen.fullScreen = GS.Fullscreen;
        HudT.isOn = GS.Hud;
        Hud.active = GS.Hud;

		ResD.RefreshShownValue();
	}
}
/* */