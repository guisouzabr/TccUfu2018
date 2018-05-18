using UnityEngine;
using System.Collections;

public class PlataformaQueda : MonoBehaviour {
    public Rigidbody2D plataformaRB;
    public Collider2D boxPlataforma;
    private float time;
    private bool toque;

    // Use this for initialization
    void Start() {
        time = 0;
        toque = false;
    }

    // Update is called once per frame
    void Update() {
        if (toque) {
            time += Time.deltaTime;
        }
        if(time >= 0.3) {
            boxPlataforma.isTrigger = true;
            plataformaRB.isKinematic = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            toque = true;
        }
    }
}
