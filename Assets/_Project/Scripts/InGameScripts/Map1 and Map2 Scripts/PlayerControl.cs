using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControl : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;         // For determining which way the player is currently facing.
    [HideInInspector]
    public bool jump = false;               // Condition for whether the player should jump.


    private float moveForce = 300f;         // Amount of force added to move the player left and right.
    private float maxSpeed = 5f;            // The fastest the player can travel in the x axis.
    public AudioClip[] jumpClips;           // Array of clips for when the player jumps.
    private float jumpForce = 700;         // Amount of force added when the player jumps.
    public AudioClip[] taunts;              // Array of clips for when the player taunts.
    public float tauntProbability = 50f;    // Chance of a taunt happening.
    public float tauntDelay = 1f;			// Delay for when the taunt should happen.
    public LayerMask whatIsGround;

    private int tauntIndex;                 // The index of the taunts array indicating the most recent taunt.
    private Transform groundCheck;          // A position marking where to check if the player is grounded.
    private bool grounded = false;          // Whether or not the player is grounded.
    private Animator anim;					// Reference to the player's animator component.

    public GameObject canvasMorto;
    public GameObject canvasInfo;
    private bool morto; // Determinar quando o personagem está morto.

    /* Canvas de operadores lógicos */
    public GameObject btnAnd;
    public GameObject btnOr;
    public GameObject btnNot;
    public GameObject btnHand;
    private int countAnd;
    private int countOr;
    private int countNot;
    private string numeroBin;
    public GameObject nrsCanvasAnd;
    public GameObject nrsCanvasOr;
    public GameObject nrsCanvasNot;
    public GameObject canvasHealth;         // Objeto Health dentro do CanvasOperadores
    private int vidas;                      // Aux para cálculo de qts lifes ainda tenho
    private string somaOperador;

    List<Collider2D> collidedObjects = new List<Collider2D>();

    public Transform MIN_RR; //Posição X inicial depois que o circuito for iniciado
    public Transform MAX_RR; //Posição X final do mapa
    public bool circuitoIniciado;

    public GameObject score;
    public bool dificuldadeEasy; // Checa dificuldade de acordo com o Score
    public bool mapaBin;
    public GameObject iniciaCircuito; /* Obj do trigger iniciaCircuito pra reiniciar o mapa qdo passar */

    public GameObject soundSwitch;
    public GameObject canvasHit; /* Canvas com Panel Red */
    public GameObject audioHit; /* Audio qdo toma hit */

    void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("groundCheck");
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        circuitoIniciado = false;
        morto = false;
        canvasMorto.SetActive(false);
        canvasInfo.SetActive(false);
        countAnd = 3;
        countOr = 3;
        countNot = 3;
        somaOperador = null;
        numeroBin = "0 0 0";
        vidas = 3;
        dificuldadeEasy = true;
        if (!mapaBin)
        {
            getChildGameObject(btnAnd, "ButtonAnd").GetComponent<Button>().onClick.AddListener(() =>
            {
                btnAndClicado();
            });
            getChildGameObject(btnOr, "ButtonOr").GetComponent<Button>().onClick.AddListener(() =>
            {
                btnOrClicado();
            });
            getChildGameObject(btnHand, "ButtonHand").GetComponent<Button>().onClick.AddListener(() =>
            {
                btnHandClicado();
            });
        }
    }

    void Update()
    {
        if (morto)
        {
            morreu();
        }
        if (CrossPlatformInputManager.GetButtonDown("Action") && !morto)
        {
            checaTroca();
        }
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        //grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.3f, whatIsGround);
        // If the jump button is pressed and the player is grounded then the player should jump.
        if (CrossPlatformInputManager.GetButtonDown("Jump") && grounded)
            jump = true;

        if (!mapaBin)
        {
            if (somaOperador != null)
            {
                if (somaOperador == "and")
                {
                    countAnd++;
                }
                else if (somaOperador == "or")
                {
                    countOr++;
                }
                else if (somaOperador == "not")
                {
                    countNot++;
                }
                atualizaCanvasOperador();
                somaOperador = null;
            }
        }
        //checaVelocity();
        atualizaCanvasHealth();
        atualizaDificuldade();
    }


    void FixedUpdate()
    {

        if ((this.GetComponent<Transform>().position.x) >= MAX_RR.GetComponent<Transform>().position.x)
        {
            this.GetComponent<Transform>().position = new Vector3(MAX_RR.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y, this.GetComponent<Transform>().position.z);
        }
        collidedObjects.Clear();
        // Cache the horizontal input.
        //float h = Input.GetAxis("Horizontal");
        float h = CrossPlatformInputManager.GetAxis("Horizontal");

        // The Speed animator parameter is set to the absolute value of the horizontal input.
        anim.SetFloat("Speed", Mathf.Abs(h));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        //if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
        // ... add a force to the player.
        if (!morto)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h/2 * moveForce);
        }

        // If the player's horizontal velocity is greater than the maxSpeed...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            // ... set the player's velocity to the maxSpeed in the x axis.
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (h > 0 && !facingRight)
            // ... flip the player.
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (h < 0 && facingRight)
            // ... flip the player.
            Flip();

        // If the player should jump...
        if (jump)
        {
            // Set the Jump animator trigger parameter.
            anim.SetTrigger("Jump");

            // Play a random jump audio clip.
            int i = Random.Range(0, jumpClips.Length);
            AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

            // Add a vertical force to the player.
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

            // Make sure the player can't jump again until the jump conditions from Update are satisfied.
            jump = false;
        }
    }


    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    public IEnumerator Taunt()
    {
        // Check the random chance of taunting.
        float tauntChance = Random.Range(0f, 100f);
        if (tauntChance > tauntProbability)
        {
            // Wait for tauntDelay number of seconds.
            yield return new WaitForSeconds(tauntDelay);

            // If there is no clip currently playing.
            if (!GetComponent<AudioSource>().isPlaying)
            {
                // Choose a random, but different taunt.
                tauntIndex = TauntRandom();

                // Play the new taunt.
                GetComponent<AudioSource>().clip = taunts[tauntIndex];
                GetComponent<AudioSource>().Play();
            }
        }
    }


    int TauntRandom()
    {
        // Choose a random index of the taunts array.
        int i = Random.Range(0, taunts.Length);

        // If it's the same as the previous taunt...
        if (i == tauntIndex)
            // ... try another random taunt.
            return TauntRandom();
        else
            // Otherwise return this index.
            return i;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.tag);
        /*if (col.tag == "gemas")
        {
            isDoubleJump = false;
            col.gameObject.SetActive(false);
        }

        if (col.tag == "AjusteCamera")
        {
            cam.ajusteY = 0;
        }

        if (col.tag == "coletavel")
        {
            pontuacao++;
            col.gameObject.SetActive(false);
        }

        if (col.tag == "interacao")
        {
            objetointeracao = col.gameObject;
        }


        if (col.tag == "espinho" && !morto)
        {
            audioSource.PlayOneShot(soundTrap, 0.3f);
            morto = true;
            anime.SetBool("dead", true);
            anime.SetBool("grounded", true);
            StartCoroutine("contadorSegundos");
        }
        if (col.tag == "piranha" && !morto)
        {
            audioSource.PlayOneShot(soundPiranha, 0.3f);
            morto = true;
            anime.SetBool("dead", true);
            anime.SetBool("grounded", true);
            StartCoroutine("contadorSegundos");
        }
        if (col.tag == "lava" && !morto)
        {
            audioSource.PlayOneShot(soundDeadByLava, 0.3f);
            morto = true;
            anime.SetBool("dead", true);
            anime.SetBool("grounded", true);
            StartCoroutine("contadorSegundos");
        }

        
        if (col.tag == "agua" && !morto)
        {
            audioSource.PlayOneShot(soundDeadByAgua, 0.7f);
            morto = true;
            anime.SetBool("dead", true);
            anime.SetBool("grounded", true);
            StartCoroutine("contadorSegundos");
        }
        */
        if (col.tag == "matador" && !morto)
        {
            morreu();
        }
        if (col.tag == "informativo" && !morto)
        {
            canvasInfo.SetActive(true);
        }
        if (col.tag == "operadorAnd" && !morto)
        {
            somaOperador = "and";
            col.gameObject.SetActive(false);
        }
        if (col.tag == "operadorOr" && !morto)
        {
            somaOperador = "or";
            col.gameObject.SetActive(false);
        }
        if (col.tag == "operadorNot" && !morto)
        {
            somaOperador = "not";
            col.gameObject.SetActive(false);
        }
        if (col.gameObject.layer == 12)
        {
            //getChildGameObject(col.gameObject, "Fisico").GetComponent<BoxCollider2D>().enabled = true;
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (col.tag == "iniciaCircuito")
        {
            circuitoIniciado = true;
        }
        if (col.tag == "telesq")
        {
            /* Collider de fim de mapa */
            if (circuitoIniciado)
            {
                this.transform.position = new Vector2(-2f, this.transform.position.y);
                circuitoIniciado = false;
                iniciaCircuito.gameObject.GetComponent<iniciaCircuito>().forcaReiniciarCircuito();
                int auxScore = int.Parse(score.GetComponent<Text>().text);
                auxScore++;
                score.GetComponent<Text>().text = auxScore.ToString();
                countAnd = 3;
                countOr = 3;
                countNot = 3;
                atualizaCanvasOperador();
            }
        }
        if (col.tag == "telesqbin")
        {
            /* Collider de fim de mapa */
            if (circuitoIniciado)
            {
                this.transform.position = new Vector2(-2f, this.transform.position.y);
                circuitoIniciado = false;
                iniciaCircuito.gameObject.GetComponent<iniciaCircuitoNum>().forcaReiniciarCircuito();
                int auxScore = int.Parse(score.GetComponent<Text>().text);
                auxScore++;
                score.GetComponent<Text>().text = auxScore.ToString();
                if (int.Parse(score.GetComponent<Text>().text) >= 20)
                {
                    SceneManager.LoadScene("map2");
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "ChaoInvisivel" && !morto)
        {
            if (CrossPlatformInputManager.GetAxis("Vertical") < -0.75)
            {
                //getChildGameObject(col.gameObject, "Fisico").GetComponent<BoxCollider2D>().enabled = false;
                this.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        if (col.tag == "CFSwitch" && !morto)
        {
            if (!col.gameObject.GetComponent<Switch>().travado)
            {
                if (!collidedObjects.Contains(col))
                {
                    collidedObjects.Add(col);
                }
            }
        }
        if (col.tag == "CFNum" && !morto)
        {
            if (!collidedObjects.Contains(col))
            {
                collidedObjects.Add(col);
            }
            /* if (!col.gameObject.GetComponent<SwitchNum>().travado)
            {
                if (!collidedObjects.Contains(col))
                {
                    collidedObjects.Add(col);
                    col.GetComponent<SwitchNum>().status = !col.GetComponent<SwitchNum>().status;
                    Instantiate(soundSwitch, transform.position, Quaternion.identity);
                }
            }
            */
        }
        if (col.tag == "CFOperador" && !morto)
        {
            if (!collidedObjects.Contains(col))
            {
                collidedObjects.Add(col);
            }
        }
        if (col.tag == "CFNot" && !morto)
        {
            if (!collidedObjects.Contains(col))
            {
                collidedObjects.Add(col);
            }
        }
        if (col.tag == "CFChecagem" && !morto)
        {
            if (!collidedObjects.Contains(col))
            {
                collidedObjects.Add(col);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "informativo")
        {
            canvasInfo.SetActive(false);
        }

    }

    public void atualizaCanvasOperador()
    {
        List<string> s;

        //Começo verificação And
        if (countAnd == 0)
        {
            numeroBin = "0 0 0";
        }
        else if (countAnd == 1)
        {
            numeroBin = "0 0 1";
        }
        else if (countAnd == 2)
        {
            numeroBin = "0 1 0";
        }
        else if (countAnd == 3)
        {
            numeroBin = "0 1 1";
        }
        else if (countAnd == 4)
        {
            numeroBin = "1 0 0";
        }
        else if (countAnd == 5)
        {
            numeroBin = "1 0 1";
        }
        else if (countAnd == 6)
        {
            numeroBin = "1 1 0";
        }
        else if (countAnd == 7)
        {
            numeroBin = "1 1 1";
        }
        s = new List<string>(numeroBin.Split(new string[] { " " }, System.StringSplitOptions.None));
        if (s[0] == "0")
        {
            getChildGameObject(nrsCanvasAnd, "ImgEsq").GetComponent<numeroUI>().setaZero();
        }
        else
        {
            getChildGameObject(nrsCanvasAnd, "ImgEsq").GetComponent<numeroUI>().setaUm();
        }
        if (s[1] == "0")
        {
            getChildGameObject(nrsCanvasAnd, "ImgMeio").GetComponent<numeroUI>().setaZero();
        }
        else
        {
            getChildGameObject(nrsCanvasAnd, "ImgMeio").GetComponent<numeroUI>().setaUm();
        }
        if (s[2] == "0")
        {
            getChildGameObject(nrsCanvasAnd, "ImgDir").GetComponent<numeroUI>().setaZero();
        }
        else
        {
            getChildGameObject(nrsCanvasAnd, "ImgDir").GetComponent<numeroUI>().setaUm();
        }
        //Fim verificação And

        //Começa verificação Or
        if (countOr == 0)
        {
            numeroBin = "0 0 0";
        }
        else if (countOr == 1)
        {
            numeroBin = "0 0 1";
        }
        else if (countOr == 2)
        {
            numeroBin = "0 1 0";
        }
        else if (countOr == 3)
        {
            numeroBin = "0 1 1";
        }
        else if (countOr == 4)
        {
            numeroBin = "1 0 0";
        }
        else if (countOr == 5)
        {
            numeroBin = "1 0 1";
        }
        else if (countOr == 6)
        {
            numeroBin = "1 1 0";
        }
        else if (countOr == 7)
        {
            numeroBin = "1 1 1";
        }
        s = new List<string>(numeroBin.Split(new string[] { " " }, System.StringSplitOptions.None));
        if (s[0] == "0")
        {
            getChildGameObject(nrsCanvasOr, "ImgEsq").GetComponent<numeroUI>().setaZero();
        }
        else
        {
            getChildGameObject(nrsCanvasOr, "ImgEsq").GetComponent<numeroUI>().setaUm();
        }
        if (s[1] == "0")
        {
            getChildGameObject(nrsCanvasOr, "ImgMeio").GetComponent<numeroUI>().setaZero();
        }
        else
        {
            getChildGameObject(nrsCanvasOr, "ImgMeio").GetComponent<numeroUI>().setaUm();
        }
        if (s[2] == "0")
        {
            getChildGameObject(nrsCanvasOr, "ImgDir").GetComponent<numeroUI>().setaZero();
        }
        else
        {
            getChildGameObject(nrsCanvasOr, "ImgDir").GetComponent<numeroUI>().setaUm();
        }
        //Fim verificação Or

        /* Descomentar aqui para contagem do NOT */
        /* 
        //Começa verificação Not
        if (countNot == 0)
        {
            numeroBin = "0 0 0";
        }
        else if (countNot == 1)
        {
            numeroBin = "0 0 1";
        }
        else if (countNot == 2)
        {
            numeroBin = "0 1 0";
        }
        else if (countNot == 3)
        {
            numeroBin = "0 1 1";
        }
        else if (countNot == 4)
        {
            numeroBin = "1 0 0";
        }
        else if (countNot == 5)
        {
            numeroBin = "1 0 1";
        }
        else if (countNot == 6)
        {
            numeroBin = "1 1 0";
        }
        else if (countNot == 7)
        {
            numeroBin = "1 1 1";
        }
        
        s = new List<string>(numeroBin.Split(new string[] { " " }, System.StringSplitOptions.None));
        if (s[0] == "0")
        {
            getChildGameObject(nrsCanvasNot, "ImgEsq").GetComponent<numeroUI>().setaZero();
        }
        else
        {
            getChildGameObject(nrsCanvasNot, "ImgEsq").GetComponent<numeroUI>().setaUm();
        }
        if (s[1] == "0")
        {
            getChildGameObject(nrsCanvasNot, "ImgMeio").GetComponent<numeroUI>().setaZero();
        }
        else
        {
            getChildGameObject(nrsCanvasNot, "ImgMeio").GetComponent<numeroUI>().setaUm();
        }
        if (s[2] == "0")
        {
            getChildGameObject(nrsCanvasNot, "ImgDir").GetComponent<numeroUI>().setaZero();
        }
        else
        {
            getChildGameObject(nrsCanvasNot, "ImgDir").GetComponent<numeroUI>().setaUm();
        }
        //Fim verificação Not
        */ /* Descomentar aqui para contagem do NOT */

    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    void checaVelocity()
    {
        foreach (Collider2D t in collidedObjects)
        {
            if (GetComponent<Rigidbody2D>().velocity.y > 0 && t.tag == "ChaoInvisivel")
            {
                GetComponent<BoxCollider2D>().isTrigger = true;
            }
            else
            {
                GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }

    void btnAndClicado()
    {
        if (countAnd > 0)
        {
            foreach (Collider2D t in collidedObjects)
            {
                if (t.tag == "CFOperador")
                {
                    if (!t.GetComponent<Operador>().travado)
                    {
                        if (t.GetComponent<Operador>().operador == "none")
                        {
                            countAnd--;
                            t.GetComponent<Operador>().operador = "and";
                        }
                        else if (t.GetComponent<Operador>().operador == "or")
                        {
                            countAnd--;
                            countOr++;
                            vidas--;
                            tomouHit();
                            t.GetComponent<Operador>().operador = "and";
                        }
                    }
                }
            }
            atualizaCanvasOperador();
        }
    }
    void btnOrClicado()
    {
        if (countOr > 0)
        {
            foreach (Collider2D t in collidedObjects)
            {
                if (t.tag == "CFOperador")
                {
                    if (!t.GetComponent<Operador>().travado)
                    {
                        if (t.GetComponent<Operador>().operador == "none")
                        {
                            countOr--;
                            t.GetComponent<Operador>().operador = "or";
                        }
                        else if (t.GetComponent<Operador>().operador == "and")
                        {
                            countOr--;
                            countAnd++;
                            vidas--;
                            tomouHit();
                            t.GetComponent<Operador>().operador = "or";
                        }
                    }
                }
            }
            atualizaCanvasOperador();
        }
    }
    void btnNotClicado()
    {
        if (countNot > 0)
        {
            foreach (Collider2D t in collidedObjects)
            {
                if (t.tag == "CFNot")
                {
                    if (!t.GetComponent<Not>().travado)
                    {
                        if (!t.GetComponent<Not>().status)
                        {
                            countNot--;
                            t.GetComponent<Not>().status = true;
                        }
                    }
                }
            }
            atualizaCanvasOperador();
        }
    }

    void checaTroca()
    {
        foreach (Collider2D t in collidedObjects)
        {
            if (t.tag == "CFNum")
            {
                t.GetComponent<SwitchNum>().status = !t.GetComponent<SwitchNum>().status;
                Instantiate(soundSwitch, transform.position, Quaternion.identity);
            }
            if (t.tag == "CFChecagem")
            {
                Instantiate(soundSwitch, transform.position, Quaternion.identity);
                if (!t.GetComponent<SwitchChecagem>().checaBinCorreto())
                {
                    vidas--;
                    tomouHit();
                }
            }
            if (t.tag == "CFSwitch")
            {
                t.GetComponent<Switch>().status = !t.GetComponent<Switch>().status;
                Instantiate(soundSwitch, transform.position, Quaternion.identity);
            }
        }
    }

    void tomouHit()
    {
        Instantiate(canvasHit, transform.position, Quaternion.identity);
        Instantiate(audioHit, transform.position, Quaternion.identity);
    }


    void btnHandClicado()
    {
        foreach (Collider2D t in collidedObjects)
        {
            if (t.tag == "CFNot")
            {
                if (!t.GetComponent<Not>().travado)
                {
                    if (t.GetComponent<Not>().status)
                    {
                        countNot++;
                        t.GetComponent<Not>().status = false;
                        vidas--;
                        tomouHit();
                    }
                }
            }
            if (t.tag == "CFOperador")
            {
                if (!t.GetComponent<Operador>().travado)
                {
                    if (t.GetComponent<Operador>().operador == "and")
                    {
                        countAnd++;
                        t.GetComponent<Operador>().operador = "none";
                        vidas--;
                        tomouHit();
                    }
                    else if (t.GetComponent<Operador>().operador == "or")
                    {
                        countOr++;
                        t.GetComponent<Operador>().operador = "none";
                        vidas--;
                        tomouHit();
                    }
                }
            }
        }
        atualizaCanvasOperador();
    }

    public void circuitoAtivo()
    {
        if ((this.GetComponent<Transform>().position.x) <= MIN_RR.GetComponent<Transform>().position.x)
        {
            this.GetComponent<Transform>().position = new Vector3(MIN_RR.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y, this.GetComponent<Transform>().position.z);
        }
    }

    void atualizaCanvasHealth()
    {
        if (vidas == 2)
        {
            getChildGameObject(canvasHealth, "Health3").GetComponent<Coracao>().setaZero();
        }
        else if (vidas == 1)
        {
            getChildGameObject(canvasHealth, "Health2").GetComponent<Coracao>().setaZero();
        }
        else if (vidas == 0 && !morto)
        {
            getChildGameObject(canvasHealth, "Health1").GetComponent<Coracao>().setaZero();
            morreu();
        }
    }

    void atualizaDificuldade()
    {
        if (int.Parse(score.GetComponent<Text>().text) >= 10)
        {
            dificuldadeEasy = false;
        }
        else
        {
            dificuldadeEasy = true;
        }
    }

    void morreu()
    {
        if (!morto)
        {
            morto = true;
            anim.SetTrigger("Die");
            tomouHit();
            animacaoMorte();
        }
        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -15);

        //restartLevel();
    }

    void animacaoMorte()
    {
        canvasMorto.SetActive(true);
        StartCoroutine(movimentacaoMorte());
        this.GetComponent<Rigidbody2D>().gravityScale = 0.7f;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    IEnumerator movimentacaoMorte()
    {
        /*Vector3 temp;
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(0.001f);
            temp = new Vector3(0, 0.1f, 0);
            this.transform.position += temp;
        }
        */
        yield return new WaitForSeconds(0.001f);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
    }
}
