using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using US.LocationUI.Types;
using System.Collections;
using Rocket.Unturned.Extensions;

namespace US.LocationUI
{
    internal class Plugin : RocketPlugin<Configuration>
    {
        public static Plugin Instance;

        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "", "==================================================UNTURNED SHOP==================================================" },
            { "WelcomeMessage", "Здравствуй дорогой игрок,спасибо что зашел на наш сервер,надеюсь тебе понравится в нашем штате!" },
            { "Wrong", "Неправильно введена команда: /locationset name radius" },
            { "Successfully", "Вы успешно добавили новую локацию!" },
            { "", "==================================================UNTURNED SHOP==================================================" },
        };

        protected override void Load()
        {
            Plugin.Instance = this;
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
        }

        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            EffectManager.sendUIEffect(Instance.Configuration.Instance.PluginEffectID, Instance.Configuration.Instance.PluginEffectKey, player.CSteamID, true);
            var location = PlayerLocation(player);
            if (Vector3.Distance(player.Position, location.Position) < location.Radius)
            {
                EffectManager.sendUIEffectVisibility(Plugin.Instance.Configuration.Instance.PluginEffectKey, player.CSteamID, true, "Location", true);
                EffectManager.sendUIEffectText(Instance.Configuration.Instance.PluginEffectKey, player.CSteamID, true, "LocationText", location.Name);
            }
            else
            {
                EffectManager.sendUIEffectVisibility(Plugin.Instance.Configuration.Instance.PluginEffectKey, player.CSteamID, true, "Location", false);
            }

            UnturnedChat.Say(player, Plugin.Instance.Translate("WelcomeMessage"), Color.red);
        }

        public IEnumerator LocationUpdate()
        {
            yield return new WaitForSeconds(3);
            
            foreach (var stpl in Provider.clients)
            {
                UnturnedPlayer player = stpl.ToUnturnedPlayer();
                var location = PlayerLocation(player);
                if (Vector3.Distance(player.Position, location.Position) < location.Radius)
                {
                    EffectManager.sendUIEffectVisibility(Plugin.Instance.Configuration.Instance.PluginEffectKey, player.CSteamID, true, "Location", true);
                    EffectManager.sendUIEffectText(Instance.Configuration.Instance.PluginEffectKey, player.CSteamID, true, "LocationText", location.Name);
                }
                else
                {
                    EffectManager.sendUIEffectVisibility(Plugin.Instance.Configuration.Instance.PluginEffectKey, player.CSteamID, true, "Location", false);
                }
            }

            StartCoroutine(LocationUpdate());
        }

        public LocationData PlayerLocation(UnturnedPlayer player)
        {
            float num = Vector3.Distance(player.Position, Plugin.Instance.Configuration.Instance.Locations[0].Position);
            bool flag = Plugin.Instance.Configuration.Instance.Locations.Count == 0;
            LocationData result;
            if (flag)
            {
               result = null;
            }
            else
            {
                LocationData locationData = Plugin.Instance.Configuration.Instance.Locations[0];
                foreach (LocationData locationData2 in Plugin.Instance.Configuration.Instance.Locations)
                {
                    bool flag2 = Vector3.Distance(player.Position, locationData2.Position) < num;
                    if (flag2)
                    {
                        num = Vector3.Distance(player.Position, locationData2.Position);
                        locationData = locationData2;
                    }
                }
                result = locationData;
            }
            return result;
        }

        protected override void Unload()
        {
            Plugin.Instance = null;
        }
    } 
}
