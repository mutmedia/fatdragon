using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    private const int BaseScore = 100;
    
    public int _score;
    private decimal _combo;
    private int _currentStreak = 0;

    public Text ScoreText;
    public Text ComboText;


    
    
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
            _score -= BaseScore;
            _combo = 0;
        }

        ScoreText.text = "Score: " + _score;
        
        //Debug.Log("SCORE: " + _score);
    }

    public void OnListOver(object sender, OnListOverEventArgs e)
    {
        if (e.IsSuccessful)
        {
            _currentStreak++;
        }
        else
        {
            _currentStreak = 0;

        }
        _combo = (10 + new decimal(_currentStreak))/10;
        Debug.Log("Current combo is now " + _combo);
        ComboText.text = "x " + String.Format("{0:0.0}", _combo);
    }
}
