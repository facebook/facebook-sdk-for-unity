using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FBWindowsGraphAPIManager : MonoBehaviour {

	public FBWindowsLogsManager Logger;
	public InputField QueryText;
	public Dropdown QueryType; 

	private IDictionary<string, string> formData = null;

	public void GraphAPI()
	{

		HttpMethod typeQuery = (HttpMethod)Enum.Parse(typeof(HttpMethod), QueryType.options[QueryType.value].text);

		FB.API(QueryText.text, typeQuery, (result) =>
		{
			Logger.DebugLog(result.RawResult);
		}, formData);
	}
}
