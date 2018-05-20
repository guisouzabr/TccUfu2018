using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class tutorial : MonoBehaviour {

    public bool primeiraPagina; /* Boolean para verificar se é a primeira página. */
    public GameObject anterior; /* Aponta para a página anterior. */
    public GameObject proxima; /* Aponta para a página posterior. */
    public bool ultimaPagina; /* Boolean para verificar se é a primeira página. */

    // Use this for initialization
    void Start () {
        if (primeiraPagina) /* Desativa o botão de anterior caso seja primeira página */
        {
            getChildGameObject(this.gameObject, "Ant").SetActive(false);
        }
        if (anterior != null)
        {
            getChildGameObject(this.gameObject, "Ant").GetComponent<Button>().onClick.AddListener(() =>
            {
                btnAntClicado();
            });
        }
        if (proxima != null || ultimaPagina)
        {
            getChildGameObject(this.gameObject, "Prox").GetComponent<Button>().onClick.AddListener(() =>
            {
                btnProxClicado();
            });
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    public void btnProxClicado() /* Função do que ocorre quando clica em próximo.*/
    {
        if (ultimaPagina)
        {
            SceneManager.LoadScene("map1");
        }
        else
        {
            proxima.SetActive(true);
            this.gameObject.SetActive(false);
        }
        
    }

    public void btnAntClicado() /* Função do que ocorre quando clica em anterior.*/
    {
        anterior.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
