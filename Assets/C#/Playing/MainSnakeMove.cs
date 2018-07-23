//Hello
//Hello
//Alo
using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
public class MainSnakeMove : MonoBehaviour {

    public Vector3 varForHead;
    public int speed=7;
    public List<GameObject> body;
    public GameObject tail;
    public GameObject food;
    public Text points;
    public static int point=0;

    private Vector2 startPos, endPos, direction;

    public Text isContinue;
    public Vector3 nowFoodCoord;
    private int checkLines = 0;

    public GameObject onPauseButton;
    void Start()
    {
        body = new List<GameObject>();
        point = 0;
        varForHead = Vector3.up;
        string[] toDelete = new string[] { "\r\n" };
        string readFile1 = PlayerPrefs.GetString("allData");
        string[] readFile = readFile1.Split(toDelete, StringSplitOptions.None);

        if (readFile1.Length == 0)
            LoadScene.isNewGame = true;
        if (LoadScene.isNewGame)
        {
            varForHead = Vector3.up;
            body.Add(gameObject);
            CreateFood();
        }
        else
        {
            onPauseButton.SetActive(true);
            speed = 0;
            StartCoroutine(CreateTail(readFile));

        }
    }

    void Update()
    {
        varForHead = MoveHead(varForHead);
        transform.Translate(varForHead * speed * Time.deltaTime);
        points.text = point.ToString();

    }

    public void OnPauseClick()
    {
        OnApplicationPause(true);
    }

    public void OnUnpauseClick()
    {
        speed = 7;
        onPauseButton.SetActive(false);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            speed = 0;
            onPauseButton.SetActive(true);
            SaveOnClick();
        }
    }

    public void SaveOnClick()
    {
        StringBuilder str = new StringBuilder();
        foreach (GameObject n in body)
        {
            str.Append(n.transform.position.x + " " + n.transform.position.y + " " + n.transform.position.z + "\r\n");
        }
        Vector3 foodCoord = nowFoodCoord;
        str.Append("food: " + foodCoord.x + " " + foodCoord.y + " " + foodCoord.z + "\r\n");
        str.Append("direction: " + Direction());
        PlayerPrefs.SetString("allData", str.ToString());
        PlayerPrefs.SetInt("points", MainSnakeMove.point);
        print("Application quit");
        PlayerPrefs.Save();
    }

    public string Direction()
    {
        Vector3 dir = varForHead;
        if (dir == Vector3.left)
        {
            return "left";
        }
        else if (dir == Vector3.right)
        {
            return "right";
        }
        else if (dir == Vector3.up)
        {
            return "up";
        }
        else
        {
            return "down";
        }

    }


    Vector3 MoveHead(Vector3 var)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
 
            var = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            var = Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            var = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            var = Vector3.down;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !onPauseButton.activeSelf)
        {
            startPos = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && !onPauseButton.activeSelf)
        {
            endPos = Input.GetTouch(0).position;
            direction = endPos - startPos;
            var = GetDirection(direction, var);
        }
        return var;
    }

    private Vector3 GetDirection(Vector3 pos, Vector3 nowMove)
    {
        Vector3 vectorToMove;
        if (Math.Abs(pos.x) > Math.Abs(pos.y))
        {
            if (pos.x > 0)
            {
                vectorToMove = Vector3.right;
                if (nowMove != Vector3.left)
                {
                    return vectorToMove;
                }
            }
            else
            {
                vectorToMove = Vector3.left;
                if (nowMove != Vector3.right)
                {
                    return vectorToMove;
                }
            }
        }
        else
        {
            if (pos.y > 0)
            {
                vectorToMove = Vector3.up;
                if (nowMove != Vector3.down)
                {
                    return vectorToMove;
                }
            }
            else
            {
                vectorToMove = Vector3.down;
                if (nowMove != Vector3.up)
                {
                    return vectorToMove;
                }
            }
        }
        return nowMove;
    }

    IEnumerator CreateTail(string[] coord)
    {
        while (true)
        {
            if (body.Count == coord.Length - 2)
            {
                string[] foodArr = coord[coord.Length - 2].Replace("food: ", "").Split();
                Vector3 foodPos = new Vector3(float.Parse(foodArr[0]), float.Parse(foodArr[1]),1 );
                Instantiate(food, foodPos, Quaternion.identity);
                varForHead = DirectionFromString(coord[coord.Length - 1]);
                yield break;
            }
            else
            {
                string[] strBody = coord[checkLines].Split();
                if (body.Count == 0)
                {
                    gameObject.transform.position =
                        new Vector3(float.Parse(strBody[0]), float.Parse(strBody[1]), 1 );
                    body.Add(gameObject);
                }
                else
                {
                    AddTail(new Vector3(float.Parse(strBody[0]), float.Parse(strBody[1]),1));
                }
                checkLines++;
                yield return new WaitForSeconds(0.001f);
            }
        }
    }
    private void CreateFood()
    {
        nowFoodCoord = new Vector3(UnityEngine.Random.Range(-6,6), UnityEngine.Random.Range(-14, 14), 1);
        Instantiate(food, nowFoodCoord, Quaternion.identity);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            AddTail(body[body.Count - 1].transform.position);
            CreateFood();
        }
        if ((other.gameObject.CompareTag("Tail")) || (other.gameObject.CompareTag("Border")))
        {
            print("fh");
            PlayerPrefs.SetString("allData", "");
            PlayerPrefs.SetInt("points", point);
            Debug.Log("game finished, points = " + point);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Menu");
        }
    }

    public void AddTail(Vector3 pos)
    {
        point++;
        body.Add(Instantiate(tail, pos, Quaternion.identity) as GameObject);
    }
    private Vector3 DirectionFromString(string str)
    {
        str = str.Replace("direction: ", "").Replace(" ", "");
        switch (str)
        {
            case "up":
                return Vector3.up;
            case "down":
                return Vector3.down;
            case "left":
                return Vector3.left;
            case "right":
                return Vector3.right;
            default:
                throw new Exception("Wrong string");
        }
    }
    private void OnApplicationQuit()
    {
        SaveOnClick();
    }
    public void Reclama()
    {
        if (Advertisement.IsReady())
            Advertisement.Show();
    }
}
