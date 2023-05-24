using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class BotSpawner
    {
        private readonly BotConfig[] _botConfigs;
        private readonly BotFactory _botFactory;
        private readonly CoroutineRunner _coroutineRunner;
        private readonly EnemySpawnPoints _spawnPoints;
        private readonly Dictionary<int, int> _totalSpawned = new();

        public BotSpawner(BotFactory botFactory,
                          BotConfig[] botConfigs,
                          EnemySpawnPoints spawnPoints,
                          CoroutineRunner coroutineRunner)
        {
            _botFactory = botFactory;
            _botConfigs = botConfigs;
            _spawnPoints = spawnPoints;
            _coroutineRunner = coroutineRunner;
        }

        public void StartSpawn()
        {
            _botFactory.CreateBotLists(_botConfigs.Select(x => x.MobID.Value));

            foreach (var config in _botConfigs)
            {
                _totalSpawned.Add(config.MobID.Value, 0);
                _coroutineRunner.StartCoroutine(SpawnRoutine(config));
            }
        }

        public void Push(BotPrefab botPrefab)
        {
            _totalSpawned[botPrefab.ID]--;
            _botFactory.Push(botPrefab);
        }

        private IEnumerator SpawnRoutine(BotConfig config)
        {
            var wait = new WaitForSeconds(config.SpawnInterval);

            while (true)
            {
                yield return wait;

                var id = config.MobID.Value;

                if (_totalSpawned[id] >= config.MaxAmount)
                {
                    continue;
                }

                for (var i = 0; i < config.SpawnBatch; i++)
                {
                    var spawnPoint = _spawnPoints.All[Random.Range(0, _spawnPoints.All.Length)];
                    var bot = _botFactory.Pull(id, spawnPoint);
                    _totalSpawned[id]++;
                }
            }
        }
    }
}