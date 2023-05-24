using System.Collections.Generic;
using Game.ECS;
using ProjectDawn.Navigation.Hybrid;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class CreateEntitiesOnWeaponHit : ConfigComponentWrapper
    {
        public EntitySO[] All;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var weaponHitEntities = ref entity.AddComponent<WeaponHitEntites>();
            weaponHitEntities.Value = All;
        }
    }

    public class Bot : ConfigComponentWrapper
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            entity.AddComponent<BotMarker>();
            entity.AddComponent<FramesPerUpdate>();

            ref var transformRef = ref entity.AddComponent<TransformRef>();
            transformRef.Construct(gameObject);

            ref var gameObjectRef = ref entity.AddComponent<GameObjectRef>();
            gameObjectRef.Construct(gameObject);
        }
    }

    public class EnemyWalkingBot : Bot
    {
        public float DamageDirectionModifier;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            base.AddTo(entity, gameObject);

            entity.AddComponent<WorldPosition>();
            entity.AddComponent<AttackRadius>();
            entity.AddComponent<Target>();
            entity.AddComponent<TargetWorldPosition>();
            entity.AddComponent<DistanceToTarget>();
            entity.AddComponent<Construct_OneFrame>();
            entity.AddComponent<DamageDirection>();

            ref var botDeathConfig = ref entity.AddComponent<BotDeathSettings>();
            botDeathConfig.DamageDirectionModifier = DamageDirectionModifier;

            ref var navmeshAgentRef = ref entity.AddComponent<NavMeshAgentRef>();
            navmeshAgentRef.Construct(gameObject);

            ref var botPrefabRef = ref entity.AddComponent<BotPrefabRef>();
            botPrefabRef.Construct(gameObject);
        }
    }

    public class Weapon : AbilityWithCooldown
    {
        public float Damage;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            base.AddTo(entity, gameObject);

            entity.AddComponent<Construct_OneFrame>();
            entity.AddComponent<WeaponMarker>();
            entity.AddComponent<Damage>();
            ref var confgi = ref entity.AddComponent<WeaponConfig>();

            confgi.Damage = Damage;
            confgi.Cooldown = Cooldown;
        }
    }

    public class MeleeWeapon : ConfigComponentWrapper
    {
        public float Range;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var meleeWeaponConfig = ref entity.AddComponent<MeleeWeaponSettings>();
            meleeWeaponConfig.Range = Range;

            entity.AddComponent<MeleeWeaponMarker>();
        }
    }

    public class PlayerWeapon : ConfigComponentWrapper
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var weaponRef = ref entity.AddComponent<WeaponRef>();
            weaponRef.Construct(gameObject);

            ref var transformRef = ref entity.AddComponent<TransformRef>();
            transformRef.Construct(gameObject);
        }
    }

    public class PlayerPrimaryWeapon : PlayerWeapon
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            base.AddTo(entity, gameObject);

            entity.AddComponent<PrimaryMarker>();
            entity.AddComponent<PlayerInput_PrimaryAttack_IsPressed>();
        }
    }

    public class PlayerSecondaryWeapon : PlayerWeapon
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            base.AddTo(entity, gameObject);

            entity.AddComponent<SecondaryMarker>();
            entity.AddComponent<PlayerInput_SecondaryAttack_IsPressed>();
        }
    }

    public class Ability : ConfigComponentWrapper
    {
        public bool UsableAtStart = true;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var isUsable = ref entity.AddComponent<IsUsable>();
            isUsable.Value = UsableAtStart;
        }
    }

    public class AbilityWithCooldown : Ability
    {
        public float Cooldown;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            base.AddTo(entity, gameObject);

            ref var cooldown = ref entity.AddComponent<Cooldown>();
            cooldown.Max = Cooldown;
        }
    }

    public class InstantiateWeapon : ConfigComponentWrapper
    {
        public EntitySO EntityTemplate;
        public GameObject Prefab;
        public ID SlotID;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            entity.AddComponent<OneFrameMarker>();
            entity.AddComponent<Instantiate_OneFrame>();
            ref var config = ref entity.AddComponent<InstantiateWeaponConfig>();

            config.EntityTemplate = EntityTemplate;
            config.Prefab = Prefab;
            config.SlotID = SlotID;
        }
    }

    public class BotJumpAttackConfig : ConfigComponentWrapper
    {
        public float Damage;
        public float DamageRadius;
        public Gravity Gravity;
        public float JumpForce;
        public float JumpHeight;
        // public float Time;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var animation = ref entity.AddComponent<BotJumpAttackAnimation>();
            // animation.Time = Time;
            animation.JumpHeight = JumpHeight;
            animation.JumpForce = JumpForce;

            entity.AddComponent<BotJumpAttackAbilityMarker>();

            ref var gravity = ref entity.AddComponent<Gravity>();
            gravity.Value = Gravity.Value;

            ref var damage = ref entity.AddComponent<Damage>();
            damage.Value = Damage;

            ref var damageRadius = ref entity.AddComponent<Radius>();
            damageRadius.Value = DamageRadius;
        }
    }

    public class BotJumpAttack : AbilityWithCooldown
    {
        public EntitySO Template;
        public float UseRange;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            base.AddTo(entity, gameObject);

            ref var ability = ref entity.AddComponent<BotJumpAttackAbility>();
            ability.Template = Template;
            ability.UseRange = UseRange;
        }
    }

    public class GroundedCheck : ConfigComponentWrapper
    {
        public LayerMask GroundMask;
        public float Offset;
        public float Radius;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            entity.AddComponent<IsGrounded>();
            ref var config = ref entity.AddComponent<IsGroundedSettings>();
            config.Offset = Offset;
            config.Radius = Radius;
            config.GroundMask = GroundMask;
        }
    }

    public class Sprint : ConfigComponentWrapper
    {
        public float Speed;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var config = ref entity.AddComponent<SprintConfig>();
            config.SprintSpeed = Speed;

            entity.AddComponent<SprintMarker>();
        }
    }

    public class Jump : ConfigComponentWrapper
    {
        public float Height = 1.2f;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var config = ref entity.AddComponent<JumpSettings>();
            config.Height = Height;
        }
    }

    public class Health_ : ConfigComponentWrapper
    {
        public float Max = 100;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var config = ref entity.AddComponent<HealthConfig>();
            config.Max = Max;

            ref var health = ref entity.AddComponent<Health>();

            if (!entity.Has<Construct_OneFrame>())
            {
                entity.AddComponent<Construct_OneFrame>();
            }
        }
    }

    public class HealthWithRegen : Health_
    {
        public float RegenPerSecond = 10;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            base.AddTo(entity, gameObject);
            entity.AddComponent<HealthRegen>();

            ref var config = ref entity.GetComponent<HealthConfig>();
            config.RegenPerSecond = RegenPerSecond;
        }
    }

    public class Fly : ConfigComponentWrapper
    {
        public float AscendSpeed = 40f;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            entity.AddComponent<Flying_Marker>();
            ref var config = ref entity.AddComponent<FlyingConfig>();
            config.AscendSpeed = AscendSpeed;
        }
    }

    public class PlayerJump : Jump
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            entity.AddComponent<PlayerInput_Jump_IsPressed>();
            base.AddTo(entity, gameObject);
        }
    }

    public class Player : ConfigComponentWrapper
    {
        public float LookSensivity = 1;
        public float MoveSpeed = 20;
        public float MoveSpeedChangeRate = 10;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            entity.AddComponent<WorldPosition>();
            entity.AddComponent<LastNavMeshPosition>();
            entity.AddComponent<MoveDirection>();
            entity.AddComponent<LookDirection>();
            entity.AddComponent<PlayerMarker>();
            entity.AddComponent<VerticalVelocity>();
            entity.AddComponent<MoveSpeed>();

            entity.AddComponent<PlayerInput_MoveDirection>();
            entity.AddComponent<PlayerInput_LookDirection>();
            entity.AddComponent<PlayerInput_Sprint_IsPressed>();

            ref var charController = ref entity.AddComponent<CharacterControllerRef>();
            charController.Construct(gameObject);

            ref var camera = ref entity.AddComponent<CinemachineCameraTargetRef>();
            camera.Construct(gameObject);

            ref var transform = ref entity.AddComponent<TransformRef>();
            transform.Construct(gameObject);

            ref var transformParts = ref entity.AddComponent<TransformPartsRef>();
            transformParts.Construct(gameObject);

            ref var moveSpeedConfig = ref entity.AddComponent<PlayerMoveSpeedSettings>();
            moveSpeedConfig.Normal = MoveSpeed;
            moveSpeedConfig.ChangeRate = MoveSpeedChangeRate;

            ref var lookConfig = ref entity.AddComponent<LookConfig>();
            lookConfig.RotationSpeed = LookSensivity;
        }
    }

    public class PlayerDashAbility : DashAbility
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            base.AddTo(entity, gameObject);
            ref var charController = ref entity.AddComponent<CharacterControllerRef>();
            charController.Construct(gameObject);

            ref var transform = ref entity.AddComponent<TransformRef>();
            transform.Construct(gameObject);

            entity.AddComponent<PlayerMarker>();
            entity.AddComponent<PlayerInput_Sprint_IsPressed>();
            entity.AddComponent<PlayerInput_MoveDirection>();
        }
    }

    public class DashAbility : AbilityWithCooldown
    {
        public float Force = 200f;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            base.AddTo(entity, gameObject);

            entity.AddComponent<DashAbilityMarker>();
            ref var dashSettings = ref entity.AddComponent<DashAbilitySettings>();

            dashSettings.Force = Force;
        }
    }

    public class CharacterControllerRef_ : RuntimeComponentWrapper<CharacterControllerRef>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity).Value = gameObject.GetComponent<CharacterController>();
        }
    }

    public class DistanceToTarget_ : RuntimeComponentWrapper<DistanceToTarget>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class ExperienceLevel_ : RuntimeComponentWrapper<ExperienceLevel>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class Experince_ : ComponentWrapper<Experince>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class Flying_Marker_ : MarkerComponentWrapper<Flying_Marker>
    { }

    public class FlyingConfig_ : ConfigComponentWrapper<FlyingConfig>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class FramesPerUpdate_ : RuntimeComponentWrapper<FramesPerUpdate>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class ChargePoint : ConfigComponentWrapper
    {
        public float ChargePerSecond = 10;
        public float MaxCharge = 100;
        public float Radius = 10;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var radius = ref entity.AddComponent<Radius>();
            radius.Value = Radius;

            ref var chargeConfig = ref entity.AddComponent<ChargeConfig>();
            chargeConfig.ChargePerSecond = ChargePerSecond;
            chargeConfig.MaxCharge = MaxCharge;
        }
    }

    public class SlowMotionAbility : Ability
    {
        public float TimeScale;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            base.AddTo(entity, gameObject);

            ref var config = ref entity.AddComponent<SlowMotionConfig>();
            config.TimeScale = TimeScale;

            entity.AddComponent<PlayerInput_SlowMo_WasPressed>();
            entity.AddComponent<SlowMotionToggle>();
        }
    }

    public class MeleeWeapons_ : RuntimeComponentWrapper<MeleeWeapons>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var c = ref AddComponent(entity);
            c.Value = new List<Entity>();
        }
    }

    public class RangedWeapons_ : RuntimeComponentWrapper<RangedWeapons>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var c = ref AddComponent(entity);
            c.Value = new List<Entity>();
        }
    }

    public class GameObjectRef_ : RuntimeComponentWrapper<GameObjectRef>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity).Value = gameObject;
        }
    }

    public class GivenExperience_ : ConfigComponentWrapper<GivenExperience>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class GlobalMarker_ : MarkerComponentWrapper<GlobalMarker>
    { }

    public class Gravity_ : ConfigComponentWrapper<Gravity>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class HealthConfig_ : ConfigComponentWrapper<HealthConfig>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class HealthRegen_ : RuntimeComponentWrapper<HealthRegen>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    // public class Health_ : RuntimeComponentWrapper<Health>
    // {
    //     public override void AddTo(Entity entity, GameObject gameObject)
    //     {
    //         AddComponent(entity);
    //     }
    // }

    public class Instantiate_OneFrame_ : MarkerComponentWrapper<Instantiate_OneFrame>
    { }

    public class InstantiateConfig_ : ConfigComponentWrapper<InstantiateConfig>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class InstantiateWeaponConfig_ : ConfigComponentWrapper<InstantiateWeaponConfig>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class IsGroundedSettings_ : ConfigComponentWrapper<IsGroundedSettings>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class IsGrounded_ : RuntimeComponentWrapper<IsGrounded>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class IsUsable_ : ConfigComponentWrapper<IsUsable>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class JumpSettingsComponent_ : ConfigComponentWrapper<JumpSettings>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class LastNavMeshPosition_ : RuntimeComponentWrapper<LastNavMeshPosition>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class LookConfig_ : ConfigComponentWrapper<LookConfig>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class LookDirection_ : RuntimeComponentWrapper<LookDirection>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class MeleeWeaponMarker_ : MarkerComponentWrapper<MeleeWeaponMarker>
    { }

    public class MeleeWeaponSettings_ : ConfigComponentWrapper<MeleeWeaponSettings>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class MeshRendererRef_ : RuntimeComponentWrapper<MeshRendererRef>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var c = ref AddComponent(entity);
            c.Value = gameObject.GetComponent<MeshRenderer>();
        }
    }

    public class MoveDirection_ : RuntimeComponentWrapper<MoveDirection>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class NavMeshAgentRef_ : RuntimeComponentWrapper<NavMeshAgentRef>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var c = ref AddComponent(entity);
            c.Value = gameObject.GetComponent<AgentAuthoring>();
        }
    }

    public class OneFrameMarker_ : MarkerComponentWrapper<OneFrameMarker>
    { }

    public class PlayerInput_Jump_IsPressed_ : RuntimeComponentWrapper<PlayerInput_Jump_IsPressed>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class PlayerInput_LookDirection_ : RuntimeComponentWrapper<PlayerInput_LookDirection>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class PlayerInput_MoveBackward_IsPressed_ : RuntimeComponentWrapper<PlayerInput_MoveBackward_IsPressed>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class PlayerInput_MoveDirection_ : RuntimeComponentWrapper<PlayerInput_MoveDirection>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class PlayerInput_MoveForward_IsPressed_ : RuntimeComponentWrapper<PlayerInput_MoveForward_IsPressed>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class PlayerInput_MoveRight_IsPressed_ : RuntimeComponentWrapper<PlayerInput_MoveRight_IsPressed>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class PlayerInput_PrimaryAttack_IsPressed_ : RuntimeComponentWrapper<PlayerInput_PrimaryAttack_IsPressed>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class PlayerInput_SecondaryAttack_IsPressed_ : RuntimeComponentWrapper<PlayerInput_SecondaryAttack_IsPressed>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class PlayerInput_Sprint_IsPressed_ : RuntimeComponentWrapper<PlayerInput_Sprint_IsPressed>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class PlayerMarker_ : MarkerComponentWrapper<PlayerMarker>
    { }

    public class PlayerMoveSpeedSettings_ : ConfigComponentWrapper<PlayerMoveSpeedSettings>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class PlayerMoveSpeed_ : RuntimeComponentWrapper<MoveSpeed>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class PointMarker_ : MarkerComponentWrapper<PointMarker>
    { }

    public class PrimaryMarker_ : MarkerComponentWrapper<PrimaryMarker>
    { }

    public class SecondaryMarker_ : MarkerComponentWrapper<SecondaryMarker>
    { }

    public class Target_ : RuntimeComponentWrapper<Target>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class TargetWorldPosition_ : RuntimeComponentWrapper<TargetWorldPosition>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class TransformPartsRef_ : RuntimeComponentWrapper<TransformPartsRef>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var c = ref AddComponent(entity);
            c.Value = gameObject.GetComponent<TransformParts>();
        }
    }

    public class TransformRef_ : RuntimeComponentWrapper<TransformRef>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity).Value = gameObject.transform;
        }
    }

    public class VerticalVelocity_ : RuntimeComponentWrapper<VerticalVelocity>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class WeaponConfig_ : ConfigComponentWrapper<WeaponConfig>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class WeaponHitEvent_ : MarkerComponentWrapper<WeaponHitEvent>
    { }

    public class WeaponRef_ : RuntimeComponentWrapper<WeaponRef>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var c = ref AddComponent(entity);
            c.Value = gameObject.GetComponent<IWeapon>();
        }
    }

    public class WeaponSlotID_ : ConfigComponentWrapper<WeaponSlotID>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class WorldPosition_ : RuntimeComponentWrapper<WorldPosition>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class CinemachineCameraTargetRef_ : RuntimeComponentWrapper<CinemachineCameraTargetRef>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity).Value = gameObject.GetComponent<PlayerPrefab>().CinemachineCameraTarget;
        }
    }

    public class DashAbilitySettings_ : ConfigComponentWrapper<DashAbilitySettings>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class DashAbilityMarker_ : MarkerComponentWrapper<DashAbilityMarker>
    { }

    public class Damage_ : RuntimeComponentWrapper<Damage>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class DamageDirection_ : RuntimeComponentWrapper<DamageDirection>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class Construct_OneFrame_ : MarkerComponentWrapper<Construct_OneFrame>
    { }

    public class Cooldown_ : ConfigComponentWrapper<Cooldown>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class ChainMarker_ : MarkerComponentWrapper<ChainMarker>
    { }

    public class BotPrefabRef_ : RuntimeComponentWrapper<BotPrefabRef>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var c = ref AddComponent(entity);
            c.Value = gameObject.GetComponent<BotPrefab>();
        }
    }

    public class BotMarker_ : MarkerComponentWrapper<BotMarker>
    { }

    public class BotDeathSettings_ : ConfigComponentWrapper<BotDeathSettings>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }

    public class WeaponHit : ConfigComponentWrapper
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            entity.AddComponent<Damage>();
            entity.AddComponent<WeaponHitEvent>();
            entity.AddComponent<OneFrameMarker>();
        }
    }

    public class AoeWeaponHit : WeaponHit
    {
        public LayerMask HitMask;
        public float Radius;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            base.AddTo(entity, gameObject);
            
            ref var aoeConfig = ref entity.AddComponent<AoeConfig>();
            aoeConfig.Radius = Radius;
            aoeConfig.HitMask = HitMask;

            entity.AddComponent<AoEMarker>();
        }
    }

    public class DamageExplosion : ConfigComponentWrapper
    {
        public DamageExplosionConfig Config;
        
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            ref var c = ref entity.AddComponent<DamageExplosionConfig>();
            c = Config;
        }
    }

    public class AoEMarker_ : MarkerComponentWrapper<AoEMarker>
    { }

    public class AoeConfig_ : ConfigComponentWrapper<AoeConfig>
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            AddComponent(entity);
        }
    }
}