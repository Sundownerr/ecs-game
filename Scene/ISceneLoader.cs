using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Game
{
	public interface ISceneLoader
	{
		void LoadAdditiveAsync(SceneAssetReference scene, Action<Scene> onComplete = null);
		void LoadAdditiveAsyncAll(IEnumerable<SceneAssetReference> scene, Action onComplete = null);
		void UnloadAsync(SceneAssetReference scene, Action onComplete = null);
		void UnloadAsyncAll(IEnumerable<SceneAssetReference> scenes, Action onComplete = null);
	}
}