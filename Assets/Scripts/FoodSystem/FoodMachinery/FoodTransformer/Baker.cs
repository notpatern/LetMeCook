using Audio;
using FMOD.Studio;
using ItemLaunch;
using UnityEngine;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    public class Baker : FoodTransformer
    {
        EventInstance cookingSound;

        protected override void Awake()
        {
            base.Awake();
            cookingSound = AudioManager.s_Instance.Create3DInstance(AudioManager.s_Instance.m_AudioSoundData.m_BakerCooking, transform.position);
        }

        protected override void OnFoodCollected()
        {
            cookingSound.start();
            base.OnFoodCollected();

        }

        protected override void ReleaseFood()
        {
            GameObject newFood = Instantiate(collectedFoodData[0].bakedFood.prefab,
                launcher.StartPoint, Quaternion.identity);

            launcher.ThrowItem(newFood.GetComponent<LaunchableItem>());
            base.ReleaseFood();

            cookingSound.stop(STOP_MODE.ALLOWFADEOUT);
        }
        
        protected override bool CheckIfCanCook(FoodData foodData) => foodData.bakedFood != null;
    }
}
