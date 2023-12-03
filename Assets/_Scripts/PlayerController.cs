using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public GameObject farmerModel;
    public GameObject backGround;
    public GameObject rulePanel;
    public GameObject scoreLifePanel;
    public GameObject end;
    public GameObject start;
    public GameObject footSteps;

    public float speed;
    public float jumpForce;
    private float xRange = 15f;

    private Animator playerAnim;
    private Animator farmerAnim;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI yourScore;
    public Button restartButton;

    private AudioSource playerAudio;
    private AudioSource footStepsAudio;

    public AudioClip jumpAudio;
    public AudioClip scoreAudio;
    public AudioClip lifeGainedAudio;
    public AudioClip crashAudio;

    public ParticleSystem collisionParticle;
    public ParticleSystem runningParticle;
    public ParticleSystem scoreParticle;
    public ParticleSystem lifeGainedParticle;

    private int lifeCount;
    private int score;

    public bool isOnGround = true;
    public bool isGameActive = true;
    public bool isIdle;
    public bool startAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        lifeCount = 1;
        score = 0;

        Physics.gravity = new Vector3(0, -20f, 0);

        playerAnim = GetComponentInChildren<Animator>();
        farmerAnim = farmerModel.GetComponent<Animator>();

        playerAudio = GetComponent<AudioSource>();
        footStepsAudio = footSteps.GetComponent<AudioSource>();

        restartButton.onClick.AddListener(RestartGame);

        isIdle = true;
        footStepsAudio.mute = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerConstraints();
    }

    void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.A) && startAudio == true)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
            playerAnim.SetBool("Left", true);
            playerAnim.SetBool("Right", false);
            playerAnim.SetBool("Idle", false);
            playerAnim.SetInteger("Confirm_int", 2);
            farmerAnim.SetFloat("Speed_f", 1);
            farmerAnim.SetBool("Static_b", true);

            runningParticle.transform.Rotate(Vector3.up,0f);
            runningParticle.gameObject.SetActive(true);

            footStepsAudio.mute = false;

            isIdle = false;
        }

        if (Input.GetKey(KeyCode.D) && startAudio == true)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
            playerAnim.SetBool("Right", true);
            playerAnim.SetBool("Left", false);
            playerAnim.SetBool("Idle", false);
            playerAnim.SetInteger("Confirm_int", 1);
            farmerAnim.SetFloat("Speed_f", 1);
            farmerAnim.SetBool("Static_b", true);

            runningParticle.transform.Rotate(Vector3.up, 180f);
            runningParticle.gameObject.SetActive(true);

            footStepsAudio.mute = false;

            isIdle = false;
        }

        if (Input.GetKeyUp(KeyCode.A) && startAudio == true)
        {
            farmerAnim.SetFloat("Speed_f", 0.2f);
            playerAnim.SetBool("Idle", true);
            playerAnim.SetInteger("Confirm_int", 2);
            footStepsAudio.mute = true;
            isIdle = true;
        }

        if (Input.GetKeyUp(KeyCode.D) && startAudio == true)
        {
            farmerAnim.SetFloat("Speed_f", 0.2f);
            playerAnim.SetBool("Idle", true);
            playerAnim.SetInteger("Confirm_int", 1);
            footStepsAudio.mute = true;
            isIdle = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && startAudio == true)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            farmerAnim.SetBool("Jump_b", true);
            runningParticle.Stop();
            playerAudio.PlayOneShot(jumpAudio, 1f);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            farmerAnim.SetBool("Jump_b", false);       
        }

        if (isIdle == true)
        {
            runningParticle.gameObject.SetActive(false);
        }

        if (isOnGround == false)
        {
            runningParticle.gameObject.SetActive(false);
        }

        if (isOnGround == false)
        {
            footStepsAudio.mute = true;
        }

        if (startAudio == false)
        {
            footStepsAudio.mute = true;
        }

    }
    void PlayerConstraints()
    {

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            runningParticle.Play();
        }

        if (collision.gameObject.CompareTag("Cow") && lifeCount >= 0)
        {
            lifeCount = lifeCount - 1;
            lifeText.text = "Lives Left: " + lifeCount;
            Destroy(collision.gameObject);

            playerAudio.PlayOneShot(crashAudio);
            collisionParticle.Play();
            
        }
        if (collision.gameObject.CompareTag("Cow") && lifeCount < 0)
        {
            Debug.Log("GAME OVER!");           
            Destroy(collision.gameObject);
            isGameActive = false;
            GameOver();

            playerAudio.PlayOneShot(crashAudio);         
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Egg"))
        {
            Destroy(other.gameObject);
            score += 30;
            scoreText.text = "  Score: " + score;
            Debug.Log("+1 POINT!");

            playerAudio.PlayOneShot(scoreAudio, 1f);
            scoreParticle.Play();
        }

        if (other.gameObject.CompareTag("Poop") && lifeCount >= 0)
        {
            Destroy(other.gameObject);
            Debug.Log("-1 Life");
            lifeCount = lifeCount - 1;
            lifeText.text = "Lives Left: " + lifeCount;

            collisionParticle.Play();
            playerAudio.PlayOneShot(crashAudio, 1f);
        }

        if (other.gameObject.CompareTag("Poop") && lifeCount < 0)
        {
            Debug.Log("Game Over");        
            Destroy(other.gameObject);
            isGameActive = false;
            GameOver();

            playerAudio.PlayOneShot(crashAudio, 1f);
        }

        if (other.gameObject.CompareTag("ExtraLife"))
        {
            Destroy(other.gameObject);
            Debug.Log("+1 LIFE!");
            lifeCount = lifeCount + 1;
            lifeText.text = "Lives Left: " + lifeCount;

            lifeGainedParticle.Play();
            playerAudio.PlayOneShot(lifeGainedAudio, 1f);
        }
    }

    public void GameOver()
    {
        yourScore.text = "Your Score: " + score;

        end.gameObject.SetActive(true);
        backGround.gameObject.SetActive(true);

        scoreLifePanel.gameObject.SetActive(false);

        startAudio = false;
    }

     void RestartGame()
     {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
     }
}
