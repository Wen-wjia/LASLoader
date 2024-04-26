using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class FileSelectManager : MonoBehaviour
{
    GameObject importBtn;
    

    public string filePath;
    public GameObject btn;
    public GameObject listView;
    public Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        importBtn = GameObject.Find("SelectList");
    }

    public void GenerateList()
    {
        importBtn.SetActive(false);

        DirectoryInfo dir = new DirectoryInfo(@filePath);
        FileInfo[] allFiles = dir.GetFiles("*.las");

        for (int i = 0; i < allFiles.Length; i++)
        {
            GameObject newBtn = Instantiate(btn);
            newBtn.transform.SetParent(parent);

            newBtn.GetComponentInChildren<TMP_Text>().text = allFiles[i].Name.ToString();

            newBtn.transform.position = new Vector3(btn.transform.position.x, btn.transform.position.y - 50 * i, btn.transform.position.z);
        }

        btn.SetActive(false);
    }

    public void HideList()
    {
        importBtn.SetActive(true);
        listView.SetActive(false);
    }

    public string getFilePath()
    {
        return filePath;
    }
}
