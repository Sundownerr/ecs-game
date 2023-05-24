using System;
using UnityEngine.SceneManagement;

namespace Game
{
	public class LevelLoader : ILevelLoader
	{
		private readonly ILevels _levels;
		private readonly ISceneLoader _sceneLoader;

		public LevelLoader(ILevels levels, ISceneLoader sceneLoader)
		{
			_levels = levels;
			_sceneLoader = sceneLoader;
		}

		public void Load(int levelIndex, Action<Scene> onComplete = null)
		{
			var level = _levels.At(levelIndex);

			_sceneLoader.LoadAdditiveAsync(level, levelScene =>
			{
				SceneManager.SetActiveScene(levelScene);
				onComplete?.Invoke(levelScene);
			});
		}

		public void Unload(int levelIndex, Action onComplete = null)
		{
			_sceneLoader.UnloadAsync(_levels.At(levelIndex), onComplete);
		}
	}
}