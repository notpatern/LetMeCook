using Manager;
using ParticleSystemUtility;
using System.Collections;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Transform respawnPoint;
    [SerializeField] GameObject portalParticles;
    [SerializeField] Vector3 fallPortalOffset;
    [SerializeField] Vector3 spawnPortalOffset;
    [SerializeField] float portalDuration;
    [SerializeField] float startTpDelay;
    [SerializeField] float spawnDelay;
    [SerializeField] int pointAddedAtTeleportation = -5;

    void OnTriggerEnter(Collider other)
    {
        PlayerSystems.PlayerBase.Player player = other.GetComponent<PlayerSystems.PlayerBase.Player>();
        if (player)
        {
            gameManager.AddScore(pointAddedAtTeleportation); // in front of player respawn
            StartTeleportDelay(player);
        }
        else if(other.GetComponent<IDestructible>() != null)
        {
            Destroy(other.gameObject);
        }
    }

    void StartTeleportDelay(PlayerSystems.PlayerBase.Player player)
    {
        Vector3 spawnPoint = player.transform.position + fallPortalOffset;
        ParticleInstanceManager portal = Instantiate(portalParticles, spawnPoint, Quaternion.identity).GetComponent<ParticleInstanceManager>();
        StartCoroutine(DestroyPortalDelay(portal));

        spawnPoint = respawnPoint.position + spawnPortalOffset;
        ParticleInstanceManager portal2 = Instantiate(portalParticles, spawnPoint, Quaternion.identity).GetComponent<ParticleInstanceManager>();
        StartCoroutine(DestroyPortalDelay(portal2));
        
        player.ClearPlayerStamina();
        player.SetPosition(respawnPoint.position);
    }

    IEnumerator DestroyPortalDelay(ParticleInstanceManager portal)
    {
        yield return new WaitForSeconds(portalDuration);
        portal.Stop(false);
    }
}
