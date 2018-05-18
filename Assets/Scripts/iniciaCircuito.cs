using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iniciaCircuito : MonoBehaviour {

    public GameObject player;
    public GameObject circuito;
    public GameObject circuitoEasy;

    public Transform LL;
    public Transform RR;
    private bool circuitoIniciado;
    private float LLInicial;
    private bool circuitoPreparado;
    private bool cameraFixa;

    private bool dificuldadeEasy;

    public bool flagCompleto;  /* Flag utilizada para garantir que não vai iniciar um circuito já completo */

    public int modo; // 2 modos. 1 = Lock no botão, 2 = Lock nos operadores

    // Use this for initialization
    void Start () {
        cameraFixa = false;
        LLInicial = LL.position.x;
        circuitoPreparado = false;
        circuito.SetActive(false);
        circuitoEasy.SetActive(false);
        flagCompleto = false;
    }
	
	// Update is called once per frame
	void Update () {
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
        player.GetComponent<PlayerControl>().atualizaCanvasOperador();
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    public void moveCamera()
    {
        if (LL.position.x == LLInicial)
        {
            LL.position = new Vector2(player.GetComponent<Transform>().position.x, LL.position.y);
        }
        else if (LL.transform.position.x >= RR.transform.position.x)
        {
            LL.position = new Vector2(RR.position.x, LL.position.y);
            cameraFixa = true;
        }
        else
        {
            LL.position = new Vector2(LL.position.x+0.2f, LL.position.y);
        }
        
    }

    public void lockTudo()
    { 
        Transform[] ts;
        if (dificuldadeEasy)
        {
            circuitoEasy.SetActive(true);
            ts = circuitoEasy.transform.GetComponentsInChildren<Transform>();
            circuito.SetActive(false);
        }
        else
        {
            circuito.SetActive(true);
            ts = circuito.transform.GetComponentsInChildren<Transform>();
            circuitoEasy.SetActive(false);
        }
        foreach (Transform t in ts)
        {
            if (t.gameObject.name.Contains("Switch"))
            {
                if (modo == 1)
                {
                    t.gameObject.GetComponent<Switch>().travado = true;
                    bool aux; // Verifica se cada switch vai ficar ligado ou desligado
                    if (Random.Range(0, 2) == 0)
                    {
                        aux = false;  /* Switch desligado */
                    }
                    else
                    {
                        aux = true;  /* Switch ligado */
                    }
                    if (aux)
                    {
                        t.gameObject.GetComponent<Switch>().status = true;
                    }
                    else
                    {
                        t.gameObject.GetComponent<Switch>().status = false;
                    }
                }
            }
            else if (t.gameObject.name.Contains("Operador"))
            {
                if (modo == 2)
                {
                    t.gameObject.GetComponent<Operador>().travado = true;
                    string aux; // Verifica se cada switch vai ficar ligado ou desligado
                    if (Random.Range(0, 2) == 0)
                    {
                        aux = "and";  /* Operador and */
                    }
                    else
                    {
                        aux = "or";  /* Operador or */
                    }
                    if (aux == "or")
                    {
                        t.gameObject.GetComponent<Operador>().operador = "or";
                    }
                    else if (aux == "and")
                    {
                        t.gameObject.GetComponent<Operador>().operador = "and";
                    }
                }
            }

        }
    }

    public void verificacaoCircuito()
    {
        bool auxChecagem = false; /* Utilizado para verificar se ao menos um operador está recebendo ligação */
        Transform[] ts;
        if (dificuldadeEasy)
        {
            circuitoEasy.SetActive(true);
            ts = circuitoEasy.transform.GetComponentsInChildren<Transform>();
            circuito.SetActive(false);
        }
        else
        {
            circuito.SetActive(true);
            ts = circuito.transform.GetComponentsInChildren<Transform>();
            circuitoEasy.SetActive(false);
            getChildGameObject(player.GetComponent<PlayerControl>().score, "deVinte").GetComponent<Text>().text = "/ ∞";
        }
        foreach (Transform t in ts)
        {
            if (t.gameObject.name.Contains("LevelComplete"))
            {
                if (t.GetComponent<LevelComplete>().status)
                {
                    flagCompleto = true;
                }
            }
            else if (t.gameObject.name.Equals("dir"))
            {
                if (t.GetComponent<Switch>().status)
                {
                    auxChecagem = true;
                }
            }
        }

        if (flagCompleto || !auxChecagem)
        {
            flagCompleto = false;
            auxChecagem = false;
            reiniciaCircuito(); /* Reinicia o Circuito pra remontar */
            StartCoroutine(montaCircuito());
        }
    }

    public void reiniciaCircuito() /* Reinicia o circuito */
    {
        Transform[] ts;
        if (dificuldadeEasy)
        {
            circuitoEasy.SetActive(true);
            ts = circuitoEasy.transform.GetComponentsInChildren<Transform>();
            circuito.SetActive(false);
        }
        else
        {
            circuito.SetActive(true);
            ts = circuito.transform.GetComponentsInChildren<Transform>();
            circuitoEasy.SetActive(false);
        }
        foreach (Transform t in ts)
        {
            if (t.gameObject.name.Contains("not"))
            {
                t.GetComponent<Not>().feito = false;
            }
            if (t.gameObject.name.Contains("Switch"))
            {
                t.GetComponent<Switch>().status = false;
                t.GetComponent<Switch>().travado = false;
            }
            if (t.gameObject.name.Contains("Operador"))
            {
                t.GetComponent<Operador>().operador = "none";
                t.GetComponent<Operador>().travado = false;
            }
        }
    }

    
    IEnumerator montaCircuito() /* Monta o circuito e verifica se está possível de passar e já não está completo. */
    {
        lockTudo();
        yield return new WaitForSeconds(0.3f);
        verificacaoCircuito();
    }

    public void forcaReiniciarCircuito() /* Usado quando é reiniciado o circuito por fora, mesmo que o mapa esteja completo. */
    {
        selecionaModo();
        reiniciaCircuito(); /* Reinicia o Circuito pra remontar */
        StartCoroutine(montaCircuito());
    }

    public void selecionaModo()
    {
        if (Random.Range(0, 2) == 0) /* Verificação aleatória de qual modo usado. 1 = Lock no botao, 2 = Lock nos operadores */
        {
            modo = 1; // Lock no botão
            //Debug.Log("Modo = 1 - Lock no Botão");
            getChildGameObject(player.GetComponent<PlayerControl>().canvasInfo, "Texto informativo").GetComponent<Text>().text = "Botões travados, preencha apenas os operadores"; /* Altera o texto do canvas informando qual lock */
        }
        else
        {
            modo = 2; // Lock nos operadores
            //Debug.Log("Modo = 2 - Lock nos Operadores");
            getChildGameObject(player.GetComponent<PlayerControl>().canvasInfo, "Texto informativo").GetComponent<Text>().text = "Operadores travados, ligue os botões apenas"; /* Altera o texto do canvas informando qual lock */
        }
    }
}
