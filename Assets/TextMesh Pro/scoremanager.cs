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
        Debug.Log("checknextlevel() called. Current round: " + round + ", nextlevel: " + nextlevel);
        if (!nextlevel) // Ensure coroutine runs only once per round
        {
            nextlevel = true;
            Debug.Log("Round " + round + " started!");
            StartCoroutine(showroundtext(round));
        }
    }

    IEnumerator showroundtext(int round)
    {
        if (round == 3)
        {
            roundText.text = "Congratulations, you win!";
        } else
        {
            roundText.text = "Round " + round.ToString();
            if (round > 1)
            {
                roundText.text += "\nCongratulation!";
                GameObject obj = GameObject.FindGameObjectWithTag("hoop");
                obj.transform.position = new Vector2(5.5f, 0.5f);
            }            
            nextlevel = false; // Allow future calls
        }
        roundText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);

        roundText.gameObject.SetActive(false);

    }
}
       

