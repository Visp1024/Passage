using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    //public int xxx;
    //public int yyy;
    // Use this for initialization
    public GameObject go;
    public Material grenMateriall;
    public Material blackwoodMaterial;
    public GameObject EdgesPrebuf = null;
    public GameObject[] playerarray = new GameObject[5];
    int[] edg_array = new int[5];
    public Text hodind = null;
    //public Text a1 = null;
    public Text waynotfound = null;
    public Animation findway;
    public GameObject[,] ota = new GameObject[17, 17];
    static public int[,] logic = new int[17, 17];
    int layermask = 1 << 9;
    //int layerr = 0;
    int mod = 0;
    private Transform tr;
    Vector3 positionBuf;
    //Quaternion rotationBuf;
    //GameObject edg;
    public AudioSource knock;
    public AudioSource winaudio;
    //[SyncVar]
    public int hod = 1;
    public int[,] bufarrayposition = new int[5, 2];
    int xhit, yhit, xx = -10, yy = -10;

    public static int kolvoPlayers = 2;
    //bool twoPlayers = true;
    public GameObject netobj = null;
    public bool networkgame = false;
    public GameObject winimage;
    public GameObject winipart;
    public Text wintext = null;
    public GameObject EdgCounter3;
    public GameObject EdgCounter4;
    public Text[] EdgCounters;
    public Button winreload;


    //обнулить 
    public int yourhod = 1;

    void Start()
    {        
        logic = new int[17, 17];
        logic[16, 8] = 1;
        bufarrayposition[1, 0] = 16;
        bufarrayposition[1, 1] = 8;

        logic[0, 8] = 2;
        bufarrayposition[2, 0] = 0;
        bufarrayposition[2, 1] = 8;

        if (kolvoPlayers == 2)
        {
            edg_array[1] = 10;
            edg_array[2] = 10;
            Destroy(playerarray[3]);
            Destroy(playerarray[4]);
            Destroy(EdgCounter3);
            Destroy(EdgCounter4);
        }
        else
        {
            logic[8, 0] = 3;
            bufarrayposition[4, 0] = 8;
            bufarrayposition[4, 1] = 0;

            logic[8, 16] = 4;
            bufarrayposition[3, 0] = 8;
            bufarrayposition[3, 1] = 16;

            edg_array[1] = 5;
            edg_array[2] = 5;
            edg_array[3] = 5;
            edg_array[4] = 5;
        }


        //Заполняем массив GameObject
        foreach (GameObject cubs in GameObject.FindGameObjectsWithTag("Cubs"))
        {
            int x1 = int.Parse(cubs.name.Remove(1));
            int y1 = int.Parse(cubs.name.Remove(0, 1));
            x1 = (x1 - 1) + x1 + 1;
            y1 = (y1 - 1) + y1 + 1;
            ota[x1, y1] = cubs;
            logic[x1, y1] = 5;
        }

        foreach (GameObject spheres in GameObject.FindGameObjectsWithTag("Spheres"))
        {
            int x1 = int.Parse(spheres.name.Remove(1));
            int y1 = int.Parse(spheres.name.Remove(0, 1));
            x1 = (x1 - 1) + x1 + 2;
            y1 = (y1 - 1) + y1 + 2;
            ota[x1, y1] = spheres;
            logic[x1, y1] = 9;
        }

        Printarray();
        EdgCounterUpdate();
        tr = playerarray[1].transform;
        positionBuf = playerarray[1].transform.position;

        //netobj.GetComponent<NetTest>().RpcReady();
    }



    public void EdgCounterUpdate()
    {
        for (int i = 0; i < 4; i++)
            EdgCounters[i].text = edg_array[i + 1]+"";
    }

    public void Yhod(int a)
    {
        Camera.main.transform.position = GameObject.Find("CamPos" + a).transform.position;
        yourhod = a;
        //print(a);
        if (a != 1)
            CubsOff();
        tr = playerarray[a].transform;
        positionBuf = playerarray[a].transform.position;

    }

    // Update is called once per frame

    //удалить в продакшене!!!!
    void Printarray()
    {
        string col = "";
        if (hod == 1)
            col = "Green";
        if (hod == 2)
            col = "Red";
        if (hod == 3)
            col = "Blue";
        if (hod == 4)
            col = "Yellow";
        hodind.text = "<color=" + col + ">Ходит игрок " + hod + "</color>";
    }


    //Модификатор хода
    void ChangeMod()
    {
        GameObject moveedg = EdgesPrebuf;
        moveedg.name = "bufedg";
        if (mod == 0 && edg_array[hod]>0)
        {
            CubsOff();
            tr.position = positionBuf;
            Instantiate(moveedg);

            
            tr = GameObject.Find("bufedg(Clone)").transform;
            tr.SetParent(this.transform);
            tr.rotation = Quaternion.Euler(0, 0, 0);
            tr.localPosition = new Vector3(0, 6, 0);
            layermask = 1 << 10;
            mod = 1;
        }
        else
        {
            if (mod == 1)
            {
                tr.localPosition = new Vector3(0, 6, 0);
                tr.rotation = Quaternion.Euler(0, 90, 0);
                mod = 2;
            }
            else
            {
                Destroy(GameObject.Find("bufedg(Clone)"));
                tr = playerarray[hod].transform;
                positionBuf = tr.position;
                layermask = 1 << 9;
                mod = 0;
                CubsOn(hod, false);
            }
        }

    }


    bool FindWave(int x, int y)
    {
        //bool add = true;
        bool falseEndFind = false;

        int[,] BufMap = (int[,])logic.Clone();
        //print(logic[x, y]);
        BufMap[x, y] = 8;
        if (mod == 1)
        {
            BufMap[x, y + 1] = 9;
            BufMap[x, y - 1] = 9;
        }
        else
        {
            BufMap[x + 1, y] = 9;
            BufMap[x - 1, y] = 9;
        }
        //print(BufMap[x, y] + " " + logic[x, y]);


        for (int pl = 1; pl <= kolvoPlayers; pl++) //Проверяем для каждого игрока
        {
            int[,] cMap = new int[17, 17];
            int step = 0;
            bool trueEndFind = true;

            for (int y1 = 0; y1 < 17; y1++)
                for (int x1 = 0; x1 < 17; x1++)
                {
                    if (BufMap[y1, x1] == 9 || BufMap[y1, x1] == 8)
                        cMap[y1, x1] = -2;//индикатор стены
                    else
                        cMap[y1, x1] = -1;//индикатор еще не ступали сюда
                }

            cMap[bufarrayposition[pl, 0], bufarrayposition[pl, 1]] = 0;//Начинаем с позиции игрока

            while (trueEndFind)
            {
                for (int i = 0; i < 17; i++)
                    for (int z = 0; z < 17; z++)
                    {
                        if (cMap[z, i] == step)
                        {
                            //Ставим значение шага+1 в соседние ячейки (если они проходимы)
                            if (z - 1 >= 0)
                                if (cMap[z - 1, i] == -1)
                                {
                                    falseEndFind = true;
                                    cMap[z - 1, i] = step + 1;
                                }
                            if (z + 1 < 17)
                                if (cMap[z + 1, i] == -1)
                                {
                                    falseEndFind = true;
                                    cMap[z + 1, i] = step + 1;
                                }
                            if (i - 1 >= 0)
                                if (cMap[z, i - 1] == -1)
                                {
                                    falseEndFind = true;
                                    cMap[z, i - 1] = step + 1;
                                }
                            if (i + 1 < 17)
                                if (cMap[z, i + 1] == -1)
                                {
                                    falseEndFind = true;
                                    cMap[z, i + 1] = step + 1;
                                }
                        }
                    }
                step++;

                if (pl == 1)
                    for (int i = 0; i < 17; i++)
                        if (cMap[0, i] != -1 && cMap[0, i] != -2)//решение найдено
                        {
                            //  b.text = "Путь есть";
                            trueEndFind = false;
                        }
                if (pl == 2)
                    for (int i = 0; i < 17; i++)
                        if (cMap[16, i] != -1 && cMap[16, i] != -2)//решение найдено
                        {
                            // b.text = "Путь есть";
                            trueEndFind = false;
                        }
                if (pl == 3)
                    for (int i = 0; i < 17; i++)
                        if (cMap[i, 0] != -1 && cMap[i, 0] != -2)//решение найдено
                        {
                            // b.text = "Путь есть";
                            trueEndFind = false;
                        }
                if (pl == 4)
                    for (int i = 0; i < 17; i++)
                        if (cMap[i, 16] != -1 && cMap[i, 16] != -2)//решение найдено
                        {
                            // b.text = "Путь есть";
                            trueEndFind = false;
                        }

                if (!falseEndFind)//решение не найдено
                {
                    string col = "";
                    if (pl == 1)
                        col = "Green";
                    if (pl == 2)
                        col = "Red";
                    if (pl == 3)
                        col = "Blue";
                    if (pl == 4)
                        col = "Yellow";
                    waynotfound.text = "<Color=" + col + ">Игрок " + pl + "</color> не пройдет";

                    findway.Play();
                    //print("Cтоп" + pl);
                    return false;
                }
                else
                {
                    falseEndFind = false;
                }
            }
        }
        return true;
    }
    //bool letflag;

    void CubsOn(int hod, bool letflag)
    {
        //int let = 0;
        if (mod == 0)
        {
            if (bufarrayposition[hod, 0] - 2 >= 0)
                if (logic[bufarrayposition[hod, 0] - 1, bufarrayposition[hod, 1]] == 0)
                    if (logic[bufarrayposition[hod, 0] - 2, bufarrayposition[hod, 1]] == 5)
                    {
                        ota[bufarrayposition[hod, 0] - 2, bufarrayposition[hod, 1]].GetComponent<Renderer>().material = grenMateriall;
                        ota[bufarrayposition[hod, 0] - 2, bufarrayposition[hod, 1]].layer = 9;
                    }
                    else
                    {
                        if (bufarrayposition[hod, 0] - 4 >= 0)
                            if (logic[bufarrayposition[hod, 0] - 3, bufarrayposition[hod, 1]] == 0)
                                if (logic[bufarrayposition[hod, 0] - 4, bufarrayposition[hod, 1]] == 5 && !letflag)
                                {
                                    ota[bufarrayposition[hod, 0] - 4, bufarrayposition[hod, 1]].GetComponent<Renderer>().material = grenMateriall;
                                    ota[bufarrayposition[hod, 0] - 4, bufarrayposition[hod, 1]].layer = 9;
                                }
                                else
                                {
                                    if (!letflag)
                                    {
                                        CubsOn(logic[bufarrayposition[hod, 0] - 2, bufarrayposition[hod, 1]], true);
                                    }
                                }
                            else
                            {
                                if (!letflag)
                                {
                                    CubsOn(logic[bufarrayposition[hod, 0] - 2, bufarrayposition[hod, 1]], true);
                                }
                            }
                    }
            if (bufarrayposition[hod, 0] + 2 <= 16)
                if (logic[bufarrayposition[hod, 0] + 1, bufarrayposition[hod, 1]] == 0)
                    if (logic[bufarrayposition[hod, 0] + 2, bufarrayposition[hod, 1]] == 5)
                    {
                        ota[bufarrayposition[hod, 0] + 2, bufarrayposition[hod, 1]].GetComponent<Renderer>().material = grenMateriall;
                        ota[bufarrayposition[hod, 0] + 2, bufarrayposition[hod, 1]].layer = 9;
                    }
                    else
                    {
                        if (bufarrayposition[hod, 0] + 4 <= 16)
                            if (logic[bufarrayposition[hod, 0] + 3, bufarrayposition[hod, 1]] == 0)
                                if (logic[bufarrayposition[hod, 0] + 4, bufarrayposition[hod, 1]] == 5 && !letflag)
                                {
                                    ota[bufarrayposition[hod, 0] + 4, bufarrayposition[hod, 1]].GetComponent<Renderer>().material = grenMateriall;
                                    ota[bufarrayposition[hod, 0] + 4, bufarrayposition[hod, 1]].layer = 9;
                                }
                                else
                                {
                                    if (!letflag)
                                    {
                                        CubsOn(logic[bufarrayposition[hod, 0] + 2, bufarrayposition[hod, 1]], true); ;
                                    }
                                }
                            else
                            {
                                if (!letflag)
                                {
                                    CubsOn(logic[bufarrayposition[hod, 0] + 2, bufarrayposition[hod, 1]], true); ;
                                }
                            }
                    }
            if (bufarrayposition[hod, 1] - 2 >= 0)
                if (logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] - 1] == 0)
                    if (logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] - 2] == 5)
                    {
                        ota[bufarrayposition[hod, 0], bufarrayposition[hod, 1] - 2].GetComponent<Renderer>().material = grenMateriall;
                        ota[bufarrayposition[hod, 0], bufarrayposition[hod, 1] - 2].layer = 9;
                    }
                    else
                    {
                        if (bufarrayposition[hod, 1] - 4 >= 0)
                            if (logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] - 3] == 0)
                                if (logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] - 4] == 5 && !letflag)
                                {
                                    ota[bufarrayposition[hod, 0], bufarrayposition[hod, 1] - 4].GetComponent<Renderer>().material = grenMateriall;
                                    ota[bufarrayposition[hod, 0], bufarrayposition[hod, 1] - 4].layer = 9;
                                }
                                else
                                {
                                    if (!letflag)
                                    {
                                        CubsOn(logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] - 2], true);
                                    }
                                }
                            else
                            {
                                if (!letflag)
                                {
                                    CubsOn(logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] - 2], true);
                                }
                            }
                    }

            if (bufarrayposition[hod, 1] + 2 <= 16)
                if (logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] + 1] == 0)
                    if (logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] + 2] == 5)
                    {
                        ota[bufarrayposition[hod, 0], bufarrayposition[hod, 1] + 2].GetComponent<Renderer>().material = grenMateriall;
                        ota[bufarrayposition[hod, 0], bufarrayposition[hod, 1] + 2].layer = 9;
                    }
                    else
                    {
                        if (bufarrayposition[hod, 1] + 4 <= 16)
                            if (logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] + 3] == 0)
                                if (logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] + 4] == 5 && !letflag)
                                {
                                    ota[bufarrayposition[hod, 0], bufarrayposition[hod, 1] + 4].GetComponent<Renderer>().material = grenMateriall;
                                    ota[bufarrayposition[hod, 0], bufarrayposition[hod, 1] + 4].layer = 9;
                                }
                                else
                                {
                                    if (!letflag)
                                    {
                                        CubsOn(logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] + 2], true);
                                    }
                                }
                            else
                            {
                                if (!letflag)
                                {
                                    CubsOn(logic[bufarrayposition[hod, 0], bufarrayposition[hod, 1] + 2], true);
                                }
                            }
                    }
        }
    }

    void CubsOff()
    {
        foreach (GameObject cb in GameObject.FindGameObjectsWithTag("Cubs"))
        {
            cb.GetComponent<Renderer>().material = blackwoodMaterial;
            cb.layer = 0;
        }
    }

    //Сетевые ребра
    public void MoveEdge(Vector3 pos, Quaternion rot, int modif, int x, int y)
    {
        knock.Play();
        logic[x, y] = 8;
        if (modif == 1)
        {
            logic[x, y + 1] = 9;
            logic[x, y - 1] = 9;
        }
        else
        {
            logic[x + 1, y] = 9;
            logic[x - 1, y] = 9;
        }
        edg_array[hod]--;
        EdgCounterUpdate();
        GameObject dstredg= GameObject.Find("bufedg(Clone)");
        GameObject moveedg = EdgesPrebuf;
        moveedg.name = "edg";
        moveedg.transform.position = pos;
        moveedg.transform.rotation = rot;
        Instantiate(moveedg);
        Destroy(dstredg);

        if (hod < kolvoPlayers)
            hod++;
        else
            hod = 1;
        CubsOff();
        //a1.text += " " + hod;
    }

    public void MovePlayers(Vector3 vect, int x, int y, int playerhod)
    {
        knock.Play();
        playerarray[playerhod].transform.position = vect;
        logic[bufarrayposition[playerhod, 0], bufarrayposition[playerhod, 1]] = 5;
        bufarrayposition[playerhod, 0] = x;
        bufarrayposition[playerhod, 1] = y;
        logic[x, y] = hod;
        CubsOff();

        //if (playerhod==yourhod)
        switch (playerhod)
        {
            case 1:
                {
                    if (x == 0)
                    {
                        // "<color=" + col + ">"
                        if (networkgame)
                            winreload.interactable = false;
                        stopgame = true;
                        winimage.SetActive(true);
                        winipart.SetActive(true);
                        winaudio.Play();
                        wintext.text = "Победил <Color=Green> игрок 1 </color>";
                    }
                    break;
                }
            case 2:
                {
                    if (x == 16)
                    {
                        if (networkgame)
                            winreload.interactable = false;
                        stopgame = true;
                        winimage.SetActive(true);
                        winipart.SetActive(true);
                        winaudio.Play();
                        wintext.text = "Победил <Color=Red> игрок 2 </color>";
                    }
                    break;
                }
            case 3:
                {
                    if (y == 0)
                    {
                        if (networkgame)
                            winreload.interactable = false;
                        stopgame = true;
                        winimage.SetActive(true);
                        winipart.SetActive(true);
                        winaudio.Play();
                        wintext.text = "Победил <Color=Blue> игрок 3 </color>";
                    }
                    break;
                }
            case 4:
                {
                    if (y == 16)
                    {
                        if (networkgame)
                            winreload.interactable = false;
                        stopgame = true;
                        winimage.SetActive(true);
                        winipart.SetActive(true);
                        winaudio.Play();
                        wintext.text = "Победил <Color=Yellow> игрок 4 </color>";
                    }
                    break;
                }
        }

        if (hod < kolvoPlayers)
            hod++;
        else
            hod = 1;
        Printarray();
    }


    public bool stopgame;

    void Update()
    {
        //print(stopgame);
        if (!go.activeSelf && !stopgame)
        {
            if ((networkgame && hod == yourhod) || !networkgame)
            {
                Ray ray;
                RaycastHit hit;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool missray = true;
                //layerr = 9;

                CubsOn(hod, false);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
                {
                    missray = false;
                    string objname = hit.transform.name;
                    xhit = int.Parse(objname.Remove(1));
                    yhit = int.Parse(objname.Remove(0, 1));
                    if (mod == 0)
                    {
                        tr.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 0.05f, hit.transform.position.z);
                        xx = (xhit - 1) + xhit + 1;
                        yy = (yhit - 1) + yhit + 1;
                    }
                    else
                    {
                        xhit = (xhit - 1) + xhit + 2;
                        yhit = (yhit - 1) + yhit + 2;
                        if (logic[xhit, yhit] != 8)
                            if (mod == 1)
                            {
                                if (logic[xhit, yhit - 1] == 0 && logic[xhit, yhit + 1] == 0)
                                {
                                    tr.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 0.05f, hit.transform.position.z);
                                    xx = xhit;
                                    yy = yhit;
                                }
                            }
                            else
                            if (logic[xhit + 1, yhit] == 0 && logic[xhit - 1, yhit] == 0)
                            {
                                tr.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 0.05f, hit.transform.position.z);
                                xx = xhit;
                                yy = yhit;
                            }
                    }
                }

                if (Input.GetMouseButtonDown(1))
                    ChangeMod();//Запускаем модификатор хода

                if (Input.GetMouseButtonDown(0) && !missray)
                {

                    if (xx != -10)
                    {
                        
                        if (mod != 0)
                        {
                            if (FindWave(xx, yy))
                            {
                                tr.position = new Vector3(tr.transform.position.x, tr.transform.position.y - 0.025f, tr.transform.position.z);
                                
                                if (!networkgame)
                                    MoveEdge(tr.position, tr.rotation, mod, xx, yy);
                                else
                                {
                                    netobj.GetComponent<NetTest>().CmdEdg(tr.position, tr.rotation, mod, xx, yy);
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (!networkgame)
                            {
                                MovePlayers(new Vector3(tr.transform.position.x, tr.transform.position.y - 0.028f, tr.transform.position.z), xx, yy, hod);
                            }
                            else
                            {
                                netobj.GetComponent<NetTest>().CmdQwer(new Vector3(tr.transform.position.x, tr.transform.position.y - 0.03f, tr.transform.position.z), xx, yy, hod);
                            }
                        }

                        tr = playerarray[hod].transform;
                        positionBuf = tr.position;
                        mod = 0;
                        layermask = 1 << 9;
                        xx = -10;
                        yy = -10;
                    }
                }
            }
        }
    }

}
