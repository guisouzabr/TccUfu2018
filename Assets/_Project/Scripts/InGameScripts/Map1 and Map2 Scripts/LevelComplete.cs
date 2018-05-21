using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour {
    public SpriteRenderer imagem;
    public Sprite zero;
    public Sprite um;
    public bool status = false;
    public GameObject entrada;
    public GameObject barreira;
    public int animacaoAtiva;
    public int animacaoInativa;
    // Use this for initialization
    void Start () {
        animacaoAtiva = 1;
        imagem = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		if (entrada.GetComponent<Switch>().status)
        {
            this.gameObject.GetComponent<LevelComplete>().status = true;
        }
        else
        {
            this.gameObject.GetComponent<LevelComplete>().status = false;
        }
        if (status)
        {
            setaUm();
            barreira.SetActive(false);
        }
        else
        {
            setaZero();
            barreira.SetActive(true);
        }
	}

    public void setaZero()
    {
        imagem.sprite = zero;
        getChildGameObject(barreira, animacaoAtiva.ToString()).GetComponent<spawner>().boolAux = true;
    }

    public void setaUm()
    {
        imagem.sprite = um;
        getChildGameObject(barreira, animacaoAtiva.ToString()).GetComponent<spawner>().boolAux = false;
    }

    public void setaAnimacaoAtiva(int anim)
    {
        animacaoAtiva = anim;
    }
    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
