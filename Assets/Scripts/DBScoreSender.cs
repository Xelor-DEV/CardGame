using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DBScoreSender : MonoBehaviour
{
    [SerializeField] private string baseUrl = "http://localhost/";
    [SerializeField] private string phpFile = "db_create_game_1.php";

    public void SendScoreToDatabase(int timeInSeconds, int userId)
    {
        StartCoroutine(SendScoreRequest(timeInSeconds, userId));
    }

    private IEnumerator SendScoreRequest(int timeInSeconds, int userId)
    {
        string url = baseUrl + phpFile;

        // Create the score data
        RecordScoreRequest requestData = new RecordScoreRequest
        {
            userid = userId,
            time_in_seconds = timeInSeconds,
            created_by = "admin"
        };

        // Convert to JSON
        string jsonString = JsonUtility.ToJson(requestData);
        Debug.Log("JSON Payload: " + jsonString);

        // Send request
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Request Error: " + request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            ServerResponse response = JsonUtility.FromJson<ServerResponse>(responseText);
            Debug.Log("Server Message: " + response.message);
        }
    }
}

[System.Serializable]
public class RecordScoreRequest
{
    public int userid;
    public int time_in_seconds;
    public string created_by;
}

[System.Serializable]
public class ServerResponse
{
    public string message;
}