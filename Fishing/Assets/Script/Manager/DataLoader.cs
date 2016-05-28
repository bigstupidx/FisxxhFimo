using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

[System.Serializable]
public class Level
{
    public int Lv;
    public int GoldGoal;
    public string FishGoal;
    public int NumberFishGoal;
    public List<string> FishList;

    public Level()
    {
        GoldGoal = 0;
        FishGoal = String.Empty;
        NumberFishGoal = 0;
        FishList = new List<string>();
    }
}

public class DataLoader : MonoSingleton<DataLoader>
{
    public List<Level> ListLevels = new List<Level>();
    public Dictionary<string, int> FishPrice = new Dictionary<string, int>();

    //public Transform[] WayPoint;
    public List<Vector3[]> ListPath = new List<Vector3[]>();
    // Use this for initialization
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        //WayPoint = GameObject.Find("ini").GetComponentsInChildren<Transform>();

        //LoadLevelData();
        LoadPath();
    }
    #region LoadData
    public void LoadLevelData()
    {
        string[] fishPrice = null;
        TextAsset data = Resources.Load<TextAsset>("GameData/Data");
        if (data != null)
        {
            string dataText = data.text;
            var dataTextArr = Regex.Split(dataText, "\n");
            var fishName = dataTextArr[0].Split(new string[] { "\t" }, StringSplitOptions.None);
            fishPrice = dataTextArr[1].Split(new string[] { "\t" }, StringSplitOptions.None);
            for (int i = 3; i < fishName.Length; i++)
            {
                FishPrice.Add(fishName[i].Trim(), int.Parse(fishPrice[i]));
            }
            Level tempLevel;
            int currentLv = 0;
            for (int index = 2; index < dataTextArr.Length; index++)
            {
                tempLevel = new Level();
                tempLevel.Lv = currentLv;
                var s = dataTextArr[index];
                if (s != null)
                {
                    string[] path = s.Split(new string[] { "\t" }, StringSplitOptions.None);
                    int gold;
                    if (int.TryParse(path[2], out gold))
                    {
                        tempLevel.GoldGoal = gold;
                    }
                    for (int i = 3; i < path.Length; i++)
                    {
                        if (int.Parse(path[i]) != -1)
                        {
                            if (int.Parse(path[i]) != 0)
                            {
                                tempLevel.FishGoal = fishName[i];
                                tempLevel.NumberFishGoal = int.Parse(path[i]);
                            }
                            tempLevel.FishList.Add(fishName[i].Trim());
                        }
                    }
                    ListLevels.Add(tempLevel);
                }
                currentLv++;
            }
        }
    }

    public void LoadPath()
    {
        TextAsset data = Resources.Load<TextAsset>("GameData/Path");
        if (data != null)
        {
            string dataText = data.text;
            var dataTextArr = Regex.Split(dataText, "\n");
            foreach (var s in dataTextArr)
            {
                string[] pos = s.Split(new string[] { "\t" }, StringSplitOptions.None);
                int k = 0;
                Vector3 temp = new Vector3();
                List<Vector3> listVec3 = new List<Vector3>();
                for (int i = 0; i < pos.Length; i++)
                {
                    if (k == 0)
                    {
                        temp.x = float.Parse(pos[i]);
                        k = 1;
                    }
                    else
                    {
                        temp.y = float.Parse(pos[i]);
                        temp.z = 0;
                        listVec3.Add(temp);
                        temp = new Vector3();
                        k = 0;
                    }
                }
                ListPath.Add(listVec3.ToArray());
            }
        }
        Debug.Log("DOne");
    }
    #endregion

    //[ContextMenu("create path")]
    //public void CreatePath()
    //{
    //    Vector3[] _twoWayPoint = null;
    //    for (int j = 0; j < 200; j++)
    //    {
    //        _twoWayPoint = new Vector3[5];
    //        var _starttransform = WayPoint[Random.Range(0, WayPoint.Length)].position.y;
    //        for (int i = 0; i < _twoWayPoint.Length; i++)
    //        {
    //            _twoWayPoint[i] = new Vector3(Random.Range(-10, 10), _starttransform, 0);
    //        }
    //        _twoWayPoint[_twoWayPoint.Length - 1] = WayPoint[Random.Range(0, WayPoint.Length)].position;
    //        ListPath.Add(_twoWayPoint);
    //        Debug.Log(j);
    //    }
    //}
    //[ContextMenu("Save")]
    //public void SavePath()
    //{
    //    //string path = Application.persistentDataPath;
    //    //path = Path.Combine(path, "Character.txt");
    //    string path = "Assets/path1.txt";
    //    List<string> data = new List<string>();
    //    foreach (var vector3se in ListPath)
    //    {
    //        string temp = String.Empty;
    //        foreach (var vector3 in vector3se)
    //        {
    //            temp += vector3.x + "\t" + vector3.y + "\t";
    //        }
    //        temp = temp.Trim();
    //        data.Add(temp);
    //    }
    //    File.WriteAllLines(path, data.ToArray(), System.Text.Encoding.UTF8);
    //    Debug.Log("DONE");
    //}
    //[ContextMenu("Load Path")]

}
    