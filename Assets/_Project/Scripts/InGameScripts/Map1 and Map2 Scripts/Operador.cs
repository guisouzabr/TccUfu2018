using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operador : MonoBehaviour {

    public GameObject entrada1;
    public GameObject entrada2;
    public GameObject saida;
    public string operador;
    public SpriteRenderer imagem;
    public Sprite and;
    public Sprite or;
    public Sprite none;
    public bool travado;

    // Use this for initialization
    void Start () {
        imagem = GetComponent<SpriteRenderer>();
        //travado = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (operador == "and")
        {
            setaAnd();
            if ((entrada1.gameObject.GetComponent<Switch>().status) && (entrada2.gameObject.GetComponent<Switch>().status))
            {
                saida.gameObject.GetComponent<Switch>().status = true;
            }
            else
            {
                saida.gameObject.GetComponent<Switch>().status = false;
            }
        }
        else if (operador == "or")
        {
            setaOr();
            if ((entrada1.gameObject.GetComponent<Switch>().status) || (entrada2.gameObject.GetComponent<Switch>().status))
            {
                saida.gameObject.GetComponent<Switch>().status = true;
            }
            else
            {
                saida.gameObject.GetComponent<Switch>().status = false;
            }
        }
        else
        {
            setaNada();
            saida.gameObject.GetComponent<Switch>().status = false;
        }
	}

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    void setaAnd()
    {
        imagem.sprite = and;
    }

    void setaOr()
    {
        imagem.sprite = or;
    }

    void setaNada()
    {
        imagem.sprite = none;
    }

}
