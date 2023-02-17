using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Fuel : MonoBehaviour
{
    [SerializeField] private float _amount = 100;

    private bool _isCollected;
    private Animator _animator;

    public float Amount => _amount;

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
            playerInfo.CollectFuel(this);
            _isCollected = true;
            _animator.SetTrigger(Constants.FuelCollectedTriggerName);
        }
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }
}
