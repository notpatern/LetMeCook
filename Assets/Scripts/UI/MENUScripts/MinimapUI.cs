using UnityEngine;

namespace UI.MENUScripts
{
    [System.Serializable]
    public class MinimapUI
    {
        [SerializeField] GameObject _texture;
        bool state = false;

        public void MinimapState(bool state) {
            this.state = state;
        }

        public void Update()
        {
            if (state != _texture.activeSelf)
            {
                _texture.SetActive(state);
            }
        }
    }
}
