using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iniciaCircuitoNum : MonoBehaviour
{

    public GameObject player;
    public GameObject circuito;

    private bool circuitoIniciado;
    private float LLInicial;
    private bool circuitoPreparado;
    private bool cameraFixa;

    private bool dificuldadeEasy;

    public bool flagCompleto;  /* Flag utilizada para garantir que não vai iniciar um circuito já completo */

    public GameObject levelCompleteNum;
    public int modo; // 2 modos. 1 = Sem num negativo, 2 = Com num Negativo

    public int ultimaEntrada;
    // Use this for initialization
    void Start()
    {
        cameraFixa = false;
        circuitoPreparado = false;
        flagCompleto = false;
        ultimaEntrada = -1;
    }

    // Update is called once per frame
    void Update()
    {
        circuitoIniciado = player.GetComponent<PlayerControl>().circuitoIniciado;
        dificuldadeEasy = player.GetComponent<PlayerControl>().dificuldadeEasy; // Atualiza a dificuldade no início do Circuito
        if (circuitoIniciado && !circuitoPreparado)
        {
            preparaCircuito();
        }
        if (circuitoPreparado && !cameraFixa)
        {
            //moveCamera();
        }
    }

    public void preparaCircuito()
    {
        player.GetComponent<PlayerControl>().canvasInfo.SetActive(true);
        selecionaModo();
        StartCoroutine(montaCircuito());

        circuitoPreparado = true;
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    public void verificacaoCircuito()
    {
        if (flagCompleto)
        {
            flagCompleto = false;
            reiniciaCircuito(); /* Reinicia o Circuito pra remontar */
            StartCoroutine(montaCircuito());
        }
    }

    public void reiniciaCircuito() /* Reinicia o circuito */
    {
        Transform[] ts;
        ts = circuito.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts)
        {
            if (t.gameObject.name.Contains("binario"))
            {
                t.GetComponent<SwitchNum>().status = false;
            }
            if (t.gameObject.name.Contains("Checagem"))
            {
                t.GetComponent<SwitchChecagem>().status = false;
            }
        }
        levelCompleteNum.GetComponent<LevelCompleteNum>().numNegativo = false;
    }


    IEnumerator montaCircuito() /* Monta o circuito e verifica se está possível de passar e já não está completo. */
    {
        yield return new WaitForSeconds(0.3f);
        verificacaoCircuito();
        bool flagChecaRepetido = true; /* Usado para checar se o último número é igual ao novo */
        int auxRandom = 0;
        while (flagChecaRepetido)
        {
            if (dificuldadeEasy)
            {
                auxRandom = Random.Range(0, 32);
            }
            else
            {
                auxRandom = Random.Range(-15, 16);
            }
            levelCompleteNum.GetComponent<LevelCompleteNum>().numEntrada = auxRandom;
            levelCompleteNum.GetComponent<LevelCompleteNum>().converteDecimalBinario();
            if (auxRandom != ultimaEntrada)
            {
                flagChecaRepetido = false;
            }
            ultimaEntrada = auxRandom;
            getChildGameObject(player.GetComponent<PlayerControl>().canvasInfo, "Entrada num").GetComponent<Text>().text = auxRandom.ToString(); /* Altera o texto do canvas informando qual lock */
        }
    }

    IEnumerator forcaReiniciarComTempo() /* com tempo para dar tempo de atualizar a dificuldade */
    {
        yield return new WaitForSeconds(0.3f);
        selecionaModo();
        reiniciaCircuito(); /* Reinicia o Circuito pra remontar */
        StartCoroutine(montaCircuito());
    }
    public void forcaReiniciarCircuito() /* Usado quando é reiniciado o circuito por fora, mesmo que o mapa esteja completo. */
    {
        StartCoroutine(forcaReiniciarComTempo());
    }

    public void selecionaModo()
    {
        if (dificuldadeEasy) /* Verificação aleatória de qual modo usado. 1 = Sem num negativo, 2 = Com num Negativo */
        {
            modo = 1;
            getChildGameObject(player.GetComponent<PlayerControl>().canvasInfo, "Texto informativo").GetComponent<Text>().text = "NÃO estamos considerando o bit de sinal"; /* Altera o texto do canvas informando qual lock */
        }
        else
        {
            modo = 2;
            getChildGameObject(player.GetComponent<PlayerControl>().canvasInfo, "Texto informativo").GetComponent<Text>().text = "ESTAMOS considerando o bit de sinal"; /* Altera o texto do canvas informando qual lock */
        }
    }
}
