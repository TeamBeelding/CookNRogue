using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryScrolling : MonoBehaviour
{
    public RectTransform container;
    public Sprite Defaultsprite;
    public List <RectTransform> Items;
    private List<Image> ItemImages;
    private Animator animator;
    PlayerInventoryScript _inventoryScript;

    public int Pointerindex;


    // Start is called before the first frame update
    void Start()
    {
        _inventoryScript = PlayerInventoryScript.Instance;
        animator = container.GetComponent<Animator>();
       
        ItemImages = new List<Image>();

        for (int i = 0; i < Items.Count; i++)
        {
            ItemImages.Add(Items[i].GetComponentInChildren<Image>());
        }

        ReloadUI();
    }

    public void SwitchToLeftIngredient()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("switchRight") || animator.GetCurrentAnimatorStateInfo(0).IsName("switchLeft"))
            return;

        animator.Play("switchLeft");
        IncrementPointor(-1);
    }

    public void SwitchToRightIngredient()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("switchRight") || animator.GetCurrentAnimatorStateInfo(0).IsName("switchLeft"))
            return;

        animator.Play("switchRight");
        IncrementPointor(1);
    }


    public void SelectIngredient()
    {
        if (_inventoryScript.projectilesData.Count > 0)
        {
            _inventoryScript.recipe.Add(_inventoryScript.projectilesData[Pointerindex]);

            _inventoryScript.projectilesData.RemoveAt(Pointerindex);

            if (_inventoryScript.projectilesData.Count > 0)
            {
                IncrementPointor(-1);
            }

            ReloadUI();
        }
    }

    private void IncrementPointor(int v)
    {
        if(_inventoryScript.projectilesData.Count == 0)
        {
            Pointerindex = 0;
            return;
        }

        Pointerindex += v;
        if (Pointerindex < 0)
        {
            Pointerindex = _inventoryScript.projectilesData.Count - 1;
        }
        if(Pointerindex> _inventoryScript.projectilesData.Count - 1)
        {
            Pointerindex = 0;
        }
    }

    public void ReloadUI()
    {
        if (_inventoryScript.projectilesData.Count == 0)
        {
            for(int index = 0; index < ItemImages.Count; index++)
            {
                ItemImages[index].sprite = Defaultsprite;
            }

            return;           
        }
        else
        {
            int itemsNb = ItemImages.Count;
            int middleItemIndex = (itemsNb / 2) + 1;
            for (int index = 0; index < itemsNb; index++)
            {
                int itemRelativeIndex = index - middleItemIndex;
                int itemIndex = IncrementIndex(Pointerindex, itemRelativeIndex);
                ItemImages[index].sprite = _inventoryScript.projectilesData[itemIndex].inventorySprite;
            }

            return;
        } 
    }
   
    private int IncrementIndex(int baseIndex, int value)
    {
        int projectilesNb = _inventoryScript.projectilesData.Count;
        int newIndex = baseIndex + value;

        while (newIndex < 0)
        {
            newIndex = projectilesNb - newIndex;
        }

        while (newIndex >= projectilesNb)
        {
            newIndex -= projectilesNb;
        }

        return newIndex;
    }
}

    

    


