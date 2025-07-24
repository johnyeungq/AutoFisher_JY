using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using System.Threading.Tasks;
using System.Threading;

namespace AutoFisher_JY
{
    public class AutoFisherPlayer : ModPlayer
    {
        #region Data



        private int CurrentSelectingSlot;
        private int autoFishTimer = 0;
        private int clickPhase = 0;
        private bool shouldClick = false;
        private AutoFisherConfig _config;
        private Bait _Bait = new Bait(); // Assuming you have a _Bait class that contains bait information

        #region Config
        private int CatchKey() { return _config.ToggleAutoCatchKey - 1; }
        private bool AutoCatchingEnabled() { return _config.AutoCatching; }


        private int keybindToggle() { return _config.ToggleAutoFisherKey - 1; }
        private bool EnableAutoFisher() { return _config.EnableAutoFisher; }

        private bool FishingGodModeEnabled() { return _config.EnableGodMode; }

        private bool AutoBuff() { return _config.AutoBuff; }
        private bool buffOnlyFishing() { return _config.BuffOnlyFishing; }
        private int AutoBuffTimer() { return _config.AutoBuffTimer; }

        private bool DisableLog() { return _config.DisableLog; }
        private bool SpecialLog() { return _config.EnableSpecialLog; }
        private int _AutoBuffTimer = 0; // Timer for auto poison


        private bool CatchAll() { return _config.CatchAllCritters; }
        private bool AutoSwitch() { return _config.AutoSwitch; }
    
        #endregion

        #endregion

        #region Loading
        public override void OnEnterWorld()
        {
            if (_config == null)
            {
                _config = ModContent.GetInstance<AutoFisherConfig>();
            }
            if(_Bait == null)
            {
                _Bait = new Bait();
            }   
            ResetAutoFish();

            if (SpecialLog())
            {
                string version = ModContent.GetInstance<AutoFisher_JY>().Version.ToString();
                Main.NewText("[AF]AutoFisher_JY Mod Loaded!", 50, 255, 130);
                Main.NewText($"[AF]Version: {version}", 50, 255, 130);
                Main.NewText("[AF] 0.1.6 Updated! Check Config to enable / disable AutoSwitch and Catch Critters for Fishing only.", 50, 255, 130);

            }
         //   UpdateSlot(); // Initialize the current selecting slot
        }
        #endregion

        #region Update

        public async override void PreUpdate()
        {
          

          
            bool _isFishing = false;
          

            if (IsCritterNearby()&&AutoSwitch()) {

                UpdateSlot();
                await Task.Delay(1);


                Player.selectedItem = CatchKey(); 

                
                await Task.Delay(1000); // delay and select prvious item
                Player.selectedItem = CurrentSelectingSlot; //not fucking working, fix it later

            }
            else {
               Player.selectItemOnNextUse = false; // Reset the selection if no critter is nearby
            }
            



            if (!EnableAutoFisher())
            {
                ResetAutoFish();
                return;
            }
      



            if (AutoCatchingEnabled() && Player.selectedItem == CatchKey() && IsNetItem(Player.inventory[CatchKey()]) && Player.itemTime == 0 && Player.itemAnimation == 0 && !Player.noItems && clickPhase == 0 && IsCritterNearby())
            {

                Requestreel();
                clickPhase = 1;// oh fuck i forgot, this is useless i think no matter i pass the phase or not, it will still do the thing lmfao
                if (!DisableLog())
                {
                    
                }
                CombatText.NewText(Player.getRect(), Color.LightGreen, "Auto-Catch: Cast!");
            }
            else if (clickPhase == 1)
            {
                clickPhase = 0;
            }





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
                                if (!DisableLog())
                                {
                                    Main.NewText("Auto-fishing: On HooK!", 50, 255, 130);
                                }
                                Requestreel();
                            }
                            break;
                        }
                    }

                    // starting to think this is not needed, but whatever
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


            else
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
                        if (!DisableLog())
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
        #endregion

        #region Triggers
      
        void OnDisable()
        {
            ResetAutoFish();
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

        #endregion

        #region CommonFunctions

        void UpdateSlot() { 
        

        CurrentSelectingSlot = Player.selectedItem; // Update the current selecting slot
                
        }
        void ResetAutoFish()
        {
            autoFishTimer = 0;
            clickPhase = 0;
            shouldClick = false;
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
        #endregion

        #region Define
        private bool IsPlayerPoisoned()
        {
            return Player.poisoned;
        }
        private bool IsCritterNearby()
        {

            float detectionRange = 10f * 16f;
            foreach (var npc in Main.npc)
            {

                if (npc.active && npc.catchItem > 0)
                {
                    

                    float distance = Vector2.Distance(Player.Center, npc.Center);
                    if (distance <= detectionRange)
                    {
                        bool isSuitableBait = IsBaitSuitable(npc);
                        if (CatchAll())
                        {
                            return true;
                        }
                        else
                        {
                            if (!isSuitableBait)
                            {
                               
                                return false;
                            }
                            else
                            {
                                return true;

                            }
                        }
                        
                    }
                }
            }
            return false;
        }

        



        private bool IsNetItem(Item item)
        {
            if (item == null || item.type <= ItemID.None)
            {
                return false;
            }
            // Main.NewText(item.ToString(), 50, 255, 130);
            return ItemID.Sets.CatchingTool[item.type];
        }
        private bool IsBaitSuitable(NPC npc)
        {
            string[] BaitDictionary = _Bait.BaitDictionary; // Replace with actual bait names
            foreach (var bait in BaitDictionary)
            {
                if (npc.type.ToString()==bait)
                {
                    
                    return true;
                }
            }
            return false;
        }
        #endregion


    }
}