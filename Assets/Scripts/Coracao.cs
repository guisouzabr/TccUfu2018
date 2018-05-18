using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coracao : MonoBehaviour {

    public Image imagem;
    public Sprite um;
    public Sprite zero;
    
    // Use this for initialization
    void Start () {
        imagem = GetComponent<Image>();
        setaUm();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setaUm()
    {
        imagem.sprite = um;
    }

    public void setaZero()
    {
        imagem.sprite = zero;
    }
}
