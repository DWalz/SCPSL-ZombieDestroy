using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EXILED;
using EXILED.Extensions;
using UnityEngine;

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
        private int neededAttacks = 2;
        private readonly Dictionary<Door, Dictionary<ReferenceHub, Stopwatch>> _attackedDoors = 
            new Dictionary<Door, Dictionary<ReferenceHub, Stopwatch>>();

        public DoorAttackHandler()
        {
            if (Player.GetHubs().Count() > Configs.TwoZombiesLimit)
            {
                neededAttacks = 3;
            }   
        }

        public bool AddAttack(Door door, ReferenceHub player)
        {
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
            
            if (_attackedDoors[door].Count >= neededAttacks)
            {
                _attackedDoors.Remove(door);
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