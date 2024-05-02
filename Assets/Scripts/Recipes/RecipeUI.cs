using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecipeSystem.Core
{
    public class RecipeUI : MonoBehaviour
    {
        [SerializeField] GameObject UIList;
        [SerializeField] GameObject recipeCard;

        List<RecipeCard> activeRecipeCards = new List<RecipeCard>();

        public void AddNewCard(GameRecipe recipe, bool isLastRecipe)
        {
            GameObject newCard = Instantiate(recipeCard);
            RecipeCard card = newCard.GetComponent<RecipeCard>();
            card.Init(recipe, UIList.transform, isLastRecipe);
            activeRecipeCards.Add(card);
        }

        public void RemoveCard(GameRecipe recipe)
        { 
            StartCoroutine(_RemoveCard(recipe)); 
        }
        IEnumerator _RemoveCard(GameRecipe recipe)
        {
            // --- animation et trucs sympa ici
            yield return null;

            // Remove card from list
            RecipeCard cardFound = null;
            foreach (var card in activeRecipeCards)
            {
                if (card.gameRecipe.recipe == recipe.recipe)
                {
                    cardFound = card;
                    break;
                }
            }

            if (!cardFound)
            {
                Debug.LogError($"No card found for recipe {recipe.recipe.nametag}.");
                yield break;
            }

            activeRecipeCards.Remove(cardFound);
            Destroy(cardFound.gameObject);
        }
    }
}
