﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnStartGame : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void iniciarJogo()
    {
        SceneManager.LoadScene("mapbin1");
    }

    public void iniciarJogo2()
    {
        SceneManager.LoadScene("map2");
    }
}
