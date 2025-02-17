using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public player player;
    public SCORE scoremanager;
    private Rigidbody2D rb;
    private Vector2 shootforce;
   // private Vector2 winddirection;
    //public Transform wormholeIn;  
    //public Transform wormholeOut;
    //private float windstrength;
    //private Vector2 windforce; // Declare windforce
    //public GameObject winddir;
    //private RectTransform windTransform;
    
    // private int flickcounter=0;
    // private bool isVisible=true;
    public blockmanager Blockmanager;
    public bool gamenotover=true;
    private SpriteRenderer ballrenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.position = new Vector2(-7.5f, 2.5f);
        scoremanager = FindObjectOfType<SCORE>();
        PhysicsMaterial2D ballMaterial = new PhysicsMaterial2D();
        ballrenderer=GetComponent<SpriteRenderer>();
        //windTransform = winddir.GetComponent<RectTransform>();
        //ChangeWind();
        
        StartCoroutine(BallInAir());
       
    }


    //void ChangeWind()//call whenever start new level
    //{
       
    //    winddirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    //    windstrength = 5f; 
    //    // Calculate wind angle using Atan2
    //    float windAngle = Mathf.Atan2(winddirection.y, winddirection.x) * Mathf.Rad2Deg;
    //    windTransform.rotation = Quaternion.Euler(0, 0, windAngle);
    //    scoremanager.nextlevel=false;

    //}

    //IEnumerator Fickerball(){
    //    while(gamenotover){
    //       ballrenderer.enabled=true;
    //       yield return new WaitForSecondsRealtime(0.2f);
    //       ballrenderer.enabled=false;
    //       yield return new WaitForSecondsRealtime(0.5f);
    //    }
    //}
    /*
    void Update(){
        if(scoremanager.nextlevel==true){
            ChangeWind();
        }
    }
    */

    void FixedUpdate()
    {
        //windforce = new Vector2(winddirection.x * windstrength, winddirection.y * windstrength);
        //rb.AddForce(windforce, ForceMode2D.Force);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("netbottom"))
        {
            ballrenderer.sortingOrder += 1;
            Debug.Log("sorting order increased "+ballrenderer.sortingOrder);
        

        }
    }
    void OnTriggerEnter2D(Collider2D collision)//for those checked istrigger
    {
        // if (collision.gameObject.CompareTag("nettop"))
        // {

        //     scoremanager.AddScore1(1);
        //     ballrenderer.sortingOrder -= 1;
        //     Debug.Log("sorting order decreased " + ballrenderer.sortingOrder);

        //     rb.velocity = rb.velocity * 0.5f; 
        // }
        //if(collision.gameObject.CompareTag("wormholein"))
        //{
        //    //transform.position=wormholeOut.position;
        //}
        if(collision.gameObject.CompareTag("hoop")){
            if(scoremanager.round==1){
                scoremanager.round=2;
                scoremanager.checknextlevel();
                //reset player and ball position
                player.transform.position =new Vector2(-7.5f, -3.5f);
                rb.position = new Vector2(-7.5f, 2.5f);
                rb.velocity=Vector2.zero;
                
            }


            else if(scoremanager.round==2){
                scoremanager.nextlevel=false;
                scoremanager.round=3;
                Blockmanager.firstgenblock=true;
                scoremanager.checknextlevel();
                rb.velocity=Vector2.zero;
                player.transform.position =new Vector2(-7.5f, -3.5f);
                rb.position = new Vector2(-7.4f, -2.5f);
            }
            else if(scoremanager.round==3){
                scoremanager.nextlevel=false;
                scoremanager.round=4;
                scoremanager.checknextlevel();
            }
        }
        
        
        
    }
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with Player!");
            attachplayer();
        }
       
        else if(collision.gameObject.CompareTag("spike"))
        {
            Debug.Log("Game over!");
            gamenotover=false;
            StopCoroutine("Fickerball"); 
            ballrenderer.enabled=true;
            scoremanager.roundText.gameObject.SetActive(true);
            scoremanager.roundText.text="Game Over :(";
            Time.timeScale=0;
        }
        
       
        
    }
    void attachplayer(){
        rb.velocity = Vector2.zero; // Stop ball velocity
        rb.angularVelocity = 0f;
        rb.transform.rotation = Quaternion.identity; // Set the ball rotation to (0,0,0)
        rb.isKinematic = true; // Disable physics
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
        if (player == null)
        {
            player = FindObjectOfType<player>(); // 
        }
        
        transform.SetParent(player.transform); // Attach ball to player
        player.shootstep=1;
        // if(scoremanager.getScore() >= 3)
        // {
        //     player.speed = 0f;
        // }
        Debug.Log("attach to parent");
        transform.localPosition = new Vector2(1.1f, 1.1f); // Adjust position relative to player
    }

    public void ReleaseBall(float strength,float angle)
    {
        player.shootstep=0;
        rb.isKinematic=false;
        transform.SetParent(null);
       
        Debug.Log("fly out of parent");
        StartCoroutine(enablecollide());
        shootforce = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * strength;
        rb.AddForce(shootforce,ForceMode2D.Impulse);
        player.speed=5f;
        Debug.Log("Ball Shot! Angle: " + angle + " Strength: " + strength);
       
    }
    IEnumerator BallInAir()
    {
        while (true) 
        {
            if (rb.transform.parent == null) // Not attached to parent
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    player.transform.position = rb.transform.position;
                    Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                    Debug.Log("BALL VELOCITY:"+rb.velocity);
                    playerRb.velocity = new Vector2(0, rb.velocity.y);
                    
                    yield return null;
                }
            }
            yield return null; // Pause execution until the next frame
        }
    }

    IEnumerator enablecollide(){
        yield return new WaitForSecondsRealtime(0.1f);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
    }
}
