using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using US.LocationUI.Types;

namespace US.LocationUI.Commands
{
    internal class LocationSet : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "locationSet";

        public string Syntax => "Когда вы в городе, на улице добавьте название через команду /locationset <name> ";

        public List<string> Aliases => new List<string> { "ls"};

        public List<string> Permissions => new List<string> { "US.locationset"};

        public string Help => "";

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer)caller;
            bool flag = command.Length < 2;
            if (flag)
            {
                UnturnedChat.Say(unturnedPlayer, Plugin.Instance.Translate("Wrong"), Color.red);
                return;
            }
            else
            {
                Plugin.Instance.Configuration.Instance.Locations.Add(new LocationData
                {
                    Name = command[0],
                    Radius = Convert.ToUInt64(command[1]),
                    Position = unturnedPlayer.Position
                });
                UnturnedChat.Say(unturnedPlayer, Plugin.Instance.Translate("Successfully"), Color.red);
                Plugin.Instance.Configuration.Save();
            }
        }
    }
}
