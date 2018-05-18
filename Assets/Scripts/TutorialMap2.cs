using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMap2 : MonoBehaviour {
    public GameObject player;
    public GameObject esteCanvas;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void voltaAcaoPlayer()
    {
        
        this.gameObject.SetActive(false);
    }
}
