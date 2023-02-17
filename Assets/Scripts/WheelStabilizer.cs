using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WheelStabilizer : MonoBehaviour
{
    private float _xPos;


    private void Start()
    {
        _xPos = transform.localPosition.x;
    }

    private void LateUpdate()
    {
        Vector3 localPos = transform.localPosition;
        localPos.x = _xPos;
        transform.localPosition = localPos;
    }
}
