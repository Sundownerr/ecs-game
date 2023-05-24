using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Scenes/Scene List", fileName = "SceneListSO")]
    public class SceneListSO : ScriptableObject, ILevels
    {
        [SerializeField] private SceneAssetReference[] _levels;
        [SerializeField] private SceneAssetReference[] _gameplayScenes;
        [SerializeField] private SceneAssetReference[] _startScenes;
        [SerializeField] private SceneAssetReference[] _playerDeathScenes;

        public IReadOnlyList<SceneAssetReference> Levels => _levels;
        public IEnumerable<SceneAssetReference> GameplayScenes => _gameplayScenes;
        public IEnumerable<SceneAssetReference> StartScenes => _startScenes;

        public IEnumerable<SceneAssetReference> PlayerDeathScenes => _playerDeathScenes;

        public SceneAssetReference At(int level)
        {
            return _levels[level];
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateSceneNames(_levels);
            UpdateSceneNames(_gameplayScenes);
            UpdateSceneNames(_startScenes);
            UpdateSceneNames(_playerDeathScenes);

            EditorUtility.SetDirty(this);
        }

        private void UpdateSceneNames(IEnumerable<SceneAssetReference> sceneAssetReferences)
        {
            foreach (var sceneAssetReference in sceneAssetReferences)
            {
                if (sceneAssetReference.Reference.editorAsset == null)
                {
                    continue;
                }

                sceneAssetReference.Name = sceneAssetReference.Reference.editorAsset.name;
            }
        }

#endif
    }
}