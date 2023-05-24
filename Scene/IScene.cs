using UnityEngine.AddressableAssets;

namespace Game
{
	public interface IScene
	{
		string Name { get; }
		AssetReference SceneReference { get; }
	}
}