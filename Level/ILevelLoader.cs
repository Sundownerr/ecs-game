using System;
using UnityEngine.SceneManagement;

namespace Game
{
	public interface ILevelLoader
	{
		void Load(int levelIndex, Action<Scene> onComplete = null);
		void Unload(int levelIndex, Action onComplete = null);
	}
}