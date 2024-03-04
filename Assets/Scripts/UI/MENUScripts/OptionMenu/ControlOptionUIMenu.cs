using UnityEngine;
using ControlOptions;
using UnityEngine.UI;
using Player.Input;

namespace UI.MENUScripts.Options
{
    public class ControlOptionsUIMenu : MonoBehaviour
    {
        [Header("Sensitivity")]
        [SerializeField] Slider mouseSensitivitySlider;

        [Header("Keybinds")]
        [SerializeField] KeybindsDataBase keybindsDataBase;
        [SerializeField] GameObject keybindsPrefab;
        [SerializeField] Transform keybindsTransform;

        void Start()
        {
            mouseSensitivitySlider.minValue = ControlOptionsManagement.s_Instance.GetSensitivityBounds().x;
            mouseSensitivitySlider.maxValue = ControlOptionsManagement.s_Instance.GetSensitivityBounds().y;

            mouseSensitivitySlider.value = ControlOptionsManagement.s_Instance.GetMouseSensitivity();
            mouseSensitivitySlider.onValueChanged.AddListener(OnSliderValueChange);

            LoadKeybinds();
        }

        void LoadKeybinds()
        {
            for(int i=0; i<keybindsDataBase.keybinds.Length; i++)
            {
                GameObject keybindsGo = Instantiate(keybindsPrefab, keybindsTransform);
                keybindsGo.GetComponent<InputActionDisplay>().LoadInput(keybindsDataBase.keybinds[i]);
            }
        }

        void OnSliderValueChange(float value)
        {
            ControlOptionsManagement.s_Instance.SetMouseSensitivity(value);
        }
    }
}