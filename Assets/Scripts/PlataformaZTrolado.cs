using UnityEngine;
using System.Collections;

public class PlataformaZTrolado : MonoBehaviour {
    public Rigidbody2D plataforma;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(plataforma.rotation >= 45) {
            plataforma.MoveRotation(45);
        }else if(plataforma.rotation <= -45) {
            plataforma.MoveRotation(-45);
        }
    }
}
