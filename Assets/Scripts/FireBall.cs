using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {
    public Rigidbody2D RB_FireBall;
    public GameObject Spawn;
    public int altura;

	// Use this for initialization
	void Start () {
        if (altura == 0)
            altura = 800;

	}
	
	// Update is called once per frame
	void Update () {
        rodar();

        if (Random.Range(1, 50) == 1 && 
            RB_FireBall.transform.position.y <= Spawn.transform.position.y &&
            RB_FireBall.velocity.y < 0) {
            RB_FireBall.velocity = new Vector3(0, 0, 0);
            RB_FireBall.position = Spawn.transform.position;
            RB_FireBall.AddForce(new Vector3(0, altura, 0));
        }
	}

    void rodar() {
        if(RB_FireBall.velocity.y >= 0) {
            RB_FireBall.rotation = 0;
        } else if (RB_FireBall.velocity.y < 0) {
            RB_FireBall.rotation = 180;
        }
    }
}
