using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Scenes/Scene", fileName = "SceneSO")]
    public class SceneSO : ScriptableObject, IScene
    {
        [SerializeField] private AssetReference _sceneReference;
        public string Name => _sceneReference.Asset.name;

        public AssetReference SceneReference => _sceneReference;
    }
}