using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Char : MonoBehaviour
{
    private bool andar;
    private bool morto;
    private float movimentoX;
    public Animator anime;
    public Rigidbody2D RbPlayer;
    public float maxSpeed;
    public float jumpForce;
    private bool facingRight;
    public bool isDoubleJump;
    public Transform groundCheck;
    private bool grounded; //Checa se está no chao
    private bool lastGrounded; //Auxiliar para checar a ultima posição.
    public LayerMask whatIsGround;
    private bool wallCheck; //Checa parede
    public float addSpeed;
    private float walkSpeed;
    public BoxCollider2D bc;
    private int pontuacao;
    public GameObject canvasMorto;
    public UnityEngine.UI.Text txtPontos;

    //AUDIO
    public AudioSource audioSource; //O que gera som
    public AudioSource audioWalk; //O que gera som
    public AudioClip soundJump; //Som do pulo
    public AudioClip soundBounce; //Som da caída
    public AudioClip soundRun; //Som da corrida
    public AudioClip soundIdle; //Som da caída
    public AudioClip soundTrap; //Som da armadilha
    public AudioClip soundDeadByLava; //Som da armadilha
    public AudioClip soundFaliceu; //Som da morte
    public AudioClip soundDeadByAgua; //Som da morte
    public AudioClip soundPiranha; //Som da piranha

    private GameObject objetointeracao;

    // Use this for initialization
    void Start()
    {
        movimentoX = 5;
        maxSpeed = 5;
        jumpForce = 350;
        morto = false;
        andar = false;
        facingRight = true;
        isDoubleJump = false;
        grounded = false;
        walkSpeed = maxSpeed;
        lastGrounded = false;
        audioWalk.pitch = 0.8f; //Seta passos um pouco mais lentos
        canvasMorto.SetActive(false);
        pontuacao = 0;
    }

    // Update is called once per frame
    void Update()
    {
        txtPontos.text = pontuacao.ToString();
        if (morto)
        {
            dead();
        }
        else
        {
            anime.SetBool("walk", andar);
            anime.SetBool("grounded", grounded);
            anime.SetFloat("speedY", RbPlayer.velocity.y);
            //anime.SetBool ("jump", pular);
            grounded = Physics2D.OverlapCircle(groundCheck.position, 0.3f, whatIsGround);
            if (grounded && !lastGrounded)
            {
                if (isDoubleJump)
                {
                    audioSource.PlayOneShot(soundBounce, 1);
                }
                else
                {
                    audioSource.PlayOneShot(soundBounce, 0.5f);
                }
                isDoubleJump = false;
            }
            lastGrounded = grounded;
            movimentoX = Input.GetAxis("Horizontal");
            if (movimentoX != 0)
            {
                andar = true;
            }
            else
            {
                andar = false;
            }

            if (andar && grounded && !morto)
            {
                if (!audioWalk.isPlaying && grounded && !morto)
                {
                    audioWalk.Play();
                }
            }
            else
            {
                audioWalk.Stop();
            }
            if (movimentoX > 0 && !facingRight)
            {
                Flip();
            }
            else if (movimentoX < 0 && facingRight)
            {
                Flip();
            }

            if (Input.GetButtonDown("Jump") && grounded)
            {
                RbPlayer.AddForce(new Vector2(0, jumpForce));
                RbPlayer.velocity = new Vector2(0, 0);
                audioSource.volume = 1;
                audioSource.PlayOneShot(soundJump, 0.4f);
            }
            else if (Input.GetButtonDown("Jump") && !grounded)
            {
                if (!isDoubleJump)
                {
                    RbPlayer.AddForce(new Vector2(0, jumpForce));
                    RbPlayer.velocity = new Vector2(0, 0);
                    isDoubleJump = !isDoubleJump;
                    audioSource.PlayOneShot(soundJump, 0.4f);
                    //soundController.playSound(soundFx.JUMP);
                }
            }

            if (wallCheck == false)
            {
                RbPlayer.velocity = new Vector2(movimentoX * walkSpeed, RbPlayer.velocity.y);
            }

            if (Input.GetButtonDown("Fire1") && objetointeracao != null)
            {
                passafase();
                //soundController.playSound(soundFx.OPEN);
            }
            

            /*if (Input.GetButtonUp("Fire1"))
            {
                walkSpeed = maxSpeed;
            }*/ 
        }
        

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void audioSpin()
    {
        audioSource.PlayOneShot(soundIdle);
    }

    void passafase()
    {
        if (pontuacao >= 4)
        {
            if (SceneManager.GetActiveScene().buildIndex == 4){
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        else
        {
            morto = true;
            anime.SetBool("dead", true);
            anime.SetBool("grounded", true);
            audioSource.PlayOneShot(soundFaliceu);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag != "gemas" && col.tag != "gatilho" && col.tag != "plataforma" && col.tag != "AjusteCamera" && col.tag != "interacao" && col.tag != "coletavel")
        {
            wallCheck = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "gemas")
        {
            isDoubleJump = false;
            col.gameObject.SetActive(false);
            /*StartCoroutine(contadorGemas(col));*/
            //col.gameObject.SetActive (true);
        }
        /*
        if (col.tag == "AjusteCamera")
        {
            cam.ajusteY = 0;
        }*/

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
    }

    void dead()
    {
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            anime.SetBool("dead", false);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -15);
            canvasMorto.SetActive(true);
        }
        Vector3 temp = new Vector3(0, 0.13f, 0);
        this.transform.position += temp;
        RbPlayer.gravityScale = 0;
        RbPlayer.velocity = new Vector2(0, 0);
        bc.isTrigger = true;
        //restartLevel();
    }

    void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }   

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag != "gemas")
        {
            wallCheck = false;
        }
        /*
        if (col.tag == "AjusteCamera")
        {
            cam.ajusteY = 4;
        }*/
        if (col.tag == "interacao")
        {
            objetointeracao = null;
        }
    }

    /*IEnumerator contadorGemas(Collider2D col)
    {
        yield return new WaitForSeconds(1);
        col.gameObject.SetActive(true);
    }*/

    IEnumerator contadorSegundos()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(soundFaliceu);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "plataformamovel")
        {
            transform.SetParent(col.gameObject.transform);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "plataformamovel")
        {
            transform.SetParent(null);
        }
    }






}