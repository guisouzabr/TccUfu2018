using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class WorldSelection : MonoBehaviour {

	public Button[] worlds; //All the world buttons

	public int actualWorld; //Public only for test, u cant change in the final version

	private void Awake() {
		//Test the actualWorld key and get that.
		if(PlayerPrefs.HasKey("actualWorld")){
			actualWorld = PlayerPrefs.GetInt("actualWorld");
		}else{
			PlayerPrefs.SetInt("actualWorld",0);
		}
	}
	void Start () {
		//Enable the world that the player can go
		if(actualWorld != 0){
			for(int i=1;i<actualWorld+1;i++){
				worlds[i].enabled = true;
				worlds[i].gameObject.GetComponent<Image>().color = Color.green;
			}
		}
	}

	/// <summary>
	/// Load a world
	/// </summary>
	/// <param name="worldName">World to load</param>
	public void LoadWorld(string worldName){
		SceneManager.LoadScene(worldName);
	}
}
