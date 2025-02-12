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
    private Vector2 spikeareamax=new Vector2(8f,3.6f);
    private List<GameObject> spikes=new List<GameObject>();
    private bool firstspawn = true;
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
        int currScore = scoremanager.getScore();
        if (currScore == 2 || currScore == 4) {
            firstspawn = true;
        }
        if (currScore == 3 && firstspawn)
        {
            firstspawn = false;
            Destroyspike();
            spikenum=5;
            StartCoroutine(randomspike());
        } else if (currScore == 1 && firstspawn)
        {
            firstspawn = false;
            Destroyspike();
            spikenum = 5;
            //generatespike();

        }
    }
    IEnumerator randomspike(){
        while(scoremanager.round !=1 ) // Keep running while round is 3
        {
            //generatespike();
            yield return new WaitForSecondsRealtime(4f);
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


    
    public void generatespike(List<float[]> values, List<float> widths)
    {
        Debug.Log("GenerateSpikes() is running!");
        float x = values[0][0] + (2*widths[0]);
        while(x <= values[1][0] - widths[1])
        {
            Vector2 randomPosition = new Vector2(x, -4.42f);
            Quaternion rotation = Quaternion.identity;
            GameObject spike1 = Instantiate(spikeprefab, randomPosition, rotation);
            spike1.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            spikes.Add(spike1);
            x = x + 0.5f;
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
}
