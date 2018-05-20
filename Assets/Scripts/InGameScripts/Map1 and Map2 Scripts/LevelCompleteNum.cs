using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LevelCompleteNum : MonoBehaviour
{
    public bool status;
    public GameObject num0;
    public GameObject num1;
    public GameObject num2;
    public GameObject num3;
    public GameObject num4;
    public GameObject barreira;
    public int animacaoAtiva;

    public int numEntrada; /* Número de entrada (decimal) */
    public string binarioPosConv; /* Número Bin após conversão do decimal de entrada */
    public bool numNegativo; /* Bool usado pra verificar se é um número negativo */

    // Use this for initialization
    void Start()
    {
        animacaoAtiva = UnityEngine.Random.Range(1, 3);
        converteDecimalBinario();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (entrada.GetComponent<SwitchNum>().status)
        {
            this.gameObject.GetComponent<LevelCompleteNum>().status = true;
        }
        else
        {
            this.gameObject.GetComponent<LevelCompleteNum>().status = false;
        }
        */

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
        //checaBinCorreto();
    }

    public void setaZero()
    {
        // Debug.Log("SetaZeroLevelCompleteNum");
        getChildGameObject(barreira, animacaoAtiva.ToString()).GetComponent<spawner>().boolAux = true;
    }

    public void setaUm()
    {
        // Debug.Log("SetaUmLevelCompleteNum");
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

    public void converteDecimalBinario()
    {
        int entradaAux = numEntrada;
        binarioPosConv = "";
        if (entradaAux < 0)
        {
            numNegativo = true;
            entradaAux = entradaAux * -1;
        }
        while (entradaAux >= 2)
        {
            binarioPosConv += (entradaAux % 2).ToString();
            entradaAux = (entradaAux / 2);
        }

        binarioPosConv += entradaAux.ToString();
        binarioPosConv = inverteString(binarioPosConv);

        int aux = binarioPosConv.Length;
        for (int i = 0; i < 4 - aux; i++) /* Completar o número de dígitos com 0 */
        {
            binarioPosConv = "0" + binarioPosConv;
        }

        if (numNegativo)
        {
            binarioPosConv = "1" + binarioPosConv;
        }
        else if (!numNegativo && numEntrada <= 15)
        {
            binarioPosConv = "0" + binarioPosConv;
        }
    }

    public string inverteString(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public bool checaBinCorreto() /* Chamar sempre que for verificar se o binário está correto. */
    {
        bool aux = true;
        if (!binarioPosConv[0].Equals(num4.GetComponent<SwitchNum>().status ? '1' : '0'))
        {
            aux = false;
        }
        if (!binarioPosConv[1].Equals(num3.GetComponent<SwitchNum>().status ? '1' : '0'))
        {
            aux = false;
        }
        if (!binarioPosConv[2].Equals(num2.GetComponent<SwitchNum>().status ? '1' : '0'))
        {
            aux = false;
        }
        if (!binarioPosConv[3].Equals(num1.GetComponent<SwitchNum>().status ? '1' : '0'))
        {
            aux = false;
        }
        if (!binarioPosConv[4].Equals(num0.GetComponent<SwitchNum>().status ? '1' : '0'))
        {
            aux = false;
        }

        if (aux)
        {
            this.gameObject.GetComponent<LevelCompleteNum>().status = true;
            return true;
        }
        else
        {
            this.gameObject.GetComponent<LevelCompleteNum>().status = false;
            return false;
        }
    }
}
