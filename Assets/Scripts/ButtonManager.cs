using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public LASImporter LAS;

    public GameObject listView;

    string path;
    string fileName;

    FileSelectManager fsm;

    // Start is called before the first frame update
    void Start()
    {
        fsm = FindObjectOfType<FileSelectManager>();
    }
    
    public void ShowList()
    {
        listView.SetActive(true);
        fsm.GenerateList();
    }

    public void LoadFile(TMP_Text text)
    {
        fileName = text.text;
        path = fsm.getFilePath() + "" + fileName;

        LAS.ImportLAS(path);

        fsm.HideList();
    }
}
