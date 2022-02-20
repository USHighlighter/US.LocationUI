using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using US.LocationUI.Types;

namespace US.LocationUI
{
    public class Configuration : IRocketPluginConfiguration
    {
        public List<LocationData> Locations;

        public ushort PluginEffectID;

        public short PluginEffectKey;

        public void LoadDefaults()
        {
            PluginEffectID = 15638;
            PluginEffectKey = 15638;

            Locations = new List<LocationData>();
        }

    }
}
