using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour {

    public bool chat;
    public Animation ChatAnimation;

    public void ChatSizeChange()
    {
        if (!chat)
        {
            ChatAnimation.Play("Chat_max");
        }
        else
        {
            ChatAnimation.Play("Chat_min");
        }
        chat = !chat;
    }
}
