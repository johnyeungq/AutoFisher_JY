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

        // Call this to request a click
        private void RequestClick()
        {

            PlayerInput.GenerateInputTags_GamepadUI("MouseLeft");


        }
        private void Requestree()
        {


            PlayerInput.Triggers.Current.MouseLeft = true; // Simulate mouse click
            shouldClick = true;
        }

        public override void PreUpdate()
        {
           
            // Your fishing logic...
            Item slot1Item = Player.inventory[0]; // Update to use slotIndex
            if (Player.selectedItem == 0 && slot1Item.fishingPole > 0 && slot1Item.stack > 0)
            {
                bool isFishing = false;
                bool fishOnHook = false;

                foreach (var projectile in Main.projectile)
                {
                    if (projectile.active && projectile.bobber && projectile.owner == Player.whoAmI)
                    {
                        isFishing = true;

                        if (projectile.ai[1] < 0f)
                        {
                            Main.NewText("Auto-fishing: On HooK!", 50, 255, 130);
                            Requestree();
                        }
                        break;
                    }
                }

                if (!isFishing && autoFishTimer == 0 && Player.itemTime == 0 && Player.itemAnimation == 0 && !Player.noItems && clickPhase == 0)
                {
                    Requestree();
                    autoFishTimer = 60;
                    Main.NewText("Auto-fishing: Cast!", 50, 255, 130);
                    CombatText.NewText(Player.getRect(), Color.Aqua, "Auto-fishing: Cast!");
                }
                else if (isFishing && fishOnHook && autoFishTimer == 0 && Player.itemTime == 0 && Player.itemAnimation == 0 && !Player.noItems && clickPhase == 1)
                {
                    RequestClick();
                    autoFishTimer = 60;
                    Main.NewText("Auto-fishing: Reel in!", 50, 255, 130);
                    CombatText.NewText(Player.getRect(), Color.LightBlue, "Auto-fishing: Reel in!");
                }
            }

            if (autoFishTimer > 0)
                autoFishTimer--;
        }
    }
}