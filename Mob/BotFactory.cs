using System.Collections.Generic;
using Game.ECS;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class BotFactory
    {
        // private readonly Dictionary<int, List<BotPrefab>> _aliveBots = new();
        private readonly Dictionary<int, Stack<BotPrefab>> _deadBots = new();
        private readonly Dictionary<int, EntitySO> _entities;
        private readonly Dictionary<int, GameObject> _prefabs;

        private int _botCount;
        private Transform _botParent;
        // private readonly Dictionary<int, int> _totalAlive = new();

        public BotFactory(Dictionary<int, GameObject> prefabs,
                          Dictionary<int, EntitySO> entities)
        {
            _prefabs = prefabs;
            _entities = entities;
        }

        private Transform NextParent()
        {
            if (_botCount % 256 == 0)
            {
                _botParent = new GameObject($"bots {(_botCount / 256).ToString()}").transform;
            }

            return _botParent;
        }

        private BotPrefab CreateBot(int id)
        {
            var parent = NextParent();
            var mob = Object.Instantiate(_prefabs[id], parent).GetComponent<BotPrefab>();
            mob.ID = id;

            _botCount++;

            return mob;
        }

        public void Push(BotPrefab bot)
        {
            bot.gameObject.SetActive(false);

            // var aliveBot = _aliveBots[bot.ID];
            // var lastAliveIndex = aliveBot.Count - 1;
            // var lastBot = aliveBot[lastAliveIndex];
            //
            // aliveBot[lastAliveIndex] = aliveBot[bot.Index];
            // aliveBot[bot.Index] = lastBot;
            // lastBot.Index = bot.Index;
            //
            // aliveBot.RemoveAt(lastAliveIndex);
            _deadBots[bot.ID].Push(bot);
            // _totalAlive[bot.ID]--;
        }

        public void CreateBotLists(IEnumerable<int> botIds)
        {
            foreach (var id in botIds)
            {
                // _aliveBots.Add(id, new List<BotPrefab>());
                _deadBots.Add(id, new Stack<BotPrefab>());
                // _totalAlive.Add(id, 0);
            }
        }

        public BotPrefab Pull(int id, Transform spawnPoint)
        {
            if (!_deadBots[id].TryPop(out var bot))
            {
                bot = CreateBot(id);
            }

            var transform = bot.transform;
            var offset = Random.insideUnitSphere * 2f;
            offset.y = 0f;

            transform.position = spawnPoint.position + offset;
            transform.rotation = spawnPoint.rotation;

            var gameObject = bot.gameObject;
            gameObject.SetActive(true);

            var entity = CreateBotEntity(gameObject, id);

            bot.EntityReference.Entity = entity;
            // _aliveBots[id].Add(bot);
            // bot.Index = _totalAlive[id];
            // _totalAlive[id]++;

            return bot;
        }

        private Entity CreateBotEntity(GameObject gameObject, int id)
        {
            var entity = _entities[id].Create(World.Default, gameObject);

            if (entity.Has<Target>())
            {
                ref var target = ref entity.GetComponent<Target>();
                target.Value = World.Default.Filter.With<PlayerMarker>().With<LastNavMeshPosition>()
                    .With<TransformRef>()
                    .With<WorldPosition>().First();
            }

            return entity;
        }
    }
}