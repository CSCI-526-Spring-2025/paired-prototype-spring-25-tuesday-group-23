using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class player : MonoBehaviour
{
    private Rigidbody2D rb;
    public Ball ball;
    private float movementX;
    private float anglead;
    private float strengthad;
    public float speed = 5f;
    public int shootstep = 0;
    private float strength = 1f;
    private float angle = 0f;
    private bool adjustangle = false;
    private bool adjuststrength = false;
    private bool readyshoot = false;
    private bool spaceReleased = true;
    private int min_strength = 15;
    private int max_strength = 30;
    //public TextMeshProUGUI angleText;
    //public TextMeshProUGUI strengthText;
    public GameObject arrow;
    private RectTransform arrowTransform;
    public Slider strengthBar;
    private bool playerinair=false;

    void Start()
    {
        
        Time.fixedDeltaTime = 0.02f;
        
        rb = GameObject.Find("player").GetComponent<Rigidbody2D>();
        transform.position = new Vector2(-7.5f, -3.5f);
        arrowTransform = arrow.GetComponent<RectTransform>();
        arrow.SetActive(false);
        strengthBar.minValue=min_strength;
        strengthBar.maxValue=max_strength;
        strengthBar.gameObject.SetActive(false);
    }

    void Update()
    {
        //movementX = Input.GetAxisRaw("Horizontal");
        anglead=Input.GetAxisRaw("Angleadjust");
        strengthad=Input.GetAxisRaw("strengthadjust");
        if (shootstep == 1 && !adjustangle)
        {
            StartCoroutine(ModifyAngle());
        }
        else if (shootstep == 2 && !adjuststrength)
        {
            StartCoroutine(ModifyStrength());
        }
        else if (shootstep == 3 && !readyshoot)
        {
            StartCoroutine(ShootBall());
        }
    }
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("block")  || collision.gameObject.CompareTag("topbotwall")){
            playerinair=false;
        }
    }
    void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.CompareTag("block")  || collision.gameObject.CompareTag("topbotwall")){
            playerinair=true;
        }
    }

    IEnumerator ModifyAngle()
    {
        adjustangle = true;
        float dir = 1f;
        arrow.SetActive(true);
        while (adjustangle)
        {
            angle += dir * 60f * Time.deltaTime;
            if (angle >= 160f) { angle = 160f; dir = -1f; }
            else if (angle <= 20f) { angle = 20f; dir = 1f; }

            //angleText.text = "Angle: " + angle.ToString("F1");
            arrowTransform.rotation = Quaternion.Euler(0, 0, angle);
            
            if (Input.GetKeyDown(KeyCode.Space) && spaceReleased)
            {
                Debug.Log("Angle is set: " + angle);
                spaceReleased = false; 
                break;
            }

            yield return null;
        }

        
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
        spaceReleased = true;
        shootstep = 2;
        adjustangle = false;
        
        yield break;
    }

    IEnumerator ModifyStrength()
    {
        adjuststrength = true;
        int dir = 1;
        strengthBar.gameObject.SetActive(true);
       
        while (adjuststrength)
        {
            strength += dir * 10f * Time.deltaTime;
            if (strength >=max_strength) { strength = max_strength; dir = -1; }
            else if (strength < min_strength) { strength = min_strength; dir = 1; }

            //strengthText.text = "Strength: " + strength.ToString("F1");
            strengthBar.value=strength;
            if (Input.GetKeyDown(KeyCode.Space) && spaceReleased)
            {
                Debug.Log("Strength is set: " + strength);
                spaceReleased = false; 
                break;
            }

            yield return null;
        }

       
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
        spaceReleased = true;
        shootstep = 3;
        adjuststrength = false;
        yield break;
    }

    IEnumerator ShootBall()
    {
        readyshoot = true;

        while (true)
        {   //wasd, ws adjust angle, ad adjust strength
            if (Input.GetKey(KeyCode.W)) 
            {
                angle += 60f * Time.deltaTime;
                if (angle > 160f) angle = 160f;
            }
            else if (Input.GetKey(KeyCode.S)) 
            {
                angle -= 60f * Time.deltaTime;
                if (angle < 20f) angle = 20f;
            }

            if (Input.GetKey(KeyCode.A)) 
            {
                strength -= 10f * Time.deltaTime;
                if (strength < min_strength) strength = min_strength;
            }
            else if (Input.GetKey(KeyCode.D)) 
            {
                strength += 10f * Time.deltaTime;
                if (strength > max_strength) strength = max_strength;
            }

            // update ui
            arrowTransform.rotation = Quaternion.Euler(0, 0, angle);
            strengthBar.value = strength;
            if (Input.GetKeyDown(KeyCode.Space) && spaceReleased)
            {
                ball.ReleaseBall(strength, angle);
                Debug.Log("Ball Shot! Angle: " + angle + " Strength: " + strength);
                break;
            }

            yield return null;
        }

        shootstep = 0;
        readyshoot = false;
        arrow.SetActive(false);
        strengthBar.gameObject.SetActive(false);
        yield break;
    }

    void FixedUpdate()
    {
        if(playerinair){
            rb.velocity = new Vector2(0, 0);
        }else{
            rb.velocity = new Vector2(movementX * speed, 0);
        }
        if(shootstep!=3){
            angle+=anglead*3f;
            strength+=strengthad*3f;
        }
        arrow.transform.position=new Vector2(rb.position.x+1.03f, rb.position.y+1.13f);
        strengthBar.transform.position=new Vector2(rb.position.x+1.5f, rb.position.y+2.5f);
    }
}
