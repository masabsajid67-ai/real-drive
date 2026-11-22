using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace RealDrive.Core
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        public static string TargetScene { get; private set; } = GameConstants.SceneOpenWorld;
        public static float LoadProgress { get; private set; }
        public static bool IsLoading { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        public void LoadWithLoadingScreen(string targetScene)
        {
            if (IsLoading) return;
            TargetScene = targetScene;
            LoadProgress = 0f;
            StartCoroutine(LoadLoadingSceneThenTarget());
        }
        IEnumerator LoadLoadingSceneThenTarget()
        {
            IsLoading = true;
            AsyncOperation op = SceneManager.LoadSceneAsync(GameConstants.SceneLoading, LoadSceneMode.Single);
            while (!op.isDone) yield return null;
            yield return null;
            // LoadingScreenManager in LoadingScene will call LoadTargetAsync()
        }
        public IEnumerator LoadTargetAsync()
        {
            IsLoading = true;
            LoadProgress = 0f;
            AsyncOperation op = SceneManager.LoadSceneAsync(TargetScene, LoadSceneMode.Single);
            op.allowSceneActivation = false;
            while (op.progress < 0.9f)
            {
                LoadProgress = Mathf.Clamp01(op.progress / 0.9f);
                yield return null;
            }
            LoadProgress = 1f;
            yield return new WaitForSeconds(0.4f);
            op.allowSceneActivation = true;
            while (!op.isDone) yield return null;
            IsLoading = false;
        }
        public void LoadDirect(string sceneName)
        {
            if (IsLoading) return;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
