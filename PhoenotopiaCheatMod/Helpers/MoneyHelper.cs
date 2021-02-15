
using HarmonyLib;

namespace PhoenotopiaCheatMod.Helpers
{
    public class MoneyHelper
    {
        public static void SetMoney(int newamount, bool dontGoLower = false)
        {
            if (newamount < 0)
                newamount = 0;

            int oldamount = PT2.save_file.GetInt((int)SaveFile.SAVE_ENUMS.MONEY);
            if (oldamount == newamount)
                return;

            bool goingLower = oldamount > newamount;

            if (goingLower && dontGoLower)
                return;

            PT2.save_file.SetGeneralInts(SaveFile.SAVE_ENUMS.MONEY, newamount);

            int fakeprevamount = goingLower ? (newamount + 10) : (newamount - 10);
            Traverse.Create(PT2.hud_money).Field("_curr_display_amt").SetValue(fakeprevamount);
            PT2.hud_money.UpdateGraphic(PickupLogic.PICKUP_CLASS.P1_RAI, fakeprevamount, true);
        }
    }
}
