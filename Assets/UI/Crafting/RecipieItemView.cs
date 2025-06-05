using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System.Linq;

public class RecipeItemView : MonoBehaviour
{
    [SerializeField]
    //public Image resultIcon;
    public TextMeshProUGUI resultName;
    public TextMeshProUGUI ingredientsText;
    public Button craftButton;

    private Recipe recipe;
    private CraftingManager craftingManager;

    public void Awake()
    {
       
    }
    private void OnEnable()
    {
    }

    private void Start()
    {

    }

    public void Setup(Recipe recipe, CraftingManager manager)
    {
        this.recipe = recipe;
        craftingManager = manager;

        if (recipe == null || craftingManager == null)
        {
            Debug.LogError("Recipe or CraftingManager is not set up correctly.");
            return;
        }

        if (resultName == null || craftButton == null)
        {
            Debug.LogError("UI components are not assigned correctly.");
            return;
        }
        resultName.text = recipe.Name;
        if (recipe.Icon != null)
            craftButton.image.sprite = recipe.Icon;

        StringBuilder sb = new StringBuilder();
        foreach (var ing in recipe.ingredients)
        {
            sb.Append($"{ing.quantity}x {ing.item.name}, ");
        }
        ingredientsText.text = sb.ToString().TrimEnd(',', ' ');

        craftButton.onClick.AddListener(() =>
        {
            if (craftingManager.Craft(recipe))
            {
                Debug.Log("Crafted!");
            }
            else
            {
                Debug.Log("Not enough resources.");
            }
        });
    }
}
