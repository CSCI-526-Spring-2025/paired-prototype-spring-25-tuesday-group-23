using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class blockmanager : MonoBehaviour
{
    public GameObject blockprefab;
    public Ball ball;
    public SCORE scoremanager;
    public Pinspawner pinspawner;
    private List<GameObject> blocks = new List<GameObject>();
    private bool blocksSpawned = false; 
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
        if (ball.gamenotover == false)
        {
            StopAllCoroutines();
        }

        //This is checking what level is at, supposed to reset blocks and generate them again 
        if(scoremanager.round == 2 && !blocksSpawned){
           blocksSpawned = true;  // Prevents re-triggering
           Destroyblock(); // Remove previous round's blocks
           generateblock(); // Spawn new moving blocks
        }

        if(scoremanager.round == 3)
        {
            Destroyblock();
            pinspawner.Destroyspike();
        }
      
    }

    void Destroyblock()
    {
        Debug.Log("Destroying existing blocks...");

        foreach (GameObject block in blocks)
        {
            if (block != null)
            {
                Debug.Log($"Destroying {block.name} at {block.transform.position}");
                Destroy(block);
            }
        }

        blocks.Clear(); // Clear list after confirming destruction
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
                float blockwidth=sr.bounds.size[0];
                widthforblock.Add(blockwidth);
                blocks.Add(newblock1);

            }

            pinspawner.generatespike(blockinfo, widthforblock);
                       
        }
        else if (scoremanager.round == 2) // Moving blocks for round 2
        {
            Debug.Log("Current Round: " + scoremanager.round);
            List<float[]> blockinfo = new List<float[]>
            {
                new float[] { -3.5f, 1f, 0.3f }, 
                new float[] { 2.5f, -2f, 0.25f },
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
                float blockwidth=sr.bounds.size[0];
                //add such block's width to widthforblock list
                widthforblock.Add(blockwidth);
                Debug.Log("Block size: " +blockwidth);
                blocks.Add(newblock1);//for destroyblock needs

                //Add moving blocks 
                 MovingBlock movingScript = newblock1.AddComponent<MovingBlock>();
                 if (movingScript == null)
                    {
                        Debug.LogError("Level 2 Block: MovingBlock script was NOT added!");
                    }
                    else
                    {
                        Debug.Log("Level 2 Block: MovingBlock script successfully added!");
                    }
                 movingScript.SetMovementParams(0.25f, 1f); 

            }

            pinspawner.generatespike(blockinfo, widthforblock);

        }
    }
}