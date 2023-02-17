using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Coin : MonoBehaviour
{
    [SerializeField] private byte _amount = 1;

    private bool _isCollected;
    private Animator _animator;

    public byte Amount => _amount;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isCollected)
        {
            return;
        }

        if (collision.transform.parent != null && collision.transform.parent.TryGetComponent(out PlayerInfo playerInfo))
        {
            playerInfo.CollectCoin(this);
            _isCollected = true;
            _animator.SetTrigger(Constants.CoinCollectedTriggerName);
        }
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }
}