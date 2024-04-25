using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MouseController : MonoBehaviour
{
    private GeneratorScript generator;

    public float jetpackForce;
    public ParticleSystem jetpack;

    public float forwardMovementSpeed;

    private Rigidbody2D rb;

    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    private bool grounded;
    private Animator animator;

    private bool dead = false;

    private uint coins = 0;
    private uint level = 0;
    public TMP_Text textCoins;
    public TMP_Text textLevel;

    public Button buttonRestart;
    public Button buttonToMenu;

    public AudioClip coinCollectSound;
    public AudioSource jetpackAudio;
    public AudioSource footstepAudio;

    public ParallaxScroll parallaxScroll;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        generator = GetComponent<GeneratorScript>();

        textCoins.text = coins.ToString();
        textLevel.text = "Level : " + level;

        StartCoroutine(LevelCount());
        StartCoroutine(FeverStart());
    }

    private void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");

        if (jetpackActive && !dead)
        {
            rb.AddForce(jetpackForce * Vector2.up);
        }

        if (!dead)
        {
            Vector2 newVelocity = rb.velocity;
            newVelocity.x = forwardMovementSpeed;
            rb.velocity = newVelocity;
        }

        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);
        DisplayGameOverButton();
        AdjustFootstepsAndJetpackSound(jetpackActive);

        parallaxScroll.offset = transform.position.x;
    }
    private void AdjustJetpack(bool jetpackActive)
    {
        var emission = jetpack.emission;
        emission.enabled = !grounded;
        emission.rateOverTime = jetpackActive ? 300f : 75f;
    }

    private void UpdateGroundedStatus()
    {
        grounded = Physics2D.OverlapCircle(
            groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        animator.SetBool("grounded", grounded);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coins")
            CollectCoin(collision);
        else
            HitByLaser(collision);
    }

    private void CollectCoin(Collider2D coinCollider)
    {
        ++coins;
        textCoins.text = coins.ToString();

        Destroy(coinCollider.gameObject);

        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
    }

    private void LevelUp()
    {
        level++;
        textLevel.text = "Level : " + level;
        forwardMovementSpeed += 0.2f;
    }

    private void HitByLaser(Collider2D laserCollider)
    {
        if (!dead)
        {
            AudioSource laser = laserCollider.GetComponent<AudioSource>();
            laser.Play();
        }

        dead = true;
        animator.SetBool("dead", true);
    }

    private void DisplayGameOverButton()
    {
        bool reActive = buttonRestart.gameObject.activeSelf;
        bool menuActive = buttonToMenu.gameObject.activeSelf;

        if (grounded && dead && !reActive && !menuActive)
        {
            buttonRestart.gameObject.SetActive(true);
            buttonToMenu.gameObject.SetActive(true);
        }
    }

    public void OnClickedRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickedToMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    private void AdjustFootstepsAndJetpackSound(bool jetpackActive)
    {
        footstepAudio.enabled = !dead && grounded;
        jetpackAudio.enabled = !dead && !grounded;
        jetpackAudio.volume = jetpackActive ? 1f : 0.5f;
    }

    IEnumerator FeverStart()
    {
        while (true)
        {
            bool fever = false;
            yield return new WaitForSeconds(60f);

            if (!dead)
            {   
                generator.range--;
                forwardMovementSpeed *= 2;
                fever = true;
            }

            yield return new WaitForSeconds(5f);

            if (fever)
            {
                generator.range++;
                forwardMovementSpeed /= 2;
            }
        }
    }

    IEnumerator LevelCount()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);

            if (!dead)
                LevelUp();
        }
    }
}

