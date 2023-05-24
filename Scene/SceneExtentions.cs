using TMPro;
using UnityEngine.SceneManagement;

namespace Game
{
	public static class SceneExtentions
	{
		public static Scene Instance(this IScene scene)
		{
			return SceneManager.GetSceneByName(scene.SceneReference.SubObjectName);
		}

		public static bool IsLoaded(this IScene scene)
		{
			return Instance(scene).isLoaded;
		}

		public static void SetAsMainScene(this IScene scene)
		{
			if (scene.IsLoaded())
			{
				SceneManager.SetActiveScene(scene.Instance());
			}
		}
	}
}