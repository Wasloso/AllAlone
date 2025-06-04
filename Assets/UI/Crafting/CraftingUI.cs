using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingUI : MonoBehaviour
{
    public Button toggleButton;
    public GameObject recipeListPanel;
    public RectTransform contentParent;
    public GameObject recipeItemPrefab;

    private List<Reciepie> allRecipes;
    public CraftingManager craftingManager;

    private bool isOpen = false;

    void Start()
    {
        craftingManager = GetComponent<CraftingManager>();
        allRecipes = craftingManager.reciepes;
        toggleButton.onClick.AddListener(ToggleList);
        PopulateRecipes();
        recipeListPanel.SetActive(false);
        Debug.Log(contentParent.name + " has " + contentParent.childCount + " children at start.");
    }

    void ToggleList()
    {

        isOpen = !isOpen;
        recipeListPanel.SetActive(isOpen);
    }

    void PopulateRecipes()
    {
        //contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x, allRecipes.Count * 160);
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var recipe in allRecipes)
        {
            
            var obj = Instantiate(recipeItemPrefab,contentParent);
            Debug.Log("Instantiated recipe item prefab: " + obj.name + " at " + obj.transform.parent.name);
            var rectTransform = obj.GetComponent<RectTransform>();
            var view = obj.GetComponent<RecipeItemView>();
            view.Setup(recipe, craftingManager);
        }
    }
}
