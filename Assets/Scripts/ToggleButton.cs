using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ToggleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _pressedSprite;

    private Image _renderer;

    public bool IsPressed { get; private set; }

    private void Start()
    {
        if (_normalSprite != null)
            _renderer.sprite = _normalSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;

        if (_pressedSprite != null)
            _renderer.sprite = _pressedSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;

        if (_normalSprite != null)
            _renderer.sprite = _normalSprite;
    }
}
