using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetTest : NetworkBehaviour {

    public GameObject field;
    
 
    [SyncVar]
    public Color color;
    [SyncVar]
    public string playerName;
    GameHelper _gameHelper;
    
    //GameObject mescounter;
    //Text mescountertext;
    
    // Use this for initialization
    void Start ()
    {
        _gameHelper = GameObject.FindObjectOfType<GameHelper>();
        _gameHelper.readyScreen.SetActive(true);


        if (isLocalPlayer)
        {
            _gameHelper.net = this;
        }

        field = GameObject.Find("Field");

        if (isLocalPlayer)
        {                     
            gameObject.name = "localNet";
            GameLogic gml = field.GetComponent<GameLogic>();
            gml.stopgame = true;
            gml.networkgame = true;
            gml.netobj = gameObject;

            if (GameLogic.kolvoPlayers==2)
            {
                if (color == Color.green)
                    gml.Yhod(1);
                if (color != Color.green)
                    gml.Yhod(2);
            }
            else
            {
                if (color == Color.green)
                    gml.Yhod(1);
                if (color == Color.red)
                    gml.Yhod(2);
                if (color == Color.blue)
                    gml.Yhod(3);
                if (color == Color.yellow)
                    gml.Yhod(4);
            }          
            _gameHelper.chat.SetActive(true);
        }
        CmdReady();
    }

    int readycount = 0;

    [Command]
    public void CmdReady()
    {
        readycount++;
        if (readycount== GameLogic.kolvoPlayers)
        {
            RpcReady();
            field.GetComponent<GameLogic>().stopgame = false;
        }
    }

    [ClientRpc]
    public void RpcReady()
    {
        _gameHelper.readyScreen.SetActive(false);
        field.GetComponent<GameLogic>().stopgame = false;
    }

    [Command]
    public void CmdSend(string message)
    {
        print(color);
        string col="";
        if (color == Color.green)
            col = "Green";
        if (color == Color.red)
            col = "Red";
        if (color == Color.blue)
            col = "Blue";
        if (color == Color.yellow)
            col = "Yellow";
        message = "<color=" + col + ">" + playerName + "</color>: " + message;
        RpcSend(message);
    }

    [ClientRpc]
    public void RpcSend(string message)
    {
        _gameHelper.textbloc.text += System.Environment.NewLine + message;
        Canvas.willRenderCanvases += EvAddScroll;;
        if (_gameHelper.chat.transform.localScale.x < 0.2f)
        {
            _gameHelper.mescounterint++;
            _gameHelper.mescounter.SetActive(true);
            _gameHelper.mescountertext.text = _gameHelper.mescounterint + "";
        }
        else
        {
            _gameHelper.mescounterint = 0;
        }
    }

    void EvAddScroll()
    {
        _gameHelper.textbloc.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
        Canvas.willRenderCanvases -= EvAddScroll;
    }

    [ClientRpc]
    public void RpcQwer(Vector3 playerVect, int xx, int yy, int hod)
    {
        field.GetComponent<GameLogic>().MovePlayers(playerVect,xx,yy,hod);
    }

    [Command]
    public void CmdQwer(Vector3 playerVect, int xx, int yy, int hod)
    {
        RpcQwer(playerVect, xx, yy, hod);
    }

    [ClientRpc]
    public void RpcEdg(Vector3 playerVect, Quaternion rot, int modif, int x, int y)
    {
        field.GetComponent<GameLogic>().MoveEdge(playerVect, rot, modif, x ,y);
    }

    [Command]
    public void CmdEdg(Vector3 playerVect, Quaternion rot, int modif, int x, int y)
    {
        RpcEdg(playerVect, rot, modif, x, y);
    }

}
