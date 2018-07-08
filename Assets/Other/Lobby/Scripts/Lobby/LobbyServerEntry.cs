using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;

namespace Prototype.NetworkLobby
{
    public class LobbyServerEntry : MonoBehaviour 
    {
        public Text serverInfoText;
        public Text slotInfo;
        public Button joinButton;
        public LobbyPlayer lobp;

        //int 
        public void Populate(MatchInfoSnapshot match, LobbyManager lobbyManager, Color c)
		{
            serverInfoText.text = match.name;

            slotInfo.text = match.currentSize.ToString() + "/" + match.maxSize.ToString();             
            


            NetworkID networkID = match.networkId;

            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(() => { JoinMatch(networkID, lobbyManager, match.maxSize); });
            GetComponent<Image>().color = c;
        }

        void JoinMatch(NetworkID networkID, LobbyManager lobbyManager, int max)
        {
            GameLogic.kolvoPlayers = max;
            lobbyManager.matchMaker.JoinMatch(networkID, "", "", "", 0, 0, lobbyManager.OnMatchJoined);
			lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            lobbyManager._isMatchmaking = true;
            lobbyManager.DisplayIsConnecting();
        }
    }
}