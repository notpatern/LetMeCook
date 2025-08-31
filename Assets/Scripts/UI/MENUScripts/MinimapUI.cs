using UnityEngine;

namespace UI.MENUScripts
{
    public class MinimapUI : MonoBehaviour
    {
        [SerializeField] RenderTexture _texture;
        bool state = false;

        public void MenuState(bool state) {
            this.state = state;
        }

        void Update()
        {
            if (state != gameObject.activeSelf)
            {
                gameObject.SetActive(state);
            }
        }
    }
}
