using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

namespace Reboot
{
    [ExecuteInEditMode]
    public class LevelEditor : MonoBehaviour
    {
        [SerializeField] Scene m_ActiveScene;
        [SerializeField] string m_ScenePath = "Assets/Game/Scenes/Debug.unity";

        [NaughtyAttributes.Button("CREATE SCENE")]
        public void CreateNewScene()
        {
            StartCoroutine(CreateNewSceneRoutine());
        }

        private IEnumerator CreateNewSceneRoutine()
        {
            if (m_ActiveScene.IsValid())
            {
                Debug.Log("Unloading: " + m_ActiveScene.name);
                yield return SceneManager.UnloadSceneAsync(m_ActiveScene);
            }
            m_ActiveScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            GameObject levelObject = new GameObject("Level");
            levelObject.AddComponent<Level>();
            EditorSceneManager.SaveScene(m_ActiveScene);
            yield return null;
        }

        [NaughtyAttributes.Button("LOAD SCENE")]
        public void LoadLevelScene()
        {
            StartCoroutine(LoadSceneRoutine());
        }

        private IEnumerator LoadSceneRoutine()
        {
            if (m_ActiveScene.IsValid())
            {
                Debug.Log("Unloading: " + m_ActiveScene.name);
                yield return SceneManager.UnloadSceneAsync(m_ActiveScene);
            }
            if (Application.CanStreamedLevelBeLoaded(m_ScenePath))
            {
                m_ActiveScene = EditorSceneManager.OpenScene(m_ScenePath, OpenSceneMode.Additive);
                GameObject[] sceneObjects = m_ActiveScene.GetRootGameObjects();
                Level loadedLevel = null;

                foreach (GameObject go in sceneObjects)
                {
                    Level level = go.GetComponent<Level>();
                    if (level != null)
                    {
                        loadedLevel = level;
                        break;
                    }
                }

                if (loadedLevel != null)
                {
                    Debug.Log(string.Format("LOADED: {0}", loadedLevel.name));
                }
            }
        }
    }
}