using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{

    float horizontalMove;
    public float speed;

    Rigidbody2D myBody;

    bool grounded = false;

    public float castDist = 1f;
    public float gravityScale = 1.6f;
    public float gravityFall = 2f;
    public float jumpLimit = 7f;

    bool jump = false;
    public float jumpnumber = 0;

    Animator myAnim;
    SpriteRenderer myRend;

    public GameObject signText;

    bool passedfirstlevel = false;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myRend = GetComponent<SpriteRenderer>();

        signText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //horizontal movement controls
        horizontalMove = Input.GetAxis("Horizontal");

        //jump controls
        if (Input.GetButtonDown("Jump") && jumpnumber <= 1)
        {
            jump = true;
        }

        if (horizontalMove > 0.2f)
        {
            myAnim.SetBool("walking", true);
            myRend.flipX = false;
        }
        else if (horizontalMove < -0.2f)
        {
            myAnim.SetBool("walking", true);
            myRend.flipX = true;
        }
        else
        {
            myAnim.SetBool("walking", false);
        }
    }

    //runs independent of frames
    void FixedUpdate()
    {
        float moveSpeed = horizontalMove * speed;

        //if jump is triggered, then make player jump
        if (jump)
        {
            grounded = false;
            myBody.AddForce(Vector2.up * jumpLimit, ForceMode2D.Impulse);
            jumpnumber += 1;
            jump = false;
        }

        if (grounded)
        {
            jumpnumber = 0;
        }

        //gravity is less when player is moving upwards
        if (myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale;
            myAnim.SetBool("jumping", true);
        }
        else if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
            myAnim.SetBool("jumping", true);
        }
        else
        {
            myAnim.SetBool("jumping", false);
        }

        //making raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);
        //draw the raycast in unity
        Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red);

        //if raycast hits ground...
        if (hit.collider != null && hit.transform.name == "Ground")
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Enemy")
        {
            SceneManager.LoadScene(3);
        }
        if (other.gameObject.name == "BottomCollider")
        {
            SceneManager.LoadScene(3);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Sign")
        {
            signText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Sign")
        {
            signText.SetActive(false);
        }
    }
}
