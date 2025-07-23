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



        [Label("Buff Only Fishing")]
        [Tooltip("Enable this to apply buffs only when fishing.")]
        [DefaultValue(false)]
        public bool BuffOnlyFishing { get; set; }
       
        [Label("Enable Special Log")]
        [Tooltip("Enable this to log watch the updates & others")]
        [DefaultValue(true)]
        public bool EnableSpecialLog { get; set; }


        [Label("Auto Catch")]
        [Tooltip("Enable this to apply Auto Catch.")]
        
        [DefaultValue(true)]
        public bool AutoCatching { get; set; }

        [Label("Toggle Auto Catch Keybind")]
        [Tooltip("Select a key (1-10) to bind for toggling Auto-Fisher.")]
        [DefaultValue(10)]
        [Range(1, 10)]
        public int ToggleAutoCatchKey { get; set; }

    }
}