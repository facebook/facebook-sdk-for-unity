using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour {
  private bool showSettings = false;
  public void ToggleActive () {
    showSettings = !showSettings;
    gameObject.SetActive(showSettings);
  }
}
