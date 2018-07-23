﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;

public class SaveProgress : MonoBehaviour {

    public static bool gameIsPaused = false;
    public GameObject unpauseButton;

    public MainSnakeMove snake; 
	// Use this for initialization
	void Start () {
	    snake = GameObject.FindGameObjectWithTag("MainSnake").GetComponent<MainSnakeMove>();
    }
	
	// Update is called once per frame
	void Update () {
        //snake = GameObject.FindGameObjectWithTag("MainSnake").GetComponent<MainSnakeMove>();
        //if (Input.GetKeyDown(KeyCode.F9))
        //{
        //    //OnOffGame();
        //    gameIsPaused = !gameIsPaused;
        //    OnApplicationPause(gameIsPaused);
        //}
        //Debug.Log("game is paused: " + gameIsPaused);
    }



    public void SaveOnClick()
    {
        StringBuilder str = new StringBuilder();
        foreach (GameObject n in snake.body)
        {
            str.Append(n.transform.position.x + " " + n.transform.position.y + " " + n.transform.position.z + "\r\n");
        }
        Vector3 foodCoord = snake.nowFoodCoord;
        str.Append("food: " + foodCoord.x + " " + foodCoord.y + " " + foodCoord.z + "\r\n");
        str.Append("direction: " + Direction());
        PlayerPrefs.SetString("allData", str.ToString());
        PlayerPrefs.SetInt("points", MainSnakeMove.point);
        print("Application quit");
        PlayerPrefs.Save();
    }

    public string Direction()
    {
        Vector3 dir = snake.varForHead;
        if (dir == Vector3.left)
        {
            return "left";
        }   
        else if (dir == Vector3.right)
        {
            return "right";
        }
        else if (dir == Vector3.forward)
        {
            return "forward";
        }
        else
        {
            return "back";
        }
        
    }
}
