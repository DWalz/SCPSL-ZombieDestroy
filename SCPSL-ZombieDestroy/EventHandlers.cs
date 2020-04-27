using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using EXILED;
using EXILED.Extensions;
using UnityEngine;

namespace SCPSL_ZombieDestroy
{
    public class EventHandlers
    {
        private static readonly DoorAttackHandler AttackHandler = new DoorAttackHandler();
        
        public void OnPlayerDoorInteract(ref DoorInteractionEvent ev)
        {
            Door attackedDoor = ev.Door;
            if (ev.Player.GetRole() == RoleType.Scp0492)
            {
                Log.Info($"{ev.Player.GetNickname()} is attacking a door.");
                if (AttackHandler.AddAttack(attackedDoor, ev.Player))
                {
                    Log.Info("Destroyed door.");
                    attackedDoor.DestroyDoor(true);
                    attackedDoor.Networkdestroyed = true;
                }
            }
        }
        
    }
    

    class DoorAttackHandler
    {
        private static readonly Dictionary<Door, Dictionary<ReferenceHub, Stopwatch>> AttackedDoors = 
            new Dictionary<Door, Dictionary<ReferenceHub, Stopwatch>>();

        public bool AddAttack(Door door, ReferenceHub player)
        {
            if (AttackedDoors.TryGetValue(door, out Dictionary<ReferenceHub, Stopwatch> attackingPlayers))
            {
                Log.Info("Door has been attacked before.");
                if (attackingPlayers.ContainsKey(player))
                {
                    Log.Info($"{player.GetNickname()} has attacked this door before. Resetting cooldown");
                    attackingPlayers[player].Stop();
                }
                Stopwatch st = new Stopwatch();
                st.Start();
                attackingPlayers[player] = st;
            }
            else
            {
                Log.Info("Door hasn't been attacked before. Setting up attacking state.");
                Stopwatch st = new Stopwatch();
                st.Start();
                AttackedDoors[door] = new Dictionary<ReferenceHub, Stopwatch> {{player, st}};
            }
            
            if (AttackedDoors[door].Count >= 2)
            {
                AttackedDoors.Remove(door);
                return true;
            }
            
            removePlayerAttack(door, player);
            return false;
        }

        private async void removePlayerAttack(Door door, ReferenceHub player)
        {
            await Task.Delay(10000);
            Log.Info($"Time since last attack: {AttackedDoors[door][player].ElapsedMilliseconds}");
            if (AttackedDoors[door][player].ElapsedMilliseconds >= 10000)
            {
                AttackedDoors[door][player].Stop();
                AttackedDoors[door].Remove(player);
                if (AttackedDoors[door].Count == 0)
                {
                    Log.Info("Door has no attackers.");
                    AttackedDoors.Remove(door);
                }
            }
        }
        
    }
    
}