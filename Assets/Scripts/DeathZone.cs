using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeathZone : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.TryGetComponent(out PlayerInfo playerInfo))
        {
            _levelManager.OnGameOver(DeathType.Crashed);
        }
    }
}
