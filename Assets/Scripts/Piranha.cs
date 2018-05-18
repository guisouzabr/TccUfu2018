using UnityEngine;
using System.Collections;

public class Piranha : MonoBehaviour {
    public Rigidbody2D piranha;
    public GameObject Spawn;
    public Transform piranhaTR;
    public int x;
    public int y;

    // Use this for initialization
    void Start () {
        if (x == 0) x = 200;
        if (y == 0) y = 400;
        if (x < 0) {
            piranhaTR.localScale = new Vector3(piranhaTR.localScale.x * -1, piranhaTR.localScale.y, piranhaTR.localScale.z);
        }
	}

    void Update() {
        if (Random.Range(1, 50) == 1 &&
            piranha.transform.position.y <= Spawn.transform.position.y) {
            piranha.velocity = new Vector3(0, 0, 0);
            piranha.position = Spawn.transform.position;
            piranha.AddForce(new Vector3(x, y, 0));
        }
    }
}
