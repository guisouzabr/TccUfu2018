using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barreira : MonoBehaviour {

	// Use this for initialization
	void Start () {
        /*if (Random.Range(1, 3) == 1)
        {
            getChildGameObject(this.gameObject, "1").SetActive(true);
            getChildGameObject(this.gameObject, "2").SetActive(false);
        }
        else
        {
            getChildGameObject(this.gameObject, "1").SetActive(false);
            getChildGameObject(this.gameObject, "2").SetActive(true);
        }*/
    }

	// Update is called once per frame
	void Update () {
		
	}

    /*static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }*/
}
