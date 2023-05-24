using System;
using UnityEngine.AddressableAssets;

namespace Game
{
    [Serializable]
    public class SceneAssetReference
    {
        public AssetReference Reference;
        public string Name;
    }
}