#if UNITY_EDITOR
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

[ExecuteInEditMode]
public class MeshyImageTo3DGenerator : MonoBehaviour
{
    [Header("Meshy API Settings")]
    [Tooltip("Your Pro account API key from Meshy")]
    public string apiKey;
    [Tooltip("Public URL to the image you want to convert")]
    public string imageUrl;

    [Header("Save Settings")]
    [Tooltip("Relative path in Assets to save the generated model")]
    public string saveFolder = "Assets/MeshyModels";

    private string taskId;

    [ContextMenu("🕹️ Generate 3D Model")]
    public void Generate3DModel()
    {
        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(imageUrl))
        {
            Debug.LogError("API Key ו-Image URL חייבים להיות מלאים!");
            return;
        }

        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        // התחל את הקריאה וה-polling בעזרת Coroutine
        EditorCoroutineUtility.StartCoroutineOwnerless(GenerateCoroutine());
    }

    private IEnumerator GenerateCoroutine()
    {
        // 1) בקשת POST ליצירת משימה
        var body = JsonUtility.ToJson(new
        {
            image_url = imageUrl,
            should_remesh = true,
            should_texture = true
        });
        var postReq = new UnityWebRequest("https://api.meshy.ai/openapi/v1/image-to-3d", "POST");
        byte[] raw = System.Text.Encoding.UTF8.GetBytes(body);
        postReq.uploadHandler = new UploadHandlerRaw(raw);
        postReq.downloadHandler = new DownloadHandlerBuffer();
        postReq.SetRequestHeader("Content-Type", "application/json");
        postReq.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        yield return postReq.SendWebRequest();

        if (postReq.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"POST failed: {postReq.error}");
            yield break;
        }

        // 2) נזרים את ה-ID של המשימה
        var resJson = postReq.downloadHandler.text;
        taskId = JsonUtility.FromJson<TaskResponse>(resJson).id;
        Debug.Log($"Meshy Task Created: {taskId}");

        // 3) Polling עד מצב SUCCEEDED או FAILED
        string status = null, glbUrl = null;
        var statusReq = new UnityWebRequest(
            $"https://api.meshy.ai/openapi/v1/image-to-3d/{taskId}", "GET"
        );
        statusReq.downloadHandler = new DownloadHandlerBuffer();
        statusReq.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        while (true)
        {
            yield return statusReq.SendWebRequest();
            if (statusReq.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Status check failed: {statusReq.error}");
                yield break;
            }
            var st = JsonUtility.FromJson<StatusResponse>(statusReq.downloadHandler.text);
            status = st.status;
            Debug.Log($"Meshy status: {status}");
            if (status == "SUCCEEDED")
            {
                glbUrl = st.model_urls.glb;
                break;
            }
            else if (status == "FAILED")
            {
                Debug.LogError("Meshy rendering failed");
                yield break;
            }
            // המתן 5 שניות לפני בדיקה חוזרת
            yield return new EditorWaitForSeconds(5f);
        }

        // 4) הורדת קובץ ה-GLB
        var glbReq = UnityWebRequest.Get(glbUrl);
        yield return glbReq.SendWebRequest();
        if (glbReq.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"GLB download failed: {glbReq.error}");
            yield break;
        }

        // 5) שמירה ושילוב ב-Assets
        var bytes = glbReq.downloadHandler.data;
        var outPath = Path.Combine(Application.dataPath,
                        saveFolder.Replace("Assets/", ""),
                        $"{taskId}.glb");
        File.WriteAllBytes(outPath, bytes);
        AssetDatabase.Refresh();

        Debug.Log($"✅ Model saved to {outPath}");

        // 6) אופציונלי – אפסנטiate לסצנה
        var assetPath = $"{saveFolder}/{taskId}.glb";
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        if (prefab != null)
        {
            PrefabUtility.InstantiatePrefab(prefab);
            Debug.Log("🧩 Model instantiated in scene");
        }
    }

    // עזר להצפה של JSON
    [System.Serializable] class TaskResponse { public string id; }
    [System.Serializable] class StatusResponse { public string status; public ModelUrls model_urls; }
    [System.Serializable] class ModelUrls { public string glb; public string fbx; }
}
#endif
