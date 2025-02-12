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
    public float speed = 5f;
    public int shootstep = 0;
    private float strength = 1f;
    private float angle = 0f;
    private bool adjustangle = false;
    private bool adjuststrength = false;
    private bool readyshoot = false;
    private bool spaceReleased = true;
    //public TextMeshProUGUI angleText;
    //public TextMeshProUGUI strengthText;
    public GameObject arrow;
    private RectTransform arrowTransform;
    public Slider strengthBar;

    void Start()
    {
        rb = GameObject.Find("player").GetComponent<Rigidbody2D>();
        transform.position = new Vector2(-7.5f, -3.5f);
        arrowTransform = arrow.GetComponent<RectTransform>();
        arrow.SetActive(false);
        strengthBar.gameObject.SetActive(false);
    }

    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");

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
        float dir = 1f;
        strengthBar.gameObject.SetActive(true);
        float min_strength = 10f;
        float max_strength = 20f;
        float increment = 1f;
        while (adjuststrength)
        {
            strength += dir * 3f * Time.deltaTime;
            if (strength >=max_strength) { strength = max_strength; dir = (-1f * increment); }
            else if (strength < min_strength) { strength = min_strength; dir = increment; }

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
        {
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
        rb.velocity = new Vector2(movementX * speed, 0);
        arrow.transform.position=new Vector2(rb.position.x+1.03f, rb.position.y+1.13f);
        strengthBar.transform.position=new Vector2(rb.position.x+1.5f, rb.position.y+2f);
    }
}
