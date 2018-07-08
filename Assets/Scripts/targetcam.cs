using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class targetcam : MonoBehaviour
{

    public GameObject target = null;
    // Use this for initialization

    private Vector3 MousePos;
    //private Ray ray;
    //private RaycastHit hit;
    private float MyAngle = 0F;
    public GameObject escMenu;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            transform.LookAt(target.transform);

        MousePos = Input.mousePosition;

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!_paused)
                escMenu.SetActive(true);
            else
                escMenu.SetActive(false);

            _paused = !_paused;
        }

    }

    public static int connlen;
    private bool _paused = false;
    private int _window = 100;

    public void Exit()
    {
        Application.Quit();
    }
    public void LoadMainmenu()
    {
        SceneManager.LoadScene(0);
        FindObjectOfType<NetworkLobbyManager>().GetComponent<Prototype.NetworkLobby.LobbyManager>().GoBackButton();
        //Destroy(GameObject.Find("LobbyManager"));
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(2))
        {
            MyAngle = 2 * ((MousePos.x - (Screen.width / 2)) / Screen.width);
            transform.RotateAround(target.transform.position, transform.up, MyAngle);
            if (transform)
            {
                MyAngle = 2 * ((MousePos.y - (Screen.height / 2)) / Screen.height);
                transform.RotateAround(target.transform.position, transform.right, -MyAngle);
            }
            else
            {
                MyAngle = 2 * ((MousePos.y + 1 - (Screen.height / 2)) / Screen.height);
                transform.RotateAround(target.transform.position, transform.right, -MyAngle);
            }

            // расчитываем угол, как:
            // разница между позицией мышки и центром экрана, делённая на размер экрана
            //  (чем дальше от центра экрана тем сильнее поворот)
            // и умножаем угол на чуствительность из параметров

        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0f && transform.position.y < 1)
        {
            if (!GameObject.Find("Blocker"))
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && transform.position.y> 0.1)
        {
            if (!GameObject.Find("Blocker"))
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        }
    }
}




