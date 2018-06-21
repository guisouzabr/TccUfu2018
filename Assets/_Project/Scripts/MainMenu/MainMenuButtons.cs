using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {

	public void LoadAdditive(string sceneName){
		SceneManager.LoadScene(sceneName,LoadSceneMode.Additive);
	}

	public void LoadScene(string sceneName){
		SceneManager.LoadScene(sceneName);
	}
}
