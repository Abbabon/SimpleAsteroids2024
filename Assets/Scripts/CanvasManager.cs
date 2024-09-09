using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _hiScoreText;
    [SerializeField] private List<Image> _livesImages;

    [SerializeField] private GameObject _canvasButtonsContainer;
    
    
    private void Awake()
    {
        // var shouldPresentCanvasButtons = false;
        // #if UNITY_ANDROID || UNITY_IOS
        // shouldPresentCanvasButtons = true;
        // #endif
        _canvasButtonsContainer.SetActive(true);
    }

    public void UpdateHiScore(int hiScore)
    {
        _hiScoreText.text = hiScore.ToString();
    }
    
    public void UpdateCurrentScore(int currentScore)
    {
        _currentScoreText.text = currentScore.ToString();
    }
    
    public void UpdateLives(int lives)
    {
        for (var i = 0; i < _livesImages.Count; i++)
        {
            _livesImages[i].enabled = i < lives;
        }
    }
}