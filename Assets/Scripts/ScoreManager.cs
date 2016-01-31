using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    private const int BaseScore = 100;
    
    private int _score;
    private decimal _combo;
    private int _currentStreak = 0;
    private string _feedBackText = "";

    public Text ScoreText;
    public Text ComboText;
    public Text FeedbackText;
    
    
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
            _score += 10*BaseScore + (int)_combo * BaseScore;
            _feedBackText = "GOOD!";
            FeedbackText.color = Color.green;
        }
        else
        {
            _score -= BaseScore;
            _combo = 0;
            _feedBackText = "BAD.";
            FeedbackText.color = Color.red;
        }

        ScoreText.text = "Score: " + _score;
        FeedbackText.text = _feedBackText;

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
        _combo = (1 + new decimal(_currentStreak));
        Debug.Log("Current combo is now " + _combo);
        ComboText.text = "x " + String.Format("{0:0.0}", _combo);
    }
}
