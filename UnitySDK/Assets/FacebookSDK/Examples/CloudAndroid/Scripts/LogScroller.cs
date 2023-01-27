using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogScroller : MonoBehaviour {
  [SerializeField]
  private GameObject _content;

  [HideInInspector]
  public List<String> texts = new List<String>();

  private string DATE_FORMAT = @"M/d/yyyy hh:mm:ss tt";
  private int index = 0;

  void Start() {
  }

  public void ClearLogs () {
    texts.Clear();
    foreach (Transform child in _content.transform) {
      Destroy(child.gameObject);
    }
  }
  public void Log (string text) {
    GameObject nText = new GameObject("text-" + index++);
    nText.transform.parent = _content.transform;
    nText.transform.localPosition = new Vector3(0, 0, 0);
    nText.transform.localScale = new Vector3(1, 1, 1);
    nText.transform.SetAsFirstSibling();

    string formattedText = string.Format("{0}\n{1}\n", DateTime.Now.ToString(DATE_FORMAT), text);
    texts.Insert(0, formattedText);
    Text textComp = nText.AddComponent<Text>();
    textComp.text = formattedText;
    textComp.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
    textComp.color = new Color(1, 1, 1, 1);
  }
}
