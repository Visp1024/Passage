using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralOptions : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        enturage = PlayerPrefs.GetInt("enturage");
        feld = PlayerPrefs.GetInt("feld");
        EnturageActiv();
    }

    int enturage = 0;
    int feld=0;

	// Update is called once per frame
	public void EnturageActiv ()
    {
        switch (enturage)
        {
            default:
                {
                    next.interactable = true;
                    back.interactable = false;
                    enturageImage[0].SetActive(true);
                    enturageImage[1].SetActive(false);
                    enturageImage[2].SetActive(false);
                    entText.text = "Белая комната";
                }
                break;
            case 1:
                {
                    next.interactable = true;
                    back.interactable = true;
                    enturageImage[0].SetActive(false);
                    enturageImage[1].SetActive(true);
                    enturageImage[2].SetActive(false);
                    entText.text = "Загородный дом";
                }
                break;
            case 2:
                {
                    next.interactable = false;
                    back.interactable = true;
                    enturageImage[0].SetActive(false);
                    enturageImage[1].SetActive(false);
                    enturageImage[2].SetActive(true);
                    entText.text = "Библиотека";
                }
                break;
        }
    }

    public GameObject[] enturageImage;
    public Button next;
    public Button back;
    public Text entText;

    public void EntNextButton()
    {
        if (enturage<2)
            enturage++;
        EnturageActiv();
        PlayerPrefs.SetInt("enturage", enturage);
    }

    public void EntBackButton()
    {
        if (enturage > 0)
            enturage--;
        EnturageActiv();
        PlayerPrefs.SetInt("enturage", enturage);
    }
}
