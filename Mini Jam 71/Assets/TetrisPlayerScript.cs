using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisPlayerScript : MonoBehaviour
{
    //Sounds
    public AudioSource[] poundSounds = new AudioSource[4];
    public AudioSource jumpSound;
    public AudioSource switchSound;


    //Tetris
    public GameObject[] TetrisPieces = new GameObject[7];

    private float poundCooldown;
    private float switchCooldown;
    public float switchExplosionForce;
    public float poundExplosionForce;

    //Player
    private float horizontalMove = 0f;
    private float horizontalRotate = 0f;
    private float rotationSpeed = 1.5f;
    public float rotationOffset = 0f;
    private bool jump = false;
    private bool rotate = false;
    private bool rotate2 = false;
    private bool pound = false;
    private bool groundedSoundBool = false;
    public float runSpeed = 28f;
    public Rigidbody2D rb;

    //Controller
    private float initForce;
    private float groundedVal;
    public float jumpForce = 48f;
    private bool facingRight = true;
    public LayerMask whatIsGround;

    public Transform currentGroundCheck;
    private bool grounded;
    public float groundedRadius = .1f;

    public GameObject cam;
    private GameObject target;
    public Vector2 offset;

    private void Start()
    {
        initForce = jumpForce;

        target = TetrisPieces[0];
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        horizontalRotate = Input.GetAxisRaw("Horizontal") * runSpeed/2;

        if (Input.GetButtonDown("Rotate"))
        {
            rotate = true;
            rotate2 = false;
        }
        if (Input.GetButtonUp("Rotate"))
        {
            rotate = false;
            rotate2 = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (jump == false)
            {
                StartCoroutine(waitJump(.15f));
                jump = true;
                pound = false;
            }
        }
        else
        {
            if (Input.GetButtonDown("Pound"))
            {
                if (pound == false)
                {
                    StartCoroutine(waitPound(.15f));
                    pound = true;
                    jump = false;
                }
            }
        }

        if (Input.GetButtonDown("Menu"))
        {
            SceneManager.LoadScene(1);
        }
    }
    void FixedUpdate()
    {
        //Rotation Offset
        {
            if (rotate)
            {
                rotationOffset += 5f;
            }
            else if (rotate2)
            {
                rotationOffset = (float)(Math.Round(rotationOffset / 90) * 90);
                rotate2 = false;
            }
        }

        //Pounding/Switching
        {
            switchCooldown -= 0.1f;
            poundCooldown -= 0.1f;

            if (pound)
            {
                //Pound
                if (poundCooldown < 0)
                {
                    if (grounded == false)
                    {
                        poundCooldown = 1;
                        pound = false;
                        jump = false;
                        rb.velocity += new Vector2(0, -poundExplosionForce);
                        AudioSource poundSound = new AudioSource();
                        poundSound = poundSounds[UnityEngine.Random.Range(0, 4)];
                        poundSound.Play();
                    }
                }

                //Switch
                if (switchCooldown < 0)
                {
                    if (grounded == true)
                    {
                        groundedVal = 0;
                        switchCooldown = 1;
                        groundedSoundBool = true;
                        pound = false;
                        jump = false;
                        rotationSpeed = 2.5f;
                        Switch();
                        switchSound.Play();
                    }
                }
            }
        }

        //Grounding
        {
            bool wasGrounded = grounded;
            groundedVal -= 0.1f;

            if (groundedVal < 0)
            {
                grounded = false;
            }
            else
            {
                grounded = true;
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(currentGroundCheck.position, groundedRadius, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    groundedVal = .15f;
                    jumpForce = initForce;
                    rotationSpeed = 1.5f;

                    if (groundedSoundBool == true) 
                    {
                        groundedSoundBool = false;
                    }
                }
            }
        }

        //Moving
        {
            float move = horizontalMove * Time.fixedDeltaTime;
            Vector3 targetVelocity = new Vector2(transform.right.x * move * 10f, rb.velocity.y);
            rb.velocity = targetVelocity;
            rb.MoveRotation((horizontalRotate * rotationSpeed) + rotationOffset);

            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }

            if (grounded && jump)
            {
                groundedVal = 0;
                rb.velocity += new Vector2(0, jumpForce);
                groundedSoundBool = true;
                jump = false;
                rotationSpeed = 2.5f;
                jumpSound.Play();
            }
        }

        //Camera
        Vector2 desiredPosition = target.transform.position + (Vector3)offset;
        Vector2 smoothedPosition = Vector2.Lerp(cam.transform.position, desiredPosition, 0.125f);
        cam.transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, cam.transform.position.z);
    }
    private IEnumerator waitJump(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        jump = false;
    }

    private IEnumerator waitPound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        pound = false;
    }
    private void Flip()
    {
        facingRight = !facingRight;

        for (int i = 0; i < TetrisPieces.Length; i++)
        {
            Vector3 theScale = TetrisPieces[i].transform.localScale;
            theScale.x *= -1;
            TetrisPieces[i].transform.localScale = theScale;
        }
    }

    void Switch()
    {
        int caseSwitch = 0;

        GameObject _block = TetrisPieces[0].gameObject;
        GameObject _l1 = TetrisPieces[1].gameObject;
        GameObject _long = TetrisPieces[2].gameObject;
        GameObject _z1 = TetrisPieces[3].gameObject;
        GameObject _l2 = TetrisPieces[4].gameObject;

        if (_block.activeSelf)
        {
            caseSwitch = 1;
        }
        if (_l1.activeSelf)
        {
            caseSwitch = 2;
        }
        if (_long.activeSelf)
        {
            caseSwitch = 3;
        }
        if (_z1.activeSelf)
        {
            caseSwitch = 4;
        }
        if (_l2.activeSelf)
        {
            caseSwitch = 5;
        }

        switch (caseSwitch)
        {
            case 1:
                Vector2 currentPosition1 = new Vector2();

                currentPosition1 = _block.transform.position;

                _block.SetActive(false);
                _l1.SetActive(true);
                _long.SetActive(false);
                _z1.SetActive(false);
                _l2.SetActive(false);

                rotationOffset = 0;
                rb = _l1.GetComponent<Rigidbody2D>();
                rb.SetRotation(0);

                _l1.transform.position = currentPosition1;

                currentGroundCheck = GameObject.FindGameObjectWithTag("GroundCheck").transform;

                rb.velocity += new Vector2(0, switchExplosionForce);

                target = _l1;
                return;
            case 2:
                Vector2 currentPosition2 = new Vector2();

                currentPosition2 = _l1.transform.position;

                _block.SetActive(false);
                _l1.SetActive(false);
                _long.SetActive(true);
                _z1.SetActive(false);
                _l2.SetActive(false);

                rotationOffset = 0;
                rb = _long.GetComponent<Rigidbody2D>();
                rb.SetRotation(0);

                _long.transform.position = currentPosition2;

                currentGroundCheck = GameObject.FindGameObjectWithTag("GroundCheck").transform;

                rb.velocity += new Vector2(0, switchExplosionForce);

                target = _long;
                return;
            case 3:
                Vector2 currentPosition3 = new Vector2();

                currentPosition3 = _long.transform.position;

                _block.SetActive(false);
                _l1.SetActive(false);
                _long.SetActive(false);
                _z1.SetActive(true);
                _l2.SetActive(false);

                rotationOffset = 0;
                rb = _z1.GetComponent<Rigidbody2D>();
                rb.SetRotation(0);

                _z1.transform.position = currentPosition3;

                currentGroundCheck = GameObject.FindGameObjectWithTag("GroundCheck").transform;

                rb.velocity += new Vector2(0, switchExplosionForce);

                target = _z1;
                return;
            case 4:
                Vector2 currentPosition4 = new Vector2();

                currentPosition4 = _z1.transform.position;

                _block.SetActive(false);
                _l1.SetActive(false);
                _long.SetActive(false);
                _z1.SetActive(false);
                _l2.SetActive(true);

                rotationOffset = 0;
                rb = _l2.GetComponent<Rigidbody2D>();
                rb.SetRotation(0);

                _l2.transform.position = currentPosition4;

                currentGroundCheck = GameObject.FindGameObjectWithTag("GroundCheck").transform;

                rb.velocity += new Vector2(0, switchExplosionForce);

                target = _l2;
                return;
            case 5:
                Vector2 currentPosition5 = new Vector2();

                currentPosition5 = _l2.transform.position;

                _block.SetActive(true);
                _l1.SetActive(false);
                _long.SetActive(false);
                _z1.SetActive(false);
                _l2.SetActive(false);

                rotationOffset = 0;
                rb = _block.GetComponent<Rigidbody2D>();
                rb.SetRotation(0);

                _block.transform.position = currentPosition5;

                currentGroundCheck = GameObject.FindGameObjectWithTag("GroundCheck").transform;

                rb.velocity += new Vector2(0, switchExplosionForce);

                target = _block;
                return;
        }
    }

}
