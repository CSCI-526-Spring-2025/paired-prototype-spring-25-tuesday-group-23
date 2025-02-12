using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Pinspawner : MonoBehaviour
{
    public GameObject spikeprefab;
    private int spikenum=2;
    public Ball ball;
    public SCORE scoremanager;
    private Vector2 spikeareamin=new Vector2(-8.3f,-4.4f);

    
    private Vector2 spikeareamax=new Vector2(7.8f,3.6f);
    private List<GameObject> spikes=new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("spike is runnnig");
        //generatespike();
    }
    void Update(){
        checklevel();
    }
    void checklevel(){
        if(ball.gamenotover==false){
            StopAllCoroutines();
        }
        
    }
    



    void Destroyspike(){
        Debug.Log("Destroying spikes...");
        foreach (GameObject spike in spikes){
            Destroy(spike);
        }
        spikes.Clear();
    }


    

    public void generatespike(List<float[]> blockinfo, List<float> widthforblock)
    {
        Debug.Log("GenerateSpikes() is running!");

        int numofblock = blockinfo.Count;
        float spikewidth = spikeprefab.GetComponent<SpriteRenderer>().bounds.size.x;

        for (int i = 0; i < numofblock; i++) // Access the info of each block
        {
            float start = blockinfo[i][0] + widthforblock[i] /2;//block x + half block width
            Vector2 randomPosition = new Vector2(start, -4.42f);

            float length;
            if (i == numofblock - 1) // Last block
            {
                length = spikeareamax[0] - blockinfo[i][0]; 
            }
            else // Normal case
            {
                length = (blockinfo[i + 1][0]-(widthforblock[i+1] /2)) - start;//next block position- half next block's width
            }

            while (length > spikewidth) // If remaining length is shorter than spikewidth, stop generating spikes
            {
                GameObject spike1 = Instantiate(spikeprefab, randomPosition, Quaternion.identity);
                spikes.Add(spike1);
                randomPosition += new Vector2(spikewidth, 0); // Corrected vector addition
                length -= spikewidth;
            }
        }
    }
       
>>>>>>> a549d6d (new version)


       
=======
}
>>>>>>> a549d6d (new version)
