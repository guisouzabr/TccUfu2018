using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour {

    private float yMax;
    private float yMin;
    private float auxYmax;
    private float auxYmin;
    private int auxDirecao;
    private float speed;

    // Use this for initialization
    void Start () {
        yMax = 0.3f;
        yMin = 0.3f;
        auxYmax = transform.position.y + yMax;
        auxYmin = transform.position.y - yMin;
        auxDirecao = 0;
        speed = (auxYmax - auxYmin) / 50;
    }
	
	// Update is called once per frame
	void Update () {
        if (auxDirecao == 0)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + speed);
            if (transform.position.y >= auxYmax)
            {
                auxDirecao = 1;
            }
        } else {
            transform.position = new Vector2(transform.position.x, transform.position.y - speed);
            if (transform.position.y <= auxYmin)
            {
                auxDirecao = 0;
            }
        }
	}
}
