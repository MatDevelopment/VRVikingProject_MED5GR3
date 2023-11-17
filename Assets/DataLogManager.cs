using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.Threading;
using System.IO;

public class DataLogManager : MonoBehaviour
{
    [Header ("Find Objects")]
    public LevelChanger levelChanger;
    public GameObject gameObject;

    [Header ("File Related")]
    // Variables for outputting the log file
    [SerializeField] string newFileName = "";
    private StreamWriter sw;
    static string Filename = "";
    string path;
    DateTimeOffset localTime;

    // Variables to catch from other scripts
    [Header ("Logged Variables")]
    public static int NPCGazeTime;
    public static int PromptAmount;
    public static int TotalTime;
    public static int ProximityTime;

    // Keep script intact
    static DataLogManager dataLogManager;

    void Awake()
    {
        // Don't fuck with varaibles
        DontDestroyOnLoad(transform.gameObject);

        // Get the current date and time in GMT+2
        localTime = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(2));

        // Get LevelChanger
        levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();

        // Don't overwrite file
        if (PlayerPrefs.GetString("Filename", Filename) != "" && PlayerPrefs.GetString("Filename", Filename) != null)
        {
        Filename = PlayerPrefs.GetString("Filename", Filename);
        }
        else
        {
            Filename = "emergencyLog";
        }

        if (newFileName != "")
        {
            if (newFileName != Filename)
            {
                Filename = newFileName;
            }
        }

        CreateLogText();
    }

    // Start is called before the first frame update
    void Start()
    {
        dataLogManager = gameObject.AddComponent<DataLogManager>();
        // Creating a new Streamwriter object with desired path
        sw = new StreamWriter(path);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {
        LogUpdate();
        sw.Close();
    }

    private void CreateLogText()
    {
        // Get current time and date
        string filedate = localTime.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture) + ".txt";

        // Define the path of the log file
        path = Application.dataPath + "/TestLogs" + "/" + Filename + filedate;

        // Write to the file
        File.WriteAllText(path, "");
    }

    public void LogUpdate()
    {
        int currentTime = (int)Mathf.Round(Time.time);
        // Defining what is written in the log text
        string SensorLogText = "Gaze time on NPC = " + NPCGazeTime + ", Prompt Amount = " + PromptAmount + ", Proximity Time = " + ProximityTime + ", Experience Time = " + currentTime;
        // Appending the string to the textfile which means it is written behind the current text
        sw.WriteLine(SensorLogText);
    }
}
