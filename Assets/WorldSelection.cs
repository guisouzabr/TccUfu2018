using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class WorldSelection : MonoBehaviour {

	public Button[] worlds;

	int actualWorld;

	private void Awake() {
		if(PlayerPrefs.HasKey("actualWorld")){
			actualWorld = PlayerPrefs.GetInt("actualWorld");
		}else{
			PlayerPrefs.SetInt("actualWorld",0);
		}
		PlayerPrefs.SetInt("actualWorld",1);
		actualWorld = PlayerPrefs.GetInt("actualWorld");
	}

	// Use this for initialization
	void Start () {
		if(actualWorld != 0){
			for(int i=1;i<actualWorld+1;i++){
				worlds[i].enabled = true;
				worlds[i].gameObject.GetComponent<Image>().color = Color.green;
			}
		}
	}


	public void LoadWorld(string worldName){
		SceneManager.LoadScene(worldName);
	}
}
