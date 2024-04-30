using UnityEngine;

public class CrunchFoodTriggerZone : TriggerEffectZone
{
    protected override void TriggerFunc(Collider other)
    {
        PlayerSystems.PlayerBase.Player player = other.GetComponent<PlayerSystems.PlayerBase.Player>();

        if(player)
        {
            player.CrunchFoodInHands();
        }
    }
}
