using UnityEngine;
using TMPro;
using System.Collections;

public class SCORE : MonoBehaviour
{   
    // public TextMeshProUGUI scoreText;  // This should be visible in the Inspector
    public TextMeshProUGUI roundText;
    // public Rigidbody2D wormholein;
    // public Rigidbody2D wormholeout;
    //public int score1 = 0;
    public int round=1;
    public bool nextlevel=false;
    

    // public int getScore()
    // {
    //     return score1;
    // }
    void Start()
    {
        
        roundText.gameObject.SetActive(false);
        checknextlevel();

    }
    

    // public void AddScore1(int points)
    // {
    //     score1 += points;
    //     //startgoal();
    //     UpdateScore();  

    // }
    // void UpdateScore()  // Fixed function name
    // {
    //     //scoreText.text =  "Score : "+score1;  // Fixed incorrect reference
    //     checknextlevel();
    // }
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
        if (round > 1) 
        {
            roundText.text += "\nCongratulation!";
        }

        roundText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        
        roundText.gameObject.SetActive(false);
        nextlevel = false; // Allow future calls
    }
}
        // else if (score1 == 3)
        // {

        //     Debug.Log("next level!");
        //     round = 3;
        //     StartCoroutine(showroundtext(round));

        // }

    
    /*
    public void AddScore2(int points)
    {
        score2 += points;
        startgoal();
        UpdateScore();  // Fixed function name
    }
    void startgoal(){
        goalsound.Play();
        Invoke("stopgoalsound",2f);
    }
    void stopgoalsound(){
        goalsound.Stop();
    }
    void UpdateScore()  // Fixed function name
    {
        scoreText.text =  score1+" : "+score2;  // Fixed incorrect reference
        checkwin();
    }
    void Stopgame(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops Play Mode in Editor
        #else
        Application.Quit(); // Works in a built game
        #endif
    }
    void checknextlevel(){
    if (score1 == 5)
    {
        goalEffect.gameObject.SetActive(true);
        goalEffect.Play();
        Debug.Log("Goal Effect Triggered!");
        WinText.text = "Player1 Wins";
        WinText.color=Color.green;
        scoreText.color=Color.red;
        scoreText.text ="Congratulation Player1!!!";
        
        StartCoroutine(EndGameAfterDelay()); // Call coroutine
    }
    else if (score2 ==5)
    {
        goalEffect.gameObject.SetActive(true);
        goalEffect.Play();
        Debug.Log("Goal Effect Triggered!");
        WinText.text = "Player2 Wins";
        WinText.color=Color.green;
        scoreText.color=Color.yellow;
        scoreText.text ="Congratulation Player2!!!";
        StartCoroutine(EndGameAfterDelay()); // Call coroutine
    }
}

IEnumerator EndGameAfterDelay()
{
   
    yield return new WaitForSecondsRealtime(3f); // Wait 3 seconds (ignores Time.timeScale)
    Stopgame();
}
}
*/


