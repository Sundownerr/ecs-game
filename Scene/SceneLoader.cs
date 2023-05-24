using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Game
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Dictionary<SceneAssetReference, SceneInstance> _loadedScenes = new();

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadAdditiveAsync(SceneAssetReference scene, Action<Scene> onCompleted = null)
        {
            if (SceneLoaded(scene))
            {
                onCompleted?.Invoke(LoadedSceneInstance(scene));
                return;
            }

            _coroutineRunner.StartCoroutine(LoadAdditiveAsyncRoutine(scene, onCompleted));
        }

        public void LoadAdditiveAsyncAll(IEnumerable<SceneAssetReference> scenes, Action onCompleted = null)
        {
            _coroutineRunner.StartCoroutine(LoadAdditiveAsyncAllRoutine(scenes, onCompleted));
        }

        public void UnloadAsync(SceneAssetReference scene, Action onCompleted = null)
        {
            _coroutineRunner.StartCoroutine(UnloadAsyncRoutine(scene, onCompleted));
        }

        public void UnloadAsyncAll(IEnumerable<SceneAssetReference> scenes, Action onCompleted = null)
        {
            _coroutineRunner.StartCoroutine(UnloadAsyncAllRoutine(scenes, onCompleted));
        }

        private Scene LoadedSceneInstance(SceneAssetReference sceneAssetReference)
        {
            if (_loadedScenes.TryGetValue(sceneAssetReference, out var sceneInstance))
            {
                return sceneInstance.Scene;
            }

            return SceneManager.GetSceneByName(sceneAssetReference.Name);
        }

        private bool SceneLoaded(SceneAssetReference assetReference)
        {
            return SceneManager.GetSceneByName(assetReference.Name).isLoaded ||
                   _loadedScenes.ContainsKey(assetReference);
        }

        private IEnumerator LoadAdditiveAsyncAllRoutine(IEnumerable<SceneAssetReference> scenes,
                                                        Action onCompleted = null)
        {
            foreach (var scene in scenes)
            {
                yield return LoadAdditiveAsyncRoutine(scene);
            }

            onCompleted?.Invoke();
        }

        private IEnumerator LoadAdditiveAsyncRoutine(SceneAssetReference scene, Action<Scene> onCompleted = null)
        {
            if (SceneLoaded(scene))
            {
                onCompleted?.Invoke(LoadedSceneInstance(scene));
                yield break;
            }

            var op = Addressables.LoadSceneAsync(scene.Reference, LoadSceneMode.Additive);

            while (!op.IsDone)
            {
                yield return null;
            }

            _loadedScenes.Add(scene, op.Result);

            onCompleted?.Invoke(op.Result.Scene);
        }

        private IEnumerator UnloadAsyncRoutine(SceneAssetReference scene, Action onCompleted = null)
        {
            if (!SceneLoaded(scene))
            {
                onCompleted?.Invoke();
                yield break;
            }

            if (_loadedScenes.TryGetValue(scene, out var sceneInstance))
            {
                var operation = Addressables.UnloadSceneAsync(sceneInstance);
                // Addressables.UnloadSceneAsync(_loadedScenes[scene]);

                while (!operation.IsDone)
                {
                    yield return null;
                }

                _loadedScenes.Remove(scene);

                onCompleted?.Invoke();
                
                yield break;
            }
            
            var op = SceneManager.UnloadSceneAsync(scene.Name);
            
            while (!op.isDone)
            {
                yield return null;
            }
            
            onCompleted?.Invoke();
        }

        private IEnumerator UnloadAsyncAllRoutine(IEnumerable<SceneAssetReference> scenes, Action onCompleted = null)
        {
            foreach (var scene in scenes)
            {
                yield return UnloadAsyncRoutine(scene);
            }

            onCompleted?.Invoke();
        }
    }
}