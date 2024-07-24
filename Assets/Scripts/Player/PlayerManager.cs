using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private int playerCount = 0;
    public List<Color> colors;

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Color playerColor = colors[playerCount];

        if(playerColor == null) playerColor = new Color(1,0,0,1);

        playerCount++;
        PlayerController playerController = playerInput.GetComponent<PlayerController>();
        playerController.SetPlayerId(playerCount);
        playerController.SetPlayerColor(playerColor);
    }
}
