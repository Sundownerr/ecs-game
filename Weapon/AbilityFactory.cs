using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    // public class AbilityFactory : MonoBehaviour
    // {
    //     [SerializeField] private AbilityConfig[] _weaponConfigs;
    //
    //     public Entity Create(string id, Entity weaponSlot)
    //     {
    //         var weaponConfig = AbilityConfig(id);
    //         var weaponGO = Instantiate(weaponConfig.Prefab, weaponSlot.GetComponent<TransformRef>().Value);
    //         var weaponComponent = weaponGO.GetComponent<IWeapon>();
    //
    //         var weaponEntity = World.Default.CreateEntity();
    //         ref var weapon = ref weaponEntity.AddComponent<Ability>();
    //         ref var weaponTransform = ref weaponEntity.AddComponent<TransformRef>();
    //         ref var cooldown = ref weaponEntity.AddComponent<Cooldown>();
    //         ref var isUsable = ref weaponEntity.AddComponent<IsUsable>();
    //
    //         weaponTransform.Value = weaponGO.transform;
    //         weapon.Value = weaponComponent;
    //         cooldown.Max = weaponConfig.ShootInterval;
    //         isUsable.Value = true;
    //
    //         return weaponEntity;
    //     }
    //
    //     private AbilityConfig AbilityConfig(string id)
    //     {
    //         foreach (var weaponConfig in _weaponConfigs)
    //         {
    //             if (weaponConfig.Id == id)
    //             {
    //                 return weaponConfig;
    //             }
    //         }
    //
    //         return null;
    //     }
    // }
}