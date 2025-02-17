using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoop : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed=1f;
    private float positionx=6f;
    private float dir=1f;
    public SCORE scoremanager;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoremanager = FindObjectOfType<SCORE>();
        transform.position=new Vector2(6f,3.5f);//initial position for hoop
        
    }

    
    // Update is called once per frame,uncommented it for level two
    void Update()
    {
        
        if(scoremanager.round==2){
            //Debug.Log("hoop should be moving");
            rb.bodyType=RigidbodyType2D.Kinematic;
            //Debug.Log("hoop rb type"+rb.bodyType);
            positionx+=dir * speed * Time.deltaTime;
            //Debug.Log("adjust position"+positionx);
            if(positionx<=-3f){
                dir=1;
                positionx=-3f;
            }
            else if(positionx>=3f){
                dir=-1;
                positionx=3f;
            }
        
        
            rb.MovePosition(new Vector2(positionx,3f));
        }
        if(scoremanager.round==3){
            transform.position=new Vector2(5.32f,-1.5f);
            transform.localScale=new Vector2(1,0.1f);
        }
        
        
        
    }
    
}
