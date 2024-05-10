using Audio;
using FMOD.Studio;
using ItemLaunch;
using UnityEngine;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    public class Baker : FoodTransformer
    {
        protected override void OnFoodCollected()
        {
            CheckForPlayingCookingSound(AudioManager.s_Instance.m_AudioSoundData.m_BakerCooking);

            base.OnFoodCollected();
        }

        protected override void ReleaseFood()
        {
            cookingSound.stop(STOP_MODE.ALLOWFADEOUT);

            GameObject newFood = Instantiate(collectedFoodData[0].bakedFood.prefab,
                launcher.StartPoint, Quaternion.identity);

            launcher.ThrowItem(newFood.GetComponent<LaunchableItem>());
            base.ReleaseFood();
        }
        
        protected override bool CheckIfCanCook(FoodData foodData) => foodData.bakedFood != null;
    }
}
