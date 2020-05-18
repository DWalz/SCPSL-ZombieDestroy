using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EXILED;
using EXILED.Extensions;

namespace SCPSL_ZombieDestroy
{
    public class EventHandlers
    {
        private readonly DoorAttackHandler _attackHandler = new DoorAttackHandler();

        public void OnPlayerDoorInteract(ref DoorInteractionEvent ev)
        {
            Door attackedDoor = ev.Door;
            if (ev.Player.GetRole() == RoleType.Scp0492)
            {
                if (_attackHandler.AddAttack(attackedDoor, ev.Player))
                {
                    attackedDoor.DestroyDoor(true);
                    if (Configs.BreakUnbreakable)
                    {
                        attackedDoor.Networkdestroyed = true;
                    }
                }
            }
        }
        
    }
    

    class DoorAttackHandler
    {
        private readonly int _neededAttacks = 2;
        private readonly Dictionary<Door, Dictionary<ReferenceHub, Stopwatch>> _attackedDoors = 
            new Dictionary<Door, Dictionary<ReferenceHub, Stopwatch>>();
        private readonly Dictionary<ReferenceHub, Stopwatch> _zombieCooldowns = new Dictionary<ReferenceHub, Stopwatch>();

        public DoorAttackHandler()
        {
            if (Player.GetHubs().Count() > Configs.TwoZombiesLimit)
            {
                _neededAttacks = 3;
            }   
        }

        public bool AddAttack(Door door, ReferenceHub player)
        {
            if (_zombieCooldowns.TryGetValue(player, out Stopwatch playerCooldown))
            {
                if (!(playerCooldown.ElapsedMilliseconds > Configs.ZombieCooldown * 1000))
                {
                    return false;
                }
                playerCooldown.Stop();
                _zombieCooldowns.Remove(player);
            }
            if (_attackedDoors.TryGetValue(door, out Dictionary<ReferenceHub, Stopwatch> attackingPlayers))
            {
                if (attackingPlayers.ContainsKey(player))
                {
                    attackingPlayers[player].Stop();
                }
                Stopwatch st = new Stopwatch();
                st.Start();
                attackingPlayers[player] = st;
            }
            else
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                _attackedDoors[door] = new Dictionary<ReferenceHub, Stopwatch> {{player, st}};
            }
            
            if (_attackedDoors[door].Count >= _neededAttacks)
            {
                _attackedDoors.Remove(door);
                Stopwatch st = new Stopwatch();
                st.Start();
                _zombieCooldowns.Add(player, st);
                return true;
            }
            
            RemovePlayerAttack(door, player);
            return false;
        }

        private async void RemovePlayerAttack(Door door, ReferenceHub player)
        {
            await Task.Delay(10000);
            if (_attackedDoors[door][player].ElapsedMilliseconds >= 10000)
            {
                _attackedDoors[door][player].Stop();
                _attackedDoors[door].Remove(player);
                if (_attackedDoors[door].Count == 0)
                {
                    _attackedDoors.Remove(door);
                }
            }
        }
        
    }
    
}