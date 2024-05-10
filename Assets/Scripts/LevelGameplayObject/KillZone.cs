using ParticleSystemUtility;
using System.Collections;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;
    [SerializeField] GameObject portalParticles;
    [SerializeField] bool clearHands = true;
    [SerializeField] Vector3 fallPortalOffset;
    [SerializeField] Vector3 spawnPortalOffset;
    [SerializeField] float portalDuration;
    [SerializeField] float startTpDelay;
    [SerializeField] float spawnDelay;

    void OnTriggerEnter(Collider other)
    {
        PlayerSystems.PlayerBase.Player player = other.GetComponent<PlayerSystems.PlayerBase.Player>();
        if (player)
        {
            if (clearHands)
            {
                player.CrunchFoodInHands(true);
            }
            
            StartCoroutine(StartTeleportDelay(player));
        }
        else if(other.GetComponent<IDestructible>() != null)
        {
            Destroy(other.gameObject);
        }
    }

    IEnumerator StartTeleportDelay(PlayerSystems.PlayerBase.Player player)
    {
        yield return new WaitForSeconds(startTpDelay);
        Vector3 spawnPoint = player.transform.position + fallPortalOffset;
        ParticleInstanceManager portal = Instantiate(portalParticles, spawnPoint, Quaternion.identity).GetComponent<ParticleInstanceManager>();
        StartCoroutine(DestroyPortalDelay(portal));

        spawnPoint = respawnPoint.position + spawnPortalOffset;
        ParticleInstanceManager portal2 = Instantiate(portalParticles, spawnPoint, Quaternion.identity).GetComponent<ParticleInstanceManager>();
        StartCoroutine(DestroyPortalDelay(portal2));
        StartCoroutine(SpawnPlayerDelay(player));
    }

    IEnumerator DestroyPortalDelay(ParticleInstanceManager portal)
    {
        yield return new WaitForSeconds(portalDuration);
        portal.Stop(false);
    }

    IEnumerator SpawnPlayerDelay(PlayerSystems.PlayerBase.Player player)
    {
        yield return new WaitForSeconds(spawnDelay);
        player.ClearPlayerStamina();
        player.SetPosition(respawnPoint.position);
    }
}
