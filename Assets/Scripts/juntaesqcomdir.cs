using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class juntaesqcomdir : MonoBehaviour
{
    public GameObject esq;
    public GameObject dir;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        dir.GetComponent<Switch>().status = esq.GetComponent<Switch>().status;
    }
}
