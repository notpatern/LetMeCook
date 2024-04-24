using ItemLaunch;
using UnityEngine;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    public class Purifier : FoodTransformer
    {
        float m_Timer = 0f;
        float m_LerpDelta = 0f;
        int m_LerpDirection = 1;

        [SerializeField] Transform m_AnnimationStartPos;
        [SerializeField] Transform m_AnnimationEndPos;

        protected override void Start()
        {
            cookingFoodPos.position = m_AnnimationStartPos.position;
        }

        protected override void ReleaseFood()
        {
            GameObject newFood = Instantiate(collectedFoodData[0].purifiedFood.prefab,
                launcher.StartPoint, Quaternion.identity);

            launcher.ThrowItem(newFood.GetComponent<LaunchableItem>());
        
            base.ReleaseFood();
        }

        protected override void Update()
        {
            if(_cooking)
            {
                m_Timer += Time.deltaTime;
                m_LerpDelta += Time.deltaTime / (cookingTime / 2f) * m_LerpDirection;

                UpdateMovement();

                if(m_Timer >= cookingTime) 
                {
                    m_Timer = 0f;
                    m_LerpDelta = 0f;
                }
            }
        }

        void UpdateMovement()
        {
            if(Mathf.Abs(m_LerpDelta) >= 1)
            {
                m_LerpDirection *= -1;
            }

            cookingFoodPos.position = PerformVectorLerp(m_AnnimationStartPos.position, m_AnnimationEndPos.position, m_LerpDirection);
        }

        Vector3 PerformVectorLerp(Vector3 from, Vector3 to, float t)
        {
            return from + (to - from) * Mathf.Clamp01(t);
        }

        protected override bool CheckIfCanCook(FoodData foodData) => foodData.purifiedFood != null;
        
    }
}
