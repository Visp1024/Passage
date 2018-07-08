using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class KolvoPlayers : MonoBehaviour {
    public Dropdown drop;
    public NetworkLobbyManager lobby;

    public void dropValueChange()
    {
        if (drop.value==0)
        {
            lobby.maxPlayers = 2;
            lobby.minPlayers = 2;
            GameLogic.kolvoPlayers = 2;
        }
        else
        {
            lobby.minPlayers = 4;
            lobby.maxPlayers = 4;
            GameLogic.kolvoPlayers = 4;
        }
    }
}
