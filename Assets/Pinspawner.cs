using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinspawner : MonoBehaviour
{
    public GameObject spikeprefab;
    private int spikenum=2;
    public Ball ball;
    public SCORE scoremanager;
    private Vector2 spikeareamin=new Vector2(-8.3f,-4.4f);
    private Vector2 spikeareamax=new Vector2(8f,3.6f);
    private List<GameObject> spikes=new List<GameObject>();
    private bool firstspawn = true;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("spike is runnnig");
        generatespike();
    }
    void Update(){
        checklevel();
    }
    void checklevel(){
        if(ball.gamenotover==false){
            StopAllCoroutines();
        }
        if(scoremanager.round==2 && firstspawn){
            Destroyspike();
            
            firstspawn=false;
            spikenum=5;
            StartCoroutine(randomspike());
        }
    }
    IEnumerator randomspike(){
        while(scoremanager.round !=1 ) // Keep running while round is 2
        {
            generatespike();
            yield return new WaitForSecondsRealtime(7f);
            Destroyspike();
            yield return new WaitForSecondsRealtime(1f);
        }
    }



    void Destroyspike(){
        Debug.Log("Destroying spikes...");
        foreach (GameObject spike in spikes){
            Destroy(spike);
        }
        spikes.Clear();
    }


    
    void generatespike()
    {
        Debug.Log("GenerateSpikes() is running!");
        for(int i=0;i<spikenum;i++){
            int randomnum=Random.Range(1,4);
            Vector2 randomPosition=new Vector2(0,0);
            Quaternion rotation = Quaternion.identity;
            switch(randomnum){
                case 1://on the ground
                    randomPosition=new Vector2(Random.Range(spikeareamin.x,spikeareamax.x),spikeareamin.y);
                    break;
                case 2:// on the left wall
                    randomPosition=new Vector2(spikeareamin.x,Random.Range(spikeareamin.y,spikeareamax.y));
                    rotation=Quaternion.Euler(0,0,-90);
                    break;
                case 3:// on the right wall
                    randomPosition=new Vector2(spikeareamax.x,Random.Range(spikeareamin.y,spikeareamax.y));
                    rotation=Quaternion.Euler(0,0,90);
                    break;

            }
            while (Mathf.Abs(randomPosition.x - ball.wormholeIn.position.x) < 0.75f &&
                   Mathf.Abs(randomPosition.y - ball.wormholeIn.position.y) < 0.75f)
            {
                Debug.Log("Position too close to wormhole, adjusting...");
                randomPosition = new Vector2(Random.Range(spikeareamin.x,spikeareamax.x),spikeareamin.y);
                rotation = Quaternion.identity;
            }
            Debug.Log("Trying to spawn spike at " + randomPosition);
            GameObject newspike=Instantiate(spikeprefab, randomPosition, rotation);
            Debug.Log("spike is at "+randomPosition);
            spikes.Add(newspike);
        }
    }
}
