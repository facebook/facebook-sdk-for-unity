using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Utility {
  private static Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();

  // TODO check if loading a texture already for an id and wait for that instead of redoing the work
  public static IEnumerator GetTexture(string id, string url, System.Action<Texture> callback) {
    if (_textures.ContainsKey(id)) {
      callback(_textures[id]);
      yield break;
    }

    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
    yield return www.SendWebRequest();

    if (www.responseCode != 200) {
      Debug.Log(www.error);
    }
    else {
      // Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
      Texture texture = DownloadHandlerTexture.GetContent(www);
      callback(texture);
    }
  }
}
