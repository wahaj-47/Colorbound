using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMarker : MonoBehaviour
{
    [SerializeField]
    private Image marker;
    public void ShowMarker()
    {
        marker.enabled = true;
    }
    public void HideMarker()
    {
        marker.enabled = false;
    }
    public void SetColor(int id, Color color)
    {
        if(id < 1) HideMarker();
        else ShowMarker();

        marker.color = color;
    }
}
