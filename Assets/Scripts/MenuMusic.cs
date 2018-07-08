using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{

    private static MenuMusic _instance;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        //if we don't have an [_instance] set yet
        if (!_instance)
            _instance = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);
    }
}