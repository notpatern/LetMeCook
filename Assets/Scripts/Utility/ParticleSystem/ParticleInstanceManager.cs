using UnityEngine;

namespace ParticleSystemUtility
{
    public class ParticleInstanceManager : MonoBehaviour
    {
        [SerializeField] ParticleSystem[] m_Particles;
        [SerializeField] bool isPermanent = false;

        private void Update()
        {
            DestructionCheck();
        }

        void DestructionCheck()
        {
            if (!isPermanent)
            {
                foreach (var particle in m_Particles)
                {
                    if (particle && particle.isPlaying)
                    {
                        return;
                    }
                }

                Destroy(gameObject);
            }
        }

        public void Emit(int amount)
        {
            foreach (var particle in m_Particles)
            {
                particle.Emit(amount);
            }
        }

        public void Stop(bool clear)
        {
            foreach (var particle in m_Particles)
            {
                particle.Stop();

                if (clear)
                {
                    particle.Clear();
                }
            }
        }
    }

}