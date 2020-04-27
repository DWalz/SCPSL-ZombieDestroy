using System.Collections.Generic;
using System.Threading.Tasks;
using EXILED;
using EXILED.Extensions;
using UnityEngine;

namespace SCPSL_ZombieDestroy
{
    public class EventHandlers
    {
        private static Dictionary<Door, List<ReferenceHub>> attackedDoors = new Dictionary<Door, List<ReferenceHub>>();
        
        public void OnPlayerDoorInteract(ref DoorInteractionEvent ev)
        {
            if (ev.Player.GetRole() == RoleType.Scp0492)
            {
                Log.Info(ev.Player.GetNickname() + " is attacking the door.");
                if (attackedDoors.TryGetValue(ev.Door, out List<ReferenceHub> players))
                {
                    // if (!players.Contains(ev.Player)) {
                    players.Add(ev.Player);
                    attackedDoors[ev.Door] = players;
                    // }
                }
                else
                {
                    attackedDoors.Add(ev.Door, new List<ReferenceHub> {ev.Player});
                }
                doorAttackTimeout(ev.Door, ev.Player);
                destroyDoorOnAttack(ev.Door);
            }
        }

        private void destroyDoorOnAttack(Door door)
        {
            Log.Info(attackedDoors[door].Count + "/3");
            if (attackedDoors[door].Count >= 3)
            {
                Log.Info("Door destroyed");
                door.DestroyDoor(true);
                door.Networkdestroyed = true;
                attackedDoors.Remove(door);
            }
        }

        private async void doorAttackTimeout(Door door, ReferenceHub player)
        {
            await Task.Delay(10000);
            Log.Info(player.GetNickname() + " is no longer attacking the door.");
            attackedDoors[door].Remove(player);
        }
        
    }
}