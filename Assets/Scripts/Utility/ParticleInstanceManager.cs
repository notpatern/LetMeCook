using UnityEngine;

public class ParticleInstanceManager : MonoBehaviour
{
    [SerializeField] ParticleSystem[] m_Particles;

    public void Emit(int amount)
    {
        foreach (var particle in m_Particles)
        {
            particle.Emit(amount);
        }
    }

    public void Stop()
    {
        foreach (var particle in m_Particles)
        {
            particle.Stop();
            particle.Clear();
        }
    }
}
