using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetAudio : MonoBehaviour {

    public Slider sld;
	// Use this for initialization
	void Start () {
        sld.onValueChanged.AddListener(delegate { SetSound(); });
	}

    void SetSound()
    {
        GetComponent<AudioSource>().volume = sld.value;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
