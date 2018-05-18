using UnityEngine;
using System.Collections;

public class EspinhoPlataforma : MonoBehaviour {
    public Transform marcadorPlataforma;
    public Transform marcadorEspinho;

    public GameObject espinhosObj;
    private bool indo;

    private float yFinal;
    private float y;
    private float yInicial;
    
    // Use this for initialization
    void Start() {
        yFinal = espinhosObj.transform.position.y + 0.35f;
        y = espinhosObj.transform.position.y;
        yInicial = y;
        indo = true;
    }

    // Update is called once per frame
    void Update() {
        /*if (toque) {
            ativo = true;
            toque = false;
            plataformaRB.isKinematic = false;
        }
        if(marcadorEspinho.position.y >= marcadorPlataforma.position.y) {
            plataformaRB.isKinematic = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player" && !ativo) {
            toque = true;
        }
        */
        if (y >= yFinal)
        {
            indo = false;
        }
        if (y <= yInicial - 0.10f)
        {
            indo = true;
        }
        if (indo)
        {
            y += 0.2f;
        }
        if (!indo)
        {
            y -= 0.0025f;
        }

        espinhosObj.transform.position = new Vector3(espinhosObj.transform.position.x, y, espinhosObj.transform.position.z);
    }
}
