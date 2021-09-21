using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FBWindowsGraphAPIManager : MonoBehaviour
{
    public FBWindowsLogsManager Logger;
    public InputField QueryText;
    public Dropdown QueryType;
    public InputField GraphAPIVersionText;
    public Text GraphAPIVersion;
    private IDictionary<string, string> formData = null;

    void Start()
    {
        GraphAPIVersionText.text = Constants.GraphApiVersion;
    }

    void OnEnable()
    {
        GraphAPIVersion.text = " Current Graph API version: " + FB.GraphApiVersion + "\n SDK Graph API version: " + Constants.GraphApiVersion;
    }

    public void GraphAPI()
    {
        HttpMethod typeQuery = (HttpMethod)Enum.Parse(typeof(HttpMethod), QueryType.options[QueryType.value].text);
        FB.API(QueryText.text, typeQuery, (result) =>
        {
            Logger.DebugLog(result.RawResult);
        }, formData);
    }

    public void SetGraphAPiVersion()
    {
        if (GraphAPIVersionText.text != "")
        {
            FB.GraphApiVersion = GraphAPIVersionText.text;
        }
        OnEnable();
    }
}
