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
    private Vector2 winddirection;
    public Transform wormholeIn;  
    public Transform wormholeOut;
    private float windstrength;
    private Vector2 windforce; // Declare windforce
    public GameObject winddir;
    private RectTransform windTransform;
    private int flickcounter=0;
    private bool isVisible=true;
    public bool gamenotover=true;
    private SpriteRenderer ballrenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.position = new Vector2(-3, 2.5f);
        scoremanager = FindObjectOfType<SCORE>();
        PhysicsMaterial2D ballMaterial = new PhysicsMaterial2D();
        ballrenderer=GetComponent<SpriteRenderer>();
        windTransform = winddir.GetComponent<RectTransform>();
        ChangeWind();
        
        StartCoroutine(Fickerball());
       
    }


    void ChangeWind()//call whenever start new level
    {
       
        winddirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        windstrength = 5f; 
        // Calculate wind angle using Atan2
        float windAngle = Mathf.Atan2(winddirection.y, winddirection.x) * Mathf.Rad2Deg;
        windTransform.rotation = Quaternion.Euler(0, 0, windAngle);
        scoremanager.nextlevel=false;

    }

    IEnumerator Fickerball(){
        while(gamenotover){
           ballrenderer.enabled=true;
           yield return new WaitForSecondsRealtime(0.2f);
           ballrenderer.enabled=false;
           yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    /*
    void Update(){
        if(scoremanager.nextlevel==true){
            ChangeWind();
        }
    }
    */

    void FixedUpdate()
    {
        windforce = new Vector2(winddirection.x * windstrength, winddirection.y * windstrength);
        rb.AddForce(windforce, ForceMode2D.Force);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("net"))
        {
            scoremanager.AddScore1(1);
            rb.velocity = rb.velocity * 0.5f; 
        }
        if(collision.gameObject.CompareTag("wormholein"))
        {
            transform.position=wormholeOut.position;
        }
        if(collision.gameObject.CompareTag("spike"))
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
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with Player!");
            attachplayer();
        }
        else if (collision.gameObject.CompareTag("block")){
            Debug.Log("add force to prevent stuck on the block");
            rb.velocity = new Vector2(5f, rb.velocity.y + 1f);
        }
        /*
        else if (collision.gameObject.CompareTag("hoop")){
            Debug.Log("ouch is bar");
            rb.velocity=new Vector2(rb.velocity.x,-rb.velocity.y*0.5f);
        }
        */
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
        
        Debug.Log("Ball Shot! Angle: " + angle + " Strength: " + strength);
       
    }
    IEnumerator enablecollide(){
        yield return new WaitForSecondsRealtime(0.1f);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
    }
}
