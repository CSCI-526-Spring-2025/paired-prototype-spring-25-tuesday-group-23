using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class arrowui : MonoBehaviour
{
    //private float angle = 0f;
    //private float dir = 1f;
    private RectTransform arrowtransform;
    public player player;
    // Start is called before the first frame update
    void Start()
    {
        arrowtransform=GetComponent<RectTransform>();
        player = FindObjectOfType<player>();
        transform.SetParent(player.transform);
        transform.localPosition = new Vector2(1.25f, 2f);
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.shootstep==1){
            gameObject.SetActive(true);
            /*
            angle += dir * 60f * Time.deltaTime;
            if (angle >= 135f) { angle = 135f; dir = -1f; }
            else if (angle <= 45f) { angle = 45f; dir = 1f; }
            arrowtransform.rotation = Quaternion.Euler(0, 0, angle);
            */
        }
        
    }
}
