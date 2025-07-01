using Terraria.ModLoader.Config;
using System.ComponentModel;
using Terraria.ModLoader;

namespace AutoFisher_JY
{
    public class AutoFisherConfig : ModConfig  //Know as _config in AutoFisherPlayer.cs  
    {    

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Enable Auto Fisher")]
        [DefaultValue(true)]
        public bool EnableAutoFisher { get; set; }

        [Label("Enable GodMode")]
        [Tooltip("This Mod will Break Game Balance but good for only testing")]
        [DefaultValue(false)]
        public bool EnableGodMode { get; set; }
        
        [Label("Disable Log")]
        [Tooltip("Enabling by Default")]
        [DefaultValue(false)]
        public bool DisableLog { get; set; }



        [Label("Toggle Auto-Fisher Keybind")]
        [Tooltip("Select a key (1-9) to bind for toggling Auto-Fisher.")]
        [DefaultValue(1)]
        [Range(1, 9)]
        public int ToggleAutoFisherKey { get; set; }


        [Label("Enable Auto Buff")]
        [DefaultValue(false)]
        public bool AutoBuff { get; set; } 

        [Label("Auto Buff Timer")]
        [Tooltip("Select Time (mins) to bind for reset buff")]
        [DefaultValue(8)]
        [Range(1, 59)]
        public int AutoBuffTimer { get; set; }

    }
}