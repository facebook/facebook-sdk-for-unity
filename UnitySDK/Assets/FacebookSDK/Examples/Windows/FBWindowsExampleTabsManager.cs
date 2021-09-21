using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBWindowsExampleTabsManager : MonoBehaviour
{
    public GameObject[] sections;

    void Start()
    {
        ShowTab(0);
    }

    public void ShowTab(int id)
    {
        foreach (GameObject section in sections)
        {
            section.SetActive(false);
        }
        sections[id].SetActive(true);
    }
}
