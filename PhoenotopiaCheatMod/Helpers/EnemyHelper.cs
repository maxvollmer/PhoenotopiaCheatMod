namespace PhoenotopiaCheatMod.Helpers
{
    public class EnemyHelper
    {
        /*
        public static IEnumerable<GameObject> FindClosestHitbox()
        {
            foreach (var hitbox in Object.FindObjectsOfType(typeof(Hitbox)).Select(hb => hb as Hitbox).Where(hb => hb != null))
            {
                if (!hitbox.gameObject.activeSelf || !hitbox.gameObject.activeInHierarchy)
                    continue;

                if (hitbox.team_orientation != Hitbox.TEAM_ORIENTATION.ENEMY)
                    continue;

                var hurtable = Traverse.Create(hurtbox).Field("_hurtable").GetValue<Object>();
                MainEntry.Mod.Logger.Log("hurtable: " + hurtable);

                if (hurtable == null)
                    continue;

                MainEntry.Mod.Logger.Log("hurtable.name: " + hurtable.name);

                if (whatToKill == WhatToKill.ENEMIES && !hurtable.name.StartsWith("Enemy"))
                    continue;

                if (whatToKill == WhatToKill.THINGS && hurtable.name.StartsWith("Enemy"))
                    continue;

                var receiveAttackResultMethod = hurtable.GetType().GetMethod("ReceiveAttackResult");
                if (receiveAttackResultMethod != null)
                {
                    receiveAttackResultMethod.Invoke(hurtable, new object[] { CreateOverpoweredAttackResult(hurtbox) });
                }
            }
        }
        */
    }
}
