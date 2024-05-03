using ItemLaunch;
using UnityEngine;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    public class Chopper : FoodTransformer
    {
        float m_Timer = 0f;
        float m_LerpDelta = 0f;
        int m_LerpDirection = 1;

        [SerializeField] Transform m_AnnimationStartPos;
        [SerializeField] Transform m_AnnimationEndPos;
        [SerializeField] Transform m_BladeTr;

        protected override void ReleaseFood()
        {
            GameObject newFood = Instantiate(collectedFoodData[0].choppedFood.prefab,
                launcher.StartPoint, Quaternion.identity);

            launcher.ThrowItem(newFood.GetComponent<LaunchableItem>());
            m_LerpDirection = 0;
            m_LerpDelta = 0f;
            m_LerpDirection = 1;

            base.ReleaseFood();
        }

        protected override void Update()
        {
            base.Update();

            if (_cooking)
            {
                m_Timer += Time.deltaTime;
                m_LerpDelta += Time.deltaTime / (cookingTime / 2f) * m_LerpDirection;

                UpdateMovement();

                if (m_Timer >= cookingTime)
                {
                    m_Timer = 0f;
                    m_LerpDelta = 0f;
                }
            }
        }

        void UpdateMovement()
        {
            if (Mathf.Abs(m_LerpDelta) >= 1)
            {
                m_LerpDirection *= -1;
            }

            m_BladeTr.position = PerformVectorLerp(m_AnnimationStartPos.position, m_AnnimationEndPos.position, m_LerpDelta);
        }

        protected override bool CheckIfCanCook(FoodData foodData) => foodData.choppedFood != null;
    }
}
