using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class blockmanager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blockprefab;
    private int blockNum = 2;
    public Ball ball;
    public SCORE scoremanager;
    public Pinspawner pinspawner;
    //private Vector2 blockareamin = new Vector2(-7f, -1f);
    //private Vector2 blockareamax = new Vector2(7f, 2f);
    private List<GameObject> blocks = new List<GameObject>();
    private bool firstspawn = true;
    void Start()
    {
        generateblock();
    }

    // Update is called once per frame
    void Update()
    {
        checklevel();
    }

    void checklevel()
    {
        int currScore = scoremanager.getScore();

        if (ball.gamenotover == false)
        {
            StopAllCoroutines();
        }
        else if (currScore == 1 && firstspawn)
        {
            Destroyblock();
            firstspawn = false;
            blockNum = 3;
            generateblock();
        } else if(currScore == 3 && firstspawn)
        {
            Destroyblock();
            firstspawn = false;
            blockNum = 5;
            generateblock();
        } else if (currScore == 2 || currScore == 4)
        {
            firstspawn = true;
        }
    }

    void Destroyblock()
    {
        Debug.Log("Destroying blocks...");
        foreach (GameObject block in blocks)
        {
            Destroy(block);
        }
        blocks.Clear();
    }

    void generateblock()
    {



        if(scoremanager.round == 1)
        {
            List<float[]> values = new List<float[]>
            {
                new float[] { -5f, -1.25f, 0.25f },
                new float[] { 0f, -2f, 0.2f },
                new float[] { 3f, 2f, 0.175f }
            };

            List<float> widths = new List<float>();

            foreach (var list in values)
            {
                Vector2 randomPosition = new Vector2(list[0], list[1]);
                Quaternion rotation = Quaternion.identity;
                GameObject newblock1 = Instantiate(blockprefab, randomPosition, rotation);
                newblock1.transform.localScale = new Vector3(list[2], 0.1f, 1f);
                float width = newblock1.GetComponent<Collider2D>().bounds.size.x * list[2];
                widths.Add(width);
                //Debug.Log("Block Width: " + width);
                blocks.Add(newblock1);
            }
            foreach (var width in widths) {
                Debug.Log("width: "+width);
            }
            
            pinspawner.generatespike(values, widths);
            


        }
        //for (int i = 0; i < blockNum; i++)
        //{
        //    //Debug.Log("Block num " + i);
        //    Vector2 randomPosition = new Vector2(0, 0);
        //    Quaternion rotation = Quaternion.identity;
        //    randomPosition = new Vector2(Random.Range(blockareamin.x, blockareamax.x), Random.Range(blockareamin.y, blockareamax.y));
        //    //Debug.Log("Block num " + randomPosition);
        //    while (Mathf.Abs(randomPosition.x - ball.wormholeIn.position.x) < 0.75f &&
        //           Mathf.Abs(randomPosition.y - ball.wormholeIn.position.y) < 0.75f)
        //    {
        //        Debug.Log("Position too close to wormhole, adjusting...");
        //        randomPosition = new Vector2(Random.Range(blockareamin.x, blockareamax.x), Random.Range(blockareamin.y, blockareamax.y));   
        //    }
        //    //blockprefab.localscale
        //    GameObject newblock = Instantiate(blockprefab, randomPosition, rotation);
        //    int scaleX = Random.Range(10,30);
        //    newblock.transform.localScale = new Vector3(scaleX/100f, 0.1f, 1f);

        //    Debug.Log("block is at " + randomPosition);
        //    blocks.Add(newblock);
        //}
        Debug.Log("Blocks " + blocks);
    }
}
// -5, -1.25, 0.25
// 0, 2, 0.2
//3, 2, 0.175

//6, 3.5