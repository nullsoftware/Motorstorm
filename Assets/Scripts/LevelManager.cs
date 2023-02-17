using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private CarController _carController;
    [SerializeField] private Text _currentScoreDisplayer;
    [SerializeField] private Text _maxScoreDisplayer;
    [SerializeField] private Text _descriptionDisplayer;
    [SerializeField] private AudioSource _gameOverSound;

    private Coroutine _gameOverCoroutine;
    private bool _isGameOverShown;

    public void StartDelayedGameOver()
    {
        if (_gameOverCoroutine == null)
        {
            _gameOverCoroutine = StartCoroutine(InvokeDelayedGameOver());
        }
    }

    public void EndDelayedGameOver()
    {
        if (_gameOverCoroutine != null)
        {
            StopCoroutine(_gameOverCoroutine);
            _gameOverCoroutine = null;
        }
    }

    public void OnGameOver(DeathType deathType = DeathType.Crashed)
    {
        if (_isGameOverShown)
        {
            return;
        }

        _isGameOverShown = true;
        _currentScoreDisplayer.text = string.Format(_currentScoreDisplayer.text, _playerInfo.Score.ToString("N1"));
        _maxScoreDisplayer.text = string.Format(_maxScoreDisplayer.text, _playerInfo.MaxScore.ToString("N1"));
        _descriptionDisplayer.text = GetDescription(deathType);
        _playerInfo.SaveGameData();

        gameObject.SetActiveRecursively(true);

        switch (deathType)
        {
            case DeathType.Crashed:
                CarController.Break(_carController);
                break;
        }

        _carController.enabled = false;
        _playerInfo.enabled = false;
        _gameOverSound?.Play();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator InvokeDelayedGameOver()
    {
        yield return new WaitForSeconds(5);

        OnGameOver(DeathType.FuelEnded);
    }

    private string GetDescription(DeathType deathType)
    {
        switch (deathType)
        {
            case DeathType.FuelEnded:
                return "Out of Fuel";
            default:
                return "Crashed";
        }
    }
}
