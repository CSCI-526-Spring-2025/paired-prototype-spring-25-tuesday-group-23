using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoop : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed=0f;
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
        if(scoremanager.round>1){
            positionx+=dir * speed * Time.deltaTime;
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
        
        
        
    }
    
}
