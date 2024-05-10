using Audio;
using FMOD.Studio;
using ItemLaunch;
using UnityEngine;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    public class Chopper : FoodTransformer
    {
        float m_Timer = 0f;
        float m_LerpDelta = 0f;

        [SerializeField] Transform m_AnnimationStartPos;
        [SerializeField] Transform m_AnnimationEndPos;
        [SerializeField] Transform m_BladeTr;

        EventInstance cookingSound;

        protected override void Awake()
        {
            base.Awake();
            cookingSound = AudioManager.s_Instance.Create3DInstance(AudioManager.s_Instance.m_AudioSoundData.m_ChopperCooking, transform.position);
        }

        protected override void OnFoodCollected()
        {
            base.OnFoodCollected();
            cookingSound.start();
        }

        protected override void ReleaseFood()
        {
            GameObject newFood = Instantiate(collectedFoodData[0].choppedFood.prefab,
                launcher.StartPoint, Quaternion.identity);

            launcher.ThrowItem(newFood.GetComponent<LaunchableItem>());
            m_LerpDelta = 0f;

            base.ReleaseFood();
            cookingSound.stop(STOP_MODE.ALLOWFADEOUT);
        }

        protected override void Update()
        {
            base.Update();

            if (_cooking)
            {
                m_Timer += Time.deltaTime;
                m_LerpDelta += Time.deltaTime / cookingTime;

                UpdateMovement();

                if (m_Timer >= cookingTime)
                {
                    m_Timer = 0f;
                    m_LerpDelta = 0f;
                }
            }
        }

        void ResetPosition()
        {
            m_BladeTr.position = m_AnnimationStartPos.position;
        }

        void UpdateMovement()
        {
            m_BladeTr.position = PerformVectorLerp(m_AnnimationStartPos.position, m_AnnimationEndPos.position, m_LerpDelta);
        }

        protected override bool CheckIfCanCook(FoodData foodData) => foodData.choppedFood != null;
    }
}
