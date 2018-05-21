using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Not : MonoBehaviour {

    public GameObject entrada;
    public GameObject saida;
    SpriteRenderer imagem;
    public Sprite zero;
    public Sprite um;
    public bool status;
    public bool travado;
    public bool feito; /* Usado para verificar se já foi setado, alterar este para false em caso de reset de circuito */
    // Use this for initialization
    void Start () {
        status = false;
        travado = true; /* Not sempre aleatório */
        feito = false;
        imagem = GetComponent<SpriteRenderer>();
        this.gameObject.GetComponent<rotateScript>().enabled = false;     
    }

    void geraAleatorio()
    {
        if (Random.Range(0, 2) == 0) /* Verificação aleatória de status do not */
        {
            status = false;
        }
        else
        {
            status = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!feito)
        {
            geraAleatorio();
            feito = true;
        }
        if (status)
        {
            saida.GetComponent<Switch>().status = !entrada.GetComponent<Switch>().status;
            setaUm();
            if (this.gameObject.name.Contains("not"))
            {
                this.gameObject.GetComponent<rotateScript>().enabled = true;
            }
        }
        else
        {
            saida.GetComponent<Switch>().status = entrada.GetComponent<Switch>().status;
            setaZero();
            if (this.gameObject.name.Contains("not"))
            {
                this.gameObject.GetComponent<rotateScript>().enabled = false;
                transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, 0, 0, 0), 5f);
            }
        }
    }

    public void setaZero()
    {
        imagem.sprite = zero;
    }

    public void setaUm()
    {
        imagem.sprite = um;
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
