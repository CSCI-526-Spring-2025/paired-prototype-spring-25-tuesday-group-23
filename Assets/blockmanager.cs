using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class blockmanager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blockprefab;
    private int blockNum = 2;
    public Ball ball;
    public SCORE scoremanager;
    private Vector2 blockareamin = new Vector2(-7f, -1f);
    private Vector2 blockareamax = new Vector2(7f, 2f);
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
        for (int i = 0; i < blockNum; i++)
        {
            //Debug.Log("Block num " + i);
            Vector2 randomPosition = new Vector2(0, 0);
            Quaternion rotation = Quaternion.identity;
            randomPosition = new Vector2(Random.Range(blockareamin.x, blockareamax.x), Random.Range(blockareamin.y, blockareamax.y));
            //Debug.Log("Block num " + randomPosition);
            while (Mathf.Abs(randomPosition.x - ball.wormholeIn.position.x) < 0.75f &&
                   Mathf.Abs(randomPosition.y - ball.wormholeIn.position.y) < 0.75f)
            {
                Debug.Log("Position too close to wormhole, adjusting...");
                randomPosition = new Vector2(Random.Range(blockareamin.x, blockareamax.x), Random.Range(blockareamin.y, blockareamax.y));   
            }
            //blockprefab.localscale
            GameObject newblock = Instantiate(blockprefab, randomPosition, rotation);
            int scaleX = Random.Range(10,30);
            newblock.transform.localScale = new Vector3(scaleX/100f, 0.1f, 1f);

            Debug.Log("block is at " + randomPosition);
            blocks.Add(newblock);
        }
        Debug.Log("Blocks " + blocks);
    }
}
