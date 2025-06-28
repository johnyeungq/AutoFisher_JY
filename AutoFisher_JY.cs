using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.Config;
using Terraria.ID;
using Terraria.UI;
using Microsoft.Xna.Framework.Input;

namespace AutoFisher_JY
{
    public class AutoFisher_JY : Mod
    {
        public static ModKeybind ToggleKeybind;

        public override void Load()
        {
            
         
        }

        public override void Unload()
        {
            ToggleKeybind = null;
        }
    }
}