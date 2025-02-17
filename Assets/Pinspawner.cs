using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Pinspawner : MonoBehaviour
{
    public GameObject spikeprefab;
   
    public Ball ball;
    public SCORE scoremanager;
    private Vector2 spikeareamin=new Vector2(-8.3f,-4.4f);
    private Vector2 spikeareamax=new Vector2(7.8f,3.6f);
    private List<GameObject> spikes=new List<GameObject>();
    private bool firstspawn = true;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("spike is runnnig");
        //generatespike();
    }
    void Update(){
        if(scoremanager.round==2 && firstspawn){
            firstspawn=false;
            Destroyspike();
            genround2spike();
        }
    }
    void genround2spike(){
        Debug.Log("get into round2, genround2spike is running");
        float spikewidth = spikeprefab.GetComponent<SpriteRenderer>().bounds.size.x;

        Vector2 randomPosition=new Vector2(spikeareamin.x+2.5f,spikeareamin.y);
        float length=spikeareamax.x-(spikeareamin.x+2.5f);
        while (length > spikewidth) // If remaining length is shorter than spikewidth, stop generating spikes
        {
                GameObject spike1 = Instantiate(spikeprefab, randomPosition, Quaternion.identity);
                randomPosition += new Vector2(spikewidth, 0); // Corrected vector addition
                length -= spikewidth;
                spikes.Add(spike1);
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
                randomPosition += new Vector2(spikewidth, 0); // Corrected vector addition
                length -= spikewidth;
                spikes.Add(spike1);
            }
        }
    }
       


        //Vector2 randomPosition = new Vector2(-5, -1.25f);
        //Quaternion rotation = Quaternion.identity;
        //GameObject spike1 = Instantiate(spikeprefab, randomPosition, rotation);
        //spike1.transform.localScale = new Vector3(0.25f, 0.1f, 1f);
        //spikes.Add(spike1);

        //for(int i=0;i<spikenum;i++){
        //    int randomnum=Random.Range(1,4);
        //    Vector2 randomPosition=new Vector2(0,0);
        //    Quaternion rotation = Quaternion.identity;
        //    switch(randomnum){
        //        case 1://on the ground
        //            randomPosition=new Vector2(Random.Range(spikeareamin.x,spikeareamax.x),spikeareamin.y);
        //            break;
        //        case 2:// on the left wall
        //            randomPosition=new Vector2(spikeareamin.x,Random.Range(spikeareamin.y,spikeareamax.y));
        //            rotation=Quaternion.Euler(0,0,-90);
        //            break;
        //        case 3:// on the right wall
        //            randomPosition=new Vector2(spikeareamax.x,Random.Range(spikeareamin.y,spikeareamax.y));
        //            rotation=Quaternion.Euler(0,0,90);
        //            break;

        //    }
        //while (Mathf.Abs(randomPosition.x - ball.wormholeIn.position.x) < 0.75f &&
        //       Mathf.Abs(randomPosition.y - ball.wormholeIn.position.y) < 0.75f)
        //{
        //    Debug.Log("Position too close to wormhole, adjusting...");
        //    randomPosition = new Vector2(Random.Range(spikeareamin.x,spikeareamax.x),spikeareamin.y);
        //    rotation = Quaternion.identity;
        //}
        //Debug.Log("Trying to spawn spike at " + randomPosition);
        //    //GameObject newspike=Instantiate(spikeprefab, randomPosition, rotation);
        //    Debug.Log("spike is at "+randomPosition);
        //    //spikes.Add(newspike);
        //}
}