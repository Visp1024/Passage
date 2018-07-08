using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHelper : MonoBehaviour {

    //public Button btn;
    public InputField input;
    public Text textbloc;
    public Scrollbar scrollbar;
    public GameObject chat;
    public GameObject mescounter;
    public Text mescountertext;
    public GameObject readyScreen;
    public int mescounterint=0;

    private NetTest _net;

    public NetTest net
    {
        get { return _net; }
        set { _net = value; }
    }

    public void Send () {
        if (input.text != "" )
        {
            _net.CmdSend(input.text);
            input.text = "";
        }
	}

    public void ClearChatCounter()
    {
        mescounter.SetActive(false);
        mescounterint = 0;
    }

   public void EntMessage()
    {
        if (Input.GetKeyDown(KeyCode.Return) && input.text != "")
            Send();        
    }
}
