using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour
{
    public float x;
    public float speed;

    public float distanceTraveled;
    public bool indo;
    // Use this for initialization
    void Start()
    {
        x = transform.position.x;
        indo = true;
    }

    // Update is called once per frame
    void Update()
    {


        if (x >= 70)
        {
            indo = false;
        }
        if (x <= 69)
        {
            indo = true;
        }
        if (indo)
        {
            x += 0.015f;
        }
        if (!indo)
        {
            x -= 0.015f;
        }


        transform.position = new Vector3(x, transform.position.y, transform.position.z);

    }
}