using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    public static string path = "SCORE.txt";
    private List<KeyValuePair<string, int>> recList = new List<KeyValuePair<string, int>>();
    private int nowResult;
    public Text score;
    public Text pointsText;
    public InputField inpName;

    private TouchScreenKeyboard keyboard;
    private void Start()
    {
        GetRecordsFromFile();

        int pts = PlayerPrefs.GetInt("points");
        Debug.Log("now playerpref points = " + pts);
        pointsText.text = pts.ToString();
        if (MainSnakeMove.point > recList.LastOrDefault().Value)
        {
            inpName.gameObject.SetActive(true);
            inpName.ActivateInputField();
            keyboard = new TouchScreenKeyboard(inpName.text, TouchScreenKeyboardType.ASCIICapable, false, false, false, false, "Please enter name");
            TouchScreenKeyboard.Open(inpName.text, TouchScreenKeyboardType.Default, false, false, false, false, "Please enter name");
        }
        nowResult = MainSnakeMove.point;
        score.text = ScoreTextForming();
    }

    private void Update()
    {
        
    }

    private string ScoreTextForming()
    {
        string str = "";
        print(recList.Count);
        for (int i = 0; i < recList.Count; i++)
        {
            str += recList[i].Key + " " + recList[i].Value + "\r\n";
        }
        return str;
    }

    private void GetRecordsFromFile()
    {
        recList = new List<KeyValuePair<string, int>>();
        string[] toDelete = new string[] { "\r\n" };
        string readFile1 = PlayerPrefs.GetString("records");
        string[] readFile = readFile1.Split(toDelete, StringSplitOptions.None);
        readFile = readFile.Take(readFile.Length - 1).ToArray();
        foreach (string str in readFile)
        {
            string[] line = str.Split();
            recList.Add(new KeyValuePair<string, int>(line[0], Convert.ToInt32(line[1])));
        }
    }

    public void OnClickOK()
    {
        if (inpName.text.Length > 0)
        {
            recList.Add(new KeyValuePair<string, int>(inpName.text.Replace(" ", ""), nowResult));
        }
        else
        {
            recList.Add(new KeyValuePair<string, int>("UnknownPlayer", nowResult));
        }
        recList = recList.OrderByDescending(n => n.Value).ToList();
        if (recList.Count > 5)
        {
            recList.Remove(recList.Last());
        }
        score.text = ScoreTextForming();
        inpName.gameObject.SetActive(false);
        PlayerPrefs.SetString("records", score.text);
        PlayerPrefs.Save();
    }

    public void OnKeyboard()
    {
        
        TouchScreenKeyboard.Open(keyboard.text, TouchScreenKeyboardType.ASCIICapable, true, false);
       
    }
}
