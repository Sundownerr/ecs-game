using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class MorpehECS
    {
        private readonly BotSpawner _botSpawner;
        private readonly FPSInput _input;
        private readonly PlayerUI _playerUI;
        private readonly VFX _vfx;
        private World _ecsWorld;

        public MorpehECS(PlayerUI playerUI, VFX vfx, FPSInput input, BotSpawner botSpawner)
        {
            _playerUI = playerUI;
            _vfx = vfx;
            _input = input;
            _botSpawner = botSpawner;
        }

        public void Install()
        {
            _ecsWorld = World.Default ?? World.Create();
            _ecsWorld.UpdateByUnity = false;
            
            var create = _ecsWorld.CreateSystemsGroup();
            create.AddSystem(new CreateWeapon());
            
            var construct = _ecsWorld.CreateSystemsGroup();
            construct.AddSystem(new ConstructHealth());
            construct.AddSystem(new ConstructWeaponRefHitEntities());
            construct.AddSystem(new ConstructWeaponConfig());
            construct.AddSystem(new ConstructWeaponHitDamage());
            construct.AddSystem(new ConstructWeaponHitParticleData());
            construct.AddSystem(new ConstructAoEWeaponHit());
            construct.AddSystem(new ConstructDamageEventTarget());
            construct.AddSystem(new ConstructAoeDamageDirection());
            // construct.AddSystem(new ProcessAoeDamageWeaponHit());
            construct.AddSystem(new AddBotWeaponToWeaponList());
            construct.AddSystem(new ConstructBotMinimumAttackRange());

            var player = _ecsWorld.CreateSystemsGroup();
            player.AddSystem(new UpdatePlayerInput(_input));
            player.AddSystem(new UpdatePlayerMoveDirection());
            player.AddSystem(new UpdatePlayerMoveSpeed());
            player.AddSystem(new PlayerSprinting());
            player.AddSystem(new ApplyMoveSpeed());
            player.AddSystem(new Jumping());
            player.AddSystem(new UpdateIsGrounded());
            player.AddSystem(new ApplyGravity());
            player.AddSystem(new MovePlayer());
            player.AddSystem(new UpdatePlayerLookDirection());
            player.AddSystem(new UpdatePlayerPosition());
            player.AddSystem(new Flying());
            player.AddSystem(new FlyingGravity());
            player.AddSystem(new Dash());
            player.AddSystem(new SlowMotion());

            var systemsGroup = _ecsWorld.CreateSystemsGroup();
            systemsGroup.AddSystem(new UpdateHealthRegen());
            systemsGroup.AddSystem(new RegenrateHealth());
            systemsGroup.AddSystem(new ClampHealth());
            systemsGroup.AddSystem(new UpdateCooldown());
            
            systemsGroup.AddSystem(new PrimaryPlayerWeaponControls());
            systemsGroup.AddSystem(new SecondaryPlayerWeaponControls());
            
            systemsGroup.AddSystem(new UpdateBotPosition());
            systemsGroup.AddSystem(new UpdateTargetWorldPosition());
            systemsGroup.AddSystem(new UpdateBotDistanceToPlayer());
            systemsGroup.AddSystem(new UpdateBotFramesPerUpdate());
            systemsGroup.AddSystem(new MoveBot());
            systemsGroup.AddSystem(new MeleeBotAttack());
            systemsGroup.AddSystem(new UpdateBotJumpAttack());
            
            systemsGroup.AddSystem(new SetTargetMarkerToDamageEvent());
            systemsGroup.AddSystem(new DecreaseHealthOnDamageEvent());
            systemsGroup.AddSystem(new BotDamaged());
            // systemsGroup.AddSystem(new BotGroupDamaged());
            systemsGroup.AddSystem(new SetDamageDirectionOnHit());
            systemsGroup.AddSystem(new ProcessPlayerDamaged());
            systemsGroup.AddSystem(new DestroyBot(_botSpawner));
            systemsGroup.AddSystem(new ExperienceGain());
            systemsGroup.AddSystem(new PlayerExperienceGain());
            systemsGroup.AddSystem(new WeaponHitVfx(_vfx.HitVfx));
            systemsGroup.AddSystem(new CreateBotDeathVfx(_vfx.MobDeathVfx));
            systemsGroup.AddSystem(new PlayerUIExperience(_playerUI));
            systemsGroup.AddSystem(new PlayerUIHealth(_playerUI));
            systemsGroup.AddSystem(new UpdateFramesPerUpdate());
            systemsGroup.AddSystem(new RemoveOneFrameEntities());

            _ecsWorld.AddSystemsGroup(0, create);
            _ecsWorld.AddSystemsGroup(1, construct);
            _ecsWorld.AddSystemsGroup(2, player);
            _ecsWorld.AddSystemsGroup(3, systemsGroup);
        }
        
        public void Dispose()
        {
            _ecsWorld?.Dispose();
        }

        public void Update()
        {
            _ecsWorld.Update(Time.deltaTime);
        }

        public void FixedUpdate()
        {
            _ecsWorld.FixedUpdate(Time.fixedDeltaTime);
        }

        public void LateUpdate()
        {
            var dt = Time.deltaTime;
            _ecsWorld.LateUpdate(dt);
            _ecsWorld.CleanupUpdate(dt);
        }
    }
}