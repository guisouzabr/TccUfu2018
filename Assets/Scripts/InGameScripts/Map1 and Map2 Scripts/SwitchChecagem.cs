using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchChecagem : MonoBehaviour {

    public SpriteRenderer imagem;
    public Sprite zero;
    public Sprite um;
    public bool status;
    public bool travado;

    public GameObject levelCompleteNum;
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
        levelCompleteNum.GetComponent<LevelCompleteNum>().status = false;
    }

    public void setaUm()
    {
        imagem.sprite = um;
        levelCompleteNum.GetComponent<LevelCompleteNum>().status = true;
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    public bool checaBinCorreto()
    {
        if (levelCompleteNum.GetComponent<LevelCompleteNum>().checaBinCorreto())
        {
            this.status = true;
            return true;
        }
        else
        {
            this.status = false;
            return false;
        }
    }
}
