using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour {

    public SpriteRenderer imagem;
    public Sprite zero;
    public Sprite um;
    public bool status;
    public bool travado;
    // Use this for initialization
    void Start()
    {
        status = false;
        travado = false;
        imagem = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!status)
        {
            setaZero();
            if (this.gameObject.name.Contains("Switch"))
            {
                getChildGameObject(this.gameObject, "esq").GetComponent<Switch>().status = false;
            }
        }
        else
        {
            setaUm();
            if (this.gameObject.name.Contains("Switch"))
            {
                getChildGameObject(this.gameObject, "esq").GetComponent<Switch>().status = true;
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