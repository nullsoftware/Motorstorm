using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float _fuel = 100;
    [SerializeField] private float _scoreMultiplier = 1;
    [SerializeField] private float _fuelUsageMultiplier = 2;
    [SerializeField] private float _fuelActiveUsageMultiplier = 4;

    [Header("UI")]
    [SerializeField] private Text _coinDisplayer;
    [SerializeField] private Text _scoreDisplayer;
    [SerializeField] private Image _fuelProgressBar;

    [Header("Audio")]
    [SerializeField] private AudioSource _coinCollectedSound;
    [SerializeField] private AudioSource _fuelCollectedSound;

    [Header("Level")]
    [SerializeField] private LevelManager _levelManager;


    private string _coinStringFormat;
    private string _scoreStringFormat;

    private uint _coins;  
    private float _score;
    private float _maxScore;

    private Vector2 _startPosition;

    public float Score => _score;
    public float MaxScore => _maxScore;

    public bool IsActivelyUse { get; set; }

    public bool IsFuelEmpty => _fuel == 0;

    private void Start()
    {
        LoadGameData();

        _startPosition = transform.position;
        _coinStringFormat = _coinDisplayer.text;
        _scoreStringFormat = _scoreDisplayer.text;

        UpdateCoinText();
        UpdateScoreText();
    }

    private void FixedUpdate()
    {
        _score = MathF.Max(_score, (transform.position.x - _startPosition.x) * _scoreMultiplier);
        _maxScore = Mathf.Max(_maxScore, _score);

        if (IsActivelyUse)
            _fuel = Mathf.Clamp(_fuel - _fuelActiveUsageMultiplier * Time.deltaTime, 0, 100);
        else
            _fuel = Mathf.Clamp(_fuel - _fuelUsageMultiplier * Time.deltaTime, 0, 100);

        UpdateFualProgressBar();
        UpdateScoreText();

        // [TODO]: replace in separated class
        if (IsFuelEmpty)
            _levelManager.StartDelayedGameOver();
        else
            _levelManager.EndDelayedGameOver();
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    public void LoadGameData()
    {
        string fileName = Path.Combine(Application.persistentDataPath, Constants.GameDataFileName);

        if (GameData.TryLoad(fileName, out GameData data))
        {
            _coins = data.Coins;
            _maxScore = data.Score;
        }
    }

    public void SaveGameData()
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        string fileName = Path.Combine(Application.persistentDataPath, Constants.GameDataFileName);

        GameData data = new GameData()
        {
            Name = Environment.UserName,
            Score = _maxScore,
            Coins = _coins,
        };

        GameData.Save(fileName, data);
    }


    public void CollectCoin(Coin coin)
    {
        _coins += coin.Amount;
        UpdateCoinText();

        _coinCollectedSound?.Play();
    }

    public void CollectFuel(Fuel fuel)
    {
        _fuel = MathF.Min(100, _fuel + fuel.Amount);
        UpdateFualProgressBar();

        _fuelCollectedSound?.Play();
    }

    private void UpdateFualProgressBar()
    {
        _fuelProgressBar.fillAmount = _fuel / 100f;
    }

    private void UpdateScoreText()
    {
        _scoreDisplayer.text = string.Format(_scoreStringFormat, Mathf.RoundToInt(_score));
    }

    private void UpdateCoinText()
    {
        _coinDisplayer.text = string.Format(_coinStringFormat, _coins);
    }

}
