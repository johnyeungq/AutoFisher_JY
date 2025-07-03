using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace AutoFisher_JY
{
    public class AutoFisherPlayer : ModPlayer
    {
        
       
        private int autoFishTimer = 0;
        private int clickPhase = 0;
        private bool shouldClick = false;
        private AutoFisherConfig _config;

        private int keybindToggle() { return _config.ToggleAutoFisherKey -1 ; }  // int key =  slot number > slot -1  
        private bool EnableAutoFisher() { return _config.EnableAutoFisher; }    
        private bool FishingGodModeEnabled() { return _config.EnableGodMode; } // This is used to check if the god mode is enabled or not  
        private int AutoBuffTimer() { return _config.AutoBuffTimer; } 
        private bool AutoBuff() { return _config.AutoBuff; }
        private bool DisableLog() { return _config.DisableLog; }
        private bool buffOnlyFishing() { return _config.BuffOnlyFishing; } // This is used to check if the buff only fishing is enabled or not
        private bool SpecialLog() { return _config.EnableSpecialLog; } // This is used to check if the special log is enabled or not
    
        void ResetAutoFish()
        {
            autoFishTimer = 0;
            clickPhase = 0;
            shouldClick = false;
        }
        void OnDisable()
        {
            ResetAutoFish();
        }
        public override void OnEnterWorld()
        {
            if(_config == null)
            {
                _config = ModContent.GetInstance<AutoFisherConfig>();
            }   
            ResetAutoFish();

            if (SpecialLog())
            {
                string version = ModContent.GetInstance<AutoFisher_JY>().Version.ToString();
                Main.NewText("[AF]AutoFisher_JY Mod Loaded!", 50, 255, 130);
                Main.NewText($"[AF]Version: {version}" , 50, 255, 130);
                //Main.NewText("Press " + _config.ToggleAutoFisherKey.ToString() + " to toggle Auto-Fisher", 50, 255, 130);
                Main.NewText("[AF] 0.1.4 Updated! Please Check Config to enable/disable this special log & 'Buff only Fishing' ", 50, 255, 130);

            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            ResetAutoFish();
        }
        public override void OnRespawn()
        {
            base.OnRespawn();
            ResetAutoFish();
        }

        // Call this to request a click , actually have no idea how it works but it works, whatever 
        private void RequestClick()
        {

            PlayerInput.GenerateInputTags_GamepadUI("MouseLeft");

            
        }
        private void RequestBuff()
        {
            PlayerInput.Triggers.Current.QuickBuff = true; // Simulate Quick Buff trigger
         //   Main.NewText("Buff", 50, 255, 130);
          
        }


        //This two methods are used to simulate mouse click and reel in the fishing line
        private void Requestreel()
        {


            PlayerInput.Triggers.Current.MouseLeft = true; // Simulate mouse click
            shouldClick = true;
        }


        private int _AutoBuffTimer = 0; // Timer for auto poison



        public override void PreUpdate()
        {
            bool _isFishing =false;
            if (!EnableAutoFisher())
            {
                ResetAutoFish();
                return;
            }

            // Check if Auto Poison is enabled
          
            if (!FishingGodModeEnabled())
            {
                Item slotItem = Player.inventory[keybindToggle()];
                if (Player.selectedItem == keybindToggle() && slotItem.fishingPole > 0 && slotItem.stack > 0)
                {
                    bool isFishing = false;
                    bool fishOnHook = false;
                  
                    foreach (var projectile in Main.projectile)
                    {
                        if (projectile.active && projectile.bobber && projectile.owner == Player.whoAmI)
                        {
                            isFishing = true;
                            _isFishing = true;
                            if (projectile.ai[1] < 0f) // Oh hook Logic
                            {
                                if (!DisableLog()){
                                    Main.NewText("Auto-fishing: On HooK!", 50, 255, 130);
                                }
                                Requestreel();
                            }
                            break;
                        }
                    }

                    //this is so fking werid, clickPhase never passed to == 1 , but this works, so whatever    
                    if (!isFishing && autoFishTimer == 0 && Player.itemTime == 0 && Player.itemAnimation == 0 && !Player.noItems && clickPhase == 0)
                    {
                        Requestreel();
                        autoFishTimer = 60; if (!DisableLog())
                        {
                            Main.NewText("Auto-fishing: Cast!", 50, 255, 130);
                        }
                        
                        CombatText.NewText(Player.getRect(), Color.Aqua, "Auto-fishing: Cast!");
                    }
                    else if (isFishing && fishOnHook && autoFishTimer == 0 && Player.itemTime == 0 && Player.itemAnimation == 0 && !Player.noItems && clickPhase == 1)
                    {
                        RequestClick();
                        autoFishTimer = 60;
                        if (!DisableLog())
                        {
                            Main.NewText("Auto-fishing: Reel in!", 50, 255, 130);
                        }
                        CombatText.NewText(Player.getRect(), Color.LightBlue, "Auto-fishing: Reel in!");
                    }
                }

                if (autoFishTimer > 0)
                    autoFishTimer--;

            }
            
            
            else {

                Item slotItem = Player.inventory[keybindToggle()];
                if (Player.selectedItem == keybindToggle() && slotItem.fishingPole > 0 && slotItem.stack > 0)
                {
                    bool isFishing = false;
                    bool fishOnHook = false;

                    foreach (var projectile in Main.projectile)
                    {
                        if (projectile.active && projectile.bobber && projectile.owner == Player.whoAmI)
                        {
                            isFishing = true;
                            _isFishing = true;
                            projectile.FishingCheck(); //God Mode Logic
                            if (projectile.ai[1] < 0f) // Oh hook Logic
                            {
                                if (!DisableLog())
                                {
                                    Main.NewText("Auto-fishing: On HooK!", 50, 255, 130);
                                }
                                Requestreel();
                            }
                            break;
                        }
                    }


                    //this is so fking werid, clickPhase never passed to == 1 , but this works, so whatever    
                    if (!isFishing && autoFishTimer == 0 && Player.itemTime == 0 && Player.itemAnimation == 0 && !Player.noItems && clickPhase == 0)
                    {
                        Requestreel();
                        autoFishTimer = 60;
                        if (!DisableLog()){
                            Main.NewText("Auto-fishing: Cast!", 50, 255, 130);
                        }
                            CombatText.NewText(Player.getRect(), Color.Aqua, "Auto-fishing: Cast!");
                    }
                    else if (isFishing && fishOnHook && autoFishTimer == 0 && Player.itemTime == 0 && Player.itemAnimation == 0 && !Player.noItems && clickPhase == 1)
                    {
                        RequestClick();
                        autoFishTimer = 60;
                        if (!DisableLog())
                        {

                            Main.NewText("Auto-fishing: Reel in!", 50, 255, 130);
                        }
                        CombatText.NewText(Player.getRect(), Color.LightBlue, "Auto-fishing: Reel in!");
                    }
                }

                if (autoFishTimer > 0)
                    autoFishTimer--;



            }



            if (AutoBuff())
            {
                if (buffOnlyFishing() && !_isFishing) // If buff only fishing is enabled and not fishing, skip buffing
                {
                    return;
                }
                if (_AutoBuffTimer <= 0)
                {
                    
                    RequestBuff();
                    _AutoBuffTimer = AutoBuffTimer() * 60 * 60; // Reset timer
                    if (!DisableLog())
                    {
                        Main.NewText($"BufferTimer:{AutoBuffTimer().ToString()} mins", 50, 255, 130);
                    }
                }
                else
                {
                    _AutoBuffTimer--; // Decrease timer
                }
            }


        }
        private bool IsPlayerPoisoned()
        {
            return Player.poisoned;
        }
       
    }
}