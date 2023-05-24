using System.Collections.Generic;
using System.Linq;
using Game.ECS;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    public class LevelScope : LifetimeScope
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private PlayerUI _playerUI;
        [SerializeField] private ExperienceOrbFactory _experienceOrbFactory;
        [SerializeField] private ExperienceOrbConfig _experienceOrbConfig;
        [SerializeField] private VFX _vfx;
        [SerializeField] private GameIDs _gameIDs;
        [SerializeField] private EnemySpawnPoints _enemySpawnPoints;
        [SerializeField] Transform _levelRoot;
        [ListDrawerSettings(DraggableItems = false, ShowIndexLabels = false)] [SerializeField]
        private List<EntitySO> _entities;
        [ListDrawerSettings(DraggableItems = false, ShowIndexLabels = false)] [SerializeField]
        private List<Prefabs> _prefabs;
        [ListDrawerSettings(DraggableItems = false, ShowIndexLabels = false)] [SerializeField] [InlineEditor]
        private BotConfig[] _mobConfigs;

#if UNITY_EDITOR
        [Button]
        private void FillReferences()
        {
            _entities = AssetDatabase.FindAssets($"t:{nameof(EntitySO)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<EntitySO>)
                .ToList();

            _prefabs = AssetDatabase.FindAssets($"t:{nameof(Prefabs)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Prefabs>)
                .ToList();
        }
#endif

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LevelEntryPoint>();

            Instances(builder);
            UI(builder);

            builder.RegisterInstance(new FPSInput());
            builder.Register<PlayerFactory>(Lifetime.Singleton);
            builder.Register<BotFactory>(Lifetime.Singleton);
            builder.Register<BotSpawner>(Lifetime.Singleton);
            builder.Register<WeaponFactory>(Lifetime.Singleton);
            builder.Register<MorpehECS>(Lifetime.Singleton);

            States(builder);
        }

        private void Instances(IContainerBuilder builder)
        {
            var entityMap = _entities.ToDictionary(x => x.ID.Value, x => x);
            var prefabsMap = _prefabs.SelectMany(prefabs => prefabs.All)
                .ToDictionary(entry => entry.ID.Value, entry => entry.Prefab);

            builder.RegisterInstance(entityMap);
            builder.RegisterInstance(prefabsMap);
            builder.RegisterInstance(_enemySpawnPoints);
            builder.RegisterInstance(_coroutineRunner);
            builder.RegisterInstance(_mobConfigs);
            builder.RegisterInstance(_experienceOrbFactory);
            builder.RegisterInstance(_experienceOrbConfig);
            builder.RegisterInstance(_vfx);
            builder.RegisterInstance(_prefabs);
            builder.RegisterInstance(_gameIDs);
            builder.RegisterInstance(_levelRoot);

            // builder.RegisterInstance(_entities).As<IEnumerable<EntitySO>>();
        }

        private void UI(IContainerBuilder builder)
        {
            builder.RegisterInstance(_playerUI);
        }

        private static void States(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<StateMachine>().AsSelf();
            builder.Register<CreatingECSWorld>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CreatePlayer>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<Gameplay>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerDead>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        protected override void OnDestroy()
        {
            World.Default?.Dispose();
            base.OnDestroy();
        }
    }
}