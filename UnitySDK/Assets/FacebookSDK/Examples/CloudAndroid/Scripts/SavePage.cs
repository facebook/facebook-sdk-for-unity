/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SavePage : MonoBehaviour
{
    [SerializeField]
    private Button _saveLogBtn;
    [SerializeField]
    private InputField _customInputField;
    [SerializeField]
    private Button _appendDataBtn;

    private string LOGS_FOLDER_PATH;
    private string PLAYER_PREFS_PATH;
    private string SAVE_FILE_PATH;
    private LogScroller _logScroller;

    private void Awake()
    {
        LOGS_FOLDER_PATH = string.Format("{0}/Logs", Application.persistentDataPath);
        PLAYER_PREFS_PATH = string.Format("/data/data/{0}/shared_prefs/{0}.v2.playerprefs.xml", Application.identifier);

        // Path that you would input under save file paths when uploading binary to Facebook Developer App
        SAVE_FILE_PATH = Application.persistentDataPath + "/savefile.txt";

        _logScroller = transform.root.GetComponent<UIState>().logScroller;
    }

    // player prefs
    public void OnLogPlayerPrefsBtnClick()
    {
        int mutedState = PlayerPrefs.GetInt("muted");
        // Debug.Log(mutedState);
        _logScroller.Log("muted: " + mutedState);
    }
    public void OnSavePlayerPrefsBtnClick()
    {
        _logScroller.Log("PlayerPrefs Saved\n" + PLAYER_PREFS_PATH);
        PlayerPrefs.Save();
    }

    // log files
    public void OnDeleteLogFilesBtnClick()
    {
        Directory.Delete(LOGS_FOLDER_PATH, true);
        _logScroller.Log("All Logs Deleted");
    }
    public void OnSaveLogsBtnClick()
    {
        _logScroller.Log("Saving Logs to Disk");

        if (!Directory.Exists(LOGS_FOLDER_PATH))
        {
            Directory.CreateDirectory(LOGS_FOLDER_PATH);
        }

        string filePath = string.Format("{0}/{1}.txt", LOGS_FOLDER_PATH, DateTimeOffset.Now.ToUnixTimeSeconds());

        try
        {
            File.AppendAllLines(filePath, _logScroller.texts);
            _logScroller.Log("Logs saved to " + filePath);
        }
        catch (Exception err)
        {
            _logScroller.Log("ERR: Failed to save logs\n" + err.Message);
        }
    }

    // save file
    public void OnAppendSaveFileBtnClick()
    {
        _logScroller.Log("Appending to Save File");
        try
        {
            File.AppendAllText(SAVE_FILE_PATH, _customInputField.text + "\n");
            _customInputField.text = "";
            _logScroller.Log("Appended to\n" + SAVE_FILE_PATH);
        }
        catch (Exception err)
        {
            _logScroller.Log("ERR: Failed to append to save file\n" + err.Message);
        }
    }
    public void OnDeleteSaveFileBtnClick()
    {
        File.Delete(SAVE_FILE_PATH);
        _logScroller.Log("Save File Deleted");
    }
    public void OnLogSaveFileBtnClick()
    {
        _logScroller.Log("Loading Save File");
        try
        {
            string[] lines = File.ReadAllLines(SAVE_FILE_PATH);
            _logScroller.Log(string.Format("Save file has {0} lines:\n\n{1}", lines.Length, String.Join("\n", lines)));
        }
        catch (Exception err)
        {
            _logScroller.Log("ERR: Failed to load from save file\n" + err.Message);
        }
    }
    public void OnLogSaveFilePathBtnClick()
    {
        _logScroller.Log("Save file path\n" + SAVE_FILE_PATH);
    }
}
