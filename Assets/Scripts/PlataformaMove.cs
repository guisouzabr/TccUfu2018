using UnityEngine;
using System.Collections;

public class PlataformaMove : MonoBehaviour {
    public Rigidbody2D plataformaRB;
    public Collider2D boxPlataforma;
    public Transform fim;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (plataformaRB.position.x >= fim.position.x) {
            plataformaRB.isKinematic = false;
            boxPlataforma.isTrigger = true;          
        }
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {

        }
    }
}
