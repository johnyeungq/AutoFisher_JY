using Terraria.ModLoader.Config;
using System.ComponentModel;
using Terraria.ModLoader;

namespace AutoFisher_JY
{
    public class AutoFisherConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Enable Auto Fisher")]
        [DefaultValue(true)]
        public bool EnableAutoFisher { get; set; }

        [Label("Enable GodMode")]
        [Tooltip("This Mod will Break Game Balance but good for only testing")]
        [DefaultValue(false)]
        public bool EnableGodMode { get; set; }



        [Label("Toggle Auto-Fisher Keybind")]
        [Tooltip("Select a key (1-9) to bind for toggling Auto-Fisher.")]
        [DefaultValue(1)]
        [Range(1, 9)]
        public int ToggleAutoFisherKey { get; set; }

      
    }
}