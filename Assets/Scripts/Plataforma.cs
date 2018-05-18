using UnityEngine;
using System.Collections;

public class Plataforma : MonoBehaviour {
    public Rigidbody2D plataformaRB;
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
        if(time >= 1) {
            plataformaRB.isKinematic = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        toque = true;
    }
}
