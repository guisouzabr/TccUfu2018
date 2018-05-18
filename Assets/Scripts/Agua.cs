using UnityEngine;
using System.Collections;

public class Agua : MonoBehaviour
{
    public float x;
    public float speed;
    public float maxXesq;
    public float maxXdir;

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


        if (x >= maxXesq)
        {
            indo = false;
        }
        if (x <= maxXdir)
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