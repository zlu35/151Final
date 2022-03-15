using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float runSpeed;
    public float jumpSpeed;
    public GameObject DeathUI;
    public GameObject VictoryUI;

    public GameObject me;
    private Transform myTransform;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private CapsuleCollider2D myBody;
    private BoxCollider2D myFeet;

    public bool isGround;
    public bool isHit;
    public bool isWin;
    // Start is called before the first frame update
    void Start()
    {
        DeathUI.SetActive(false);
        VictoryUI.SetActive(false);
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        runSpeed = 5.0f;
        jumpSpeed = 5.0f;
        isWin = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (myTransform.position.y < 0)
        {
            Die();
        }
        Run();
        Flip();
        GroundCheck();
        HitCheck();
        WinCheck();

    }

    void Flip()
    {
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasXAxisSpeed)
        {
            if (myRigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0,0,0);
            }
            if (myRigidbody.velocity.x < 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0,180,0);
            }            
        }
    }

    void Run()
    {
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir *runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVel;
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("run", playerHasXAxisSpeed);
    }

    void GroundCheck()
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        // if (isGround){
        //     myAnimator.SetBool("jump", false);
        // }
        if(Input.GetButtonDown("Jump"))//Set to "space" in Edit > Project Settings > Input Manager
        {
            if (isGround)
            {
                Jump();
            }
        }
    }

    void Jump()
    {    
        Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
        myRigidbody.velocity = Vector2.up * jumpVel;
        // myAnimator.SetBool("jump", true);
    }

    void HitCheck()
    {
        // Debug.Log(myBody.IsTouchingLayers(LayerMask.GetMask("Enemy")));
        isHit = myBody.IsTouchingLayers(LayerMask.GetMask("Enemy"));
        if(isHit)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(me);
        DeathUI.SetActive(true);
    }

    void WinCheck()
    {
        if(myBody.IsTouchingLayers(LayerMask.GetMask("LootBox")))
        {
            isWin = true;
        }
        if(isWin){
            Victory();
        }
    }

    void Victory()
    {   
        Destroy(me);
        VictoryUI.SetActive(true);
    }
}
