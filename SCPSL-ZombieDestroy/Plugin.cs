using System;
using EXILED;

namespace SCPSL_ZombieDestroy
{
    public class ZombieDestroy : Plugin
    {

        private string _name = "ZombieDestroy";

        private EventHandlers _handlers;

        public override void OnEnable()
        {
            Log.Info("Enabling ZombieDestroy");
            try
            {
                Log.Info("Loading Config");
                Configs.ReloadConfigs();

                if (!Configs.IsEnabled) return;
                
                Log.Info("Registering event handlers");
                _handlers = new EventHandlers();
                Events.DoorInteractEvent += _handlers.OnPlayerDoorInteract;
            }
            catch (Exception)
            {
                Log.Error("Registering event handlers failed!");
            }
        }

        public override void OnDisable()
        {
            if (!Configs.IsEnabled) return;
            
            Log.Info("De-registering event handlers");
            Events.DoorInteractEvent -= _handlers.OnPlayerDoorInteract;
            _handlers = null;
        }

        public override void OnReload()
        {
            OnDisable();
            OnEnable();
        }

        public override string getName { get => _name; }
        
    }
}