using System.Net.Mime;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer.Unity;

namespace Game
{
    public class DebugUIPresenter : IStartable, ITickable
    {
        private readonly BotFactory _botFactory;
        private readonly DebugUI _view;

        public DebugUIPresenter(DebugUI view, BotFactory botFactory)
        {
            _view = view;
            _botFactory = botFactory;
        }

        public void Start()
        {
            _view.MaxMobsSlider.OnValueChanged.AddListener(OnMaxMobsValueChanged);
            _view.MobSpawnBatchSlider.OnValueChanged.AddListener(OnMobSpawnBatchValueChanged);
            _view.MobSpawnIntervalSlider.OnValueChanged.AddListener(OnMobSpawnIntervalValueChanged);
        }

        public void Tick()
        {
            // _view.MaxMobsSlider.SetValue(_mobFactory._maxMobs);
            // _view.MobSpawnBatchSlider.SetValue(_mobFactory._spawnBatch);
            // _view.MobSpawnIntervalSlider.SetValue(_mobFactory._spawnInterval);

            _view.MobsCount.text = World.Default.Filter.With<BotMarker>().With<NavMeshAgentRef>().GetLengthSlow().ToString();

          
        }

        public void OnMobSpawnIntervalValueChanged(float arg0)
        {
            // _mobFactory._spawnInterval = arg0;
        }

        public void OnMobSpawnBatchValueChanged(float arg0)
        {
            // _mobFactory._spawnBatch = (int) arg0;
        }

        public void OnMaxMobsValueChanged(float arg0)
        {
            // _mobFactory._maxMobs = (int) arg0;
        }
    }
}