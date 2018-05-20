using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchNum : MonoBehaviour
{

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
        }
        else
        {
            setaUm();
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
