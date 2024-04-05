using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecipeSystem.Core
{
    public class RecipeUI : MonoBehaviour
    {
        [SerializeField] GameObject UIList;
        public static RecipeUI Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        public void AddNewCard(GameRecipe recipe)
        {
            recipe.transform.SetParent(UIList.transform);
        }

        public void RemoveCard(GameRecipe recipe)
        { StartCoroutine(_RemoveCard(recipe)); }
        IEnumerator _RemoveCard(GameRecipe recipe)
        {
            // --- animation et trucs sympa ici
            yield return null;
            Destroy(recipe.gameObject);
        }
    }
}
