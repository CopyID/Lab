using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private AudioSource playerAudio;
    public ParticleSystem deathParticle;
    public AudioClip groundSound;
    public AudioClip deathSound;
    public float jumpForce = 10;
    public float gravityModifier = 3;
    public float horizontalInput;
    public float speed = 7.0f;
    private float zBound = 0;
    private float rotation = 0;
    public bool isOnGround = true;
    public bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        
        Bounds();
    }

    // Sees if the player is colliding with an object.
    private void OnCollisionEnter(Collision collision)
    {
        // Sees if the player is colliding with the ground.
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            playerAudio.PlayOneShot(groundSound, 1);
        } else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
            gameOver = true;
            deathParticle.Play();
            playerAudio.PlayOneShot(deathSound, 1.2f);
        }
    }

    private void Movement()
    {
        // Lets the player jump when on the ground.
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }
        
        if (!gameOver)
        {
            // Lets the player move left and right.
            horizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
        }
}

    private void Bounds()
    {
        // Keeps the player in bounds and prevents them from rotating
        transform.position = new Vector3(transform.position.x, transform.position.y,zBound);
        transform.rotation = new Quaternion(rotation, rotation, rotation, rotation);
    }
}
