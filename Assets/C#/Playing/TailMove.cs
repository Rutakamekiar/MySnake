using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailMove : MonoBehaviour {

    public Vector3 tailPos;
    public GameObject tail;
    public MainSnakeMove mainSnake;
    void Start () {
        mainSnake = GameObject.FindGameObjectWithTag("MainSnake").GetComponent<MainSnakeMove>();
        tail = mainSnake.body[mainSnake.body.Count - 2];
        int size = mainSnake.body.Count;
        if (size == 2 || size == 3)
            gameObject.tag = "FirstTail";
    }
	

	void Update () {
        tailPos = tail.transform.position;
        transform.position = Vector3.Lerp(transform.position, tailPos, Time.deltaTime * mainSnake.speed);
    }
}
