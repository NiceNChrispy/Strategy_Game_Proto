using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class CustomSceneView : EditorWindow
{
    protected Camera SceneCamera;
    protected Scene Scene;

    [MenuItem("Window/Custom Scene View")]
    static void Init()
    {
        CustomSceneView window = (CustomSceneView)EditorWindow.GetWindow(typeof(CustomSceneView));
        window.titleContent = new GUIContent("Custom Scene View", "Custom scene window");
        window.Show();
    }

    protected virtual void Awake()
    {
        LoadScene();
        InitSceneCamera();
    }

    protected void LoadScene()
    {
        Scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
    }

    protected void InitSceneCamera()
    {
        SceneCamera = Instantiate(Camera.main);
    }

    protected void OnDestroy()
    {
        if (Scene.IsValid())
        {
            EditorSceneManager.UnloadSceneAsync(Scene);
        }
    }

    protected void RenderView()
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary((int)position.width, (int)position.height, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear, 2, RenderTextureMemoryless.MSAA, VRTextureUsage.None, true);
        //SceneCamera.enabled = true;
        SceneCamera.targetTexture = renderTexture;
        SceneCamera.Render();
        //SceneCamera.enabled = false;
        GUI.DrawTexture(new Rect(Vector2.zero, position.size), renderTexture);
        RenderTexture.ReleaseTemporary(renderTexture);
    }

    protected virtual void OnGUI()
    {
        RenderView();
        Repaint();
    }
}