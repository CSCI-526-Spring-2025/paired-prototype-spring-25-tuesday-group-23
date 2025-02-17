using UnityEngine;
using TMPro;
using System.Collections;

public class SCORE : MonoBehaviour
{   
   
    public TextMeshProUGUI roundText;
    
    public int round=1;
    public bool nextlevel=false;
    

    void Start()
    {
        
        roundText.gameObject.SetActive(false);
        checknextlevel();

    }
    

    
    public void checknextlevel()
    {
        if (!nextlevel) // Ensure coroutine runs only once per round
        {
            nextlevel = true;
            Debug.Log("Round " + round + " started!");
            StartCoroutine(showroundtext(round));
        }
    }

    IEnumerator showroundtext(int round)
    {
        roundText.text = "Round " + round.ToString();
        if (round ==2 ||round==3) 
        {
            roundText.text += "\nCongratulation!";
        }
        if (round ==4){
            roundText.text = "\nYOU WIN!";
            Time.timeScale=0;
        }

        roundText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        
        roundText.gameObject.SetActive(false);
        nextlevel = false; // Allow future calls
    }
}
        

