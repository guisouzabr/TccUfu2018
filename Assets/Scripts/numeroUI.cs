using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class numeroUI : MonoBehaviour {
    Image imagem;
    public Sprite zero;
    public Sprite um;

    // Use this for initialization
    void Start () {
        imagem = GetComponent<Image>();
        setaZero();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void setaZero()
    {
        imagem.sprite = zero;
    }

    public void setaUm()
    {
        imagem.sprite = um;
    }
}
