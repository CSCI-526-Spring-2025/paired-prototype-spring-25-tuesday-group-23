using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class blockmanager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blockprefab;
    //private int blockNum = 2;
    public Ball ball;
    public SCORE scoremanager;
    public Pinspawner pinspawner;
    private float positionx;
    private float positiony;
    
    
    private Dictionary<GameObject, (float initialPosition, int dir, float range)> movementData = new Dictionary<GameObject, (float, int,float)>();
    private List<GameObject> blocks = new List<GameObject>();
    private List<GameObject> moveblocks=new List<GameObject>();
    public bool firstgenblock=true;
  
    void Start()
    {
        generateblock();
    }

    // Update is called once per frame
    void Update()
    {
        checklevel();
        if(scoremanager.round==2 ||scoremanager.round==3 ){
            foreach(GameObject block in moveblocks){
                Rigidbody2D rb=block.GetComponent<Rigidbody2D>();
                if (!movementData.ContainsKey(block) || block == null)
                {
                        Debug.LogError("WTF? Block not in movementData: " + block.name);
                        continue;
                }
                var(initailPos,dir,range)=movementData[block];
                if(dir==1 || dir==-1){
                    Debug.Log("now IN UPDATE");
                    movingleftright(block,rb);
                }
                else{//for dir ==2 || dir==-2
                    movingupdown(block,rb);
                }
                
            }
           
        }
        
    }
   

    void checklevel()
    {
        //int currScore = scoremanager.getScore();

        if (ball.gamenotover == false)
        {
            StopAllCoroutines();
        }
        else if (scoremanager.round!=1 && firstgenblock){
            firstgenblock=false;
            Destroyblock();
            generateblock();
        }
       
    }

    void Destroyblock()
    {
        Debug.Log("Destroying blocks...");
        foreach (GameObject block in blocks)
        {
            Destroy(block);
            movementData.Remove(block);
        }
        blocks.Clear();
        moveblocks.Clear();
        Debug.Log("Blocks cleared, count: " + blocks.Count);

    }

    void generateblock()
    {



        if(scoremanager.round == 1)
        {
            List<float[]> blockinfo = new List<float[]>
            {
                new float[] { -4.5f, -1.25f, 0.25f },
                new float[] { 0f, -2.5f, 0.175f },
                new float[] { 4f, 0.5f, 0.25f }
            };

            

            List<float> widthforblock = new List<float>();

            foreach (var list in blockinfo)
            {
                Vector2 randomPosition = new Vector2(list[0], list[1]);
                GameObject newblock1 = Instantiate(blockprefab, randomPosition, Quaternion.identity);
                //modify scale of each block(x ,y,z)
                newblock1.transform.localScale = new Vector3(list[2],0.1f,1);

                SpriteRenderer sr = newblock1.GetComponent<SpriteRenderer>();
                //get real size of block
                float blockwidth=sr.bounds.size.x;
                //add such block's width to widthforblock list
                widthforblock.Add(blockwidth);
                Debug.Log("Block size: " +blockwidth);
                blocks.Add(newblock1);//for destroyblock needs

            }

            pinspawner.generatespike(blockinfo, widthforblock);
                       
        }
        else if(scoremanager.round==2){
           

            GameObject blockblock = Instantiate(blockprefab, new Vector2(-2, 0.8f), Quaternion.identity);
            blockblock.transform.localScale = new Vector3(1f, 0.1f, 1);
            blocks.Add(blockblock);
            moveblocks.Add(blockblock);
            movementData[blockblock] = (1.25f, 2,0.75f);//block that moves up and down


            //  blocks moving left and right
            List<float[]> leftrightmoveblockinfo = new List<float[]>
            {
                new float[] { -4.5f, -2.5f, 0.25f,1f }, // Block 1, x,y,x-scale,floating range
                new float[] { 0.5f, -3.5f, 0.3f,1.25f },
                new float[] { 6f, -1.5f, 0.3f,1.25f }, // Block 2
            };

            foreach (var list in leftrightmoveblockinfo)
            {
                Vector2 position = new Vector2(list[0], list[1]);
                GameObject newblock = Instantiate(blockprefab, position, Quaternion.identity);
                newblock.transform.localScale = new Vector3(list[2], 0.1f, 1);
                blocks.Add(newblock);
                moveblocks.Add(newblock);
                movementData[newblock] = (list[0], 1,list[3]);
            }

        }
        else{//round ==3
            
            //still block:{5,-0.83}{0.33,0.1}
            //still block:{4.13,-2.9}{0.1,0.4}
            GameObject stblock = Instantiate(blockprefab, new Vector2(5.3f, 0), Quaternion.identity);
            stblock.transform.localScale = new Vector3(0.33f, 0.1f, 1);
            GameObject hrblock = Instantiate(blockprefab, new Vector2(4.25f, -2.43f), Quaternion.identity);
            hrblock.transform.localScale = new Vector3(0.08f, 0.53f, 1);

            //leftright
            //{7.57,-3.7}{0.15,0.1}
            //{-2.4,0.6}range1.5
            //{6,1.5}range2
            List<float[]> leftrightmoveblockinfo = new List<float[]>
            {
                new float[] { 7.57f, -3.7f, 0.1f,0.5f }, // Block 1, x,y,x-scale,floating range
                new float[] { -1.8f, 0.6f, 0.15f,1.5f },
                //new float[] { 6f, 1.5f, 0.15f,1.5f }, // Block 2
            };

            foreach (var list in leftrightmoveblockinfo)
            {
                Vector2 position = new Vector2(list[0], list[1]);
                GameObject newblock = Instantiate(blockprefab, position, Quaternion.identity);
                newblock.transform.localScale = new Vector3(list[2], 0.1f, 1);
                moveblocks.Add(newblock);
                movementData[newblock] = (list[0], 1,list[3]);
            }

            //updown
            //{-5.2,2}range2
            //{1.3,2}range1
            //hoop{5.2,-1.7}{1,0.1}

            List<float[]> updownblockinfo = new List<float[]>
            {
                new float[] { -3.8f, -3f, 0.15f,1f }, // Block 1, x,y,x-scale,floating range
                new float[] { -5.8f, -1f, 0.15f,0.5f },
                new float[] { 1.6f, 1.6f, 0.15f,0.6f },
                
            };

            foreach (var list in updownblockinfo)
            {
                Vector2 position = new Vector2(list[0], list[1]);
                GameObject newblock = Instantiate(blockprefab, position, Quaternion.identity);
                newblock.transform.localScale = new Vector3(list[2], 0.1f, 1);
                moveblocks.Add(newblock);
                movementData[newblock] = (list[1], 2,list[3]);
            }


        }

        
       
        
    }
    void movingleftright(GameObject block, Rigidbody2D rb)
    {
        //Debug.Log("now moving");
        rb.bodyType = RigidbodyType2D.Kinematic;
        var (initialX, dir,range) = movementData[block];
        //Debug.Log("wtf is the speed:"+speed);
        float newX = rb.position.x + dir * 1 * Time.deltaTime;
        if (newX <= initialX - range)
        {
            movementData[block] = (initialX, 1,range); 
            newX = initialX - range;
        }
        else if (newX >= initialX + range)
        {
            movementData[block] = (initialX, -1,range);
            newX = initialX + range;
        }
        //Debug.Log("now moving to x position:"+newX);
        rb.MovePosition(new Vector2(newX, rb.position.y));
    }


    void movingupdown(GameObject block, Rigidbody2D rb)
    {
        Debug.Log("now moving y");
        rb.bodyType = RigidbodyType2D.Kinematic;
        var (initialY, dir,range) = movementData[block];
        //Debug.Log("wtf is the speed:"+speed);
        float newY = rb.position.y + dir * 1 * Time.deltaTime;
        if (newY <= initialY - range)
        {
            movementData[block] = (initialY, 2,range); 
            newY = initialY - range;
        }
        else if (newY >= initialY + range)
        {
            movementData[block] = (initialY, -2,range);
            newY = initialY + range;
        }
        Debug.Log("now moving to y position:"+newY);
        rb.MovePosition(new Vector2(rb.position.x,newY));
    }

    

    
}
