using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleButton : Selectable, IPointerDownHandler, IPointerUpHandler
{
    public bool IsPressed => base.IsPressed();
}
