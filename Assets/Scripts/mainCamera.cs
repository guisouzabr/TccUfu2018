using UnityEngine;
using System.Collections;

public class mainCamera : MonoBehaviour {
    public Transform player;
    public Transform LL;
    public Transform LR;
    public Transform UU;
    public Camera Camera;    
    private float x;
    private float y;
    public float transition;
    public bool usaLerp;
    public bool segueY;
    public float ajusteY;

    void Start() {

    }

    void LateUpdate() {
        moveCamera();
    }

    void moveCamera() {
        x = player.position.x;
        y = player.position.y;
        if (x < LL.position.x) {
            x = LL.position.x;
        } else if (x > LR.position.x) {
            x = LR.position.x;
        }

        if (segueY) {
            if (player.position.y >= ajusteY + 1) {
                y = player.position.y;
                if (y >= UU.position.y)
                {
                    y = UU.position.y;
                }
            } else {
                y = ajusteY;
                if (y >= UU.position.y)
                {
                    y = UU.position.y;
                }
            }
        } else {
            y = transform.position.y;
            if (y >= UU.position.y)
            {
                y = UU.position.y;
            }
        }

        if (usaLerp) {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(x, y, transform.position.z),
                transition);

        } else {
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
