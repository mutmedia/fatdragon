using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class ScoreManager : MonoBehaviour
{

    private const int BaseScore = 100;
    
    private int _score;
    private float _combo;
    private int _currentStreak = 0;


    
    
    // Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void OnResolveCommand(object sende, ResolveCommandEventArgs e)
    {
        if (e.IsCorrect)
        {
            _score += BaseScore + (int)_combo * BaseScore;
        }
        else
        {
            _combo = 0;
        }
        
        Debug.Log("SCORE: " + _score);
    }

    public void OnListOver(object sender, OnListOverEventArgs e)
    {
        if (e.IsSuccessful)
        {
            _currentStreak++;
            Debug.Log("Current Streak Increased and is now " + _currentStreak);
        }
        else
        {
            _currentStreak = 0;
        }
        _combo = (10 + _currentStreak)/10;
    }
}
