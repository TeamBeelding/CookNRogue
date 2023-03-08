using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeoScrolling : MonoBehaviour
{
    public RectTransform container;
    public Sprite Defaultsprite;
    public List <RectTransform> Items;
    public List<Image> ItemImages;
    private Animator animator;
    InventoryScript instance;

    public int Pointerindex;


    // Start is called before the first frame update
    void Start()
    {

        instance = InventoryScript._instance;
        animator = container.GetComponent<Animator>();

        
        
        ItemImages = new List<Image>();

        for (int i = 0; i < Items.Count; i++)
        {
            ItemImages.Add(Items[i].GetComponentInChildren<Image>());
        }

        ReloadUI();

    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("switchRight") && !animator.GetCurrentAnimatorStateInfo(0).IsName("switchLeft"))
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
               
                animator.Play("switchRight");
                IncrementPointor(1);

            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
              
                animator.Play("switchLeft");
                IncrementPointor(-1);
     
            }
        }

        if (Input.GetKeyDown(KeyCode.V) && instance.recipe.Count < 3 && instance.projectilesData.Count>0)
        {
            SelectIngredient();
        }
    }


    void SelectIngredient()
    {
        /*
        bool increment = false;
        if (InventoryScript.instance.projectilesData[Pointerindex] == InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 1])
            increment = true;
        */

        instance.recipe.Add(instance.projectilesData[Pointerindex]);



        instance.projectilesData.RemoveAt(Pointerindex);

        /*
        if (increment)
            IncrementPointor(1);
        */
        ReloadUI();
    }

    private void IncrementPointor(int v)
    {
        Pointerindex += v;
        if (Pointerindex < 0)
        {
            Pointerindex = instance.projectilesData.Count - 1;
        }
        if(Pointerindex> instance.projectilesData.Count - 1)
        {
            Pointerindex = 0;
        }
    }

    public void ReloadUI()
    {
        if (instance.projectilesData.Count == 0)
        {
            /*
            ItemImages[0].sprite = Defaultsprite;
            ItemImages[1].sprite = Defaultsprite;
            ItemImages[2].sprite = Defaultsprite;
            ItemImages[3].sprite = Defaultsprite;
            ItemImages[4].sprite = Defaultsprite;
            */

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
                ItemImages[index].sprite = instance.projectilesData[itemIndex].inventorySprite;
            }
            return;

            /*
            int secondItemIndex = DecrementIndex(Pointerindex);
            int firstItemIndex = DecrementIndex(secondItemIndex);
            int fourthItemIndex = IncrementIndex(Pointerindex);
            int fifthItemindex = IncrementIndex(fourthItemIndex);

            ItemImages[0].sprite = instance.projectilesData[firstItemIndex].inventorySprite;
            ItemImages[1].sprite = instance.projectilesData[secondItemIndex].inventorySprite;
            ItemImages[2].sprite = instance.projectilesData[Pointerindex].inventorySprite;
            ItemImages[3].sprite = instance.projectilesData[fourthItemIndex].inventorySprite;
            ItemImages[4].sprite = instance.projectilesData[fifthItemindex].inventorySprite;
            return;*/

            /*
            Items[2].GetComponentInChildren<Image>().sprite = instance.projectilesData[Pointerindex].inventorySprite;

            switch (instance.projectilesData.Count)
            {

                case 1:
                    break;
                case 2:
                    if (Pointerindex == 0)
                    {
                        ItemImages[0].sprite = instance.projectilesData[0].inventorySprite;
                        ItemImages[1].sprite = instance.projectilesData[1].inventorySprite;
                        ItemImages[3].sprite = instance.projectilesData[1].inventorySprite;
                        ItemImages[4].sprite = instance.projectilesData[0].inventorySprite;
                    }
                    else if (Pointerindex == 1)
                    {
                        ItemImages[0].sprite = instance.projectilesData[0].inventorySprite;
                        ItemImages[1].sprite = instance.projectilesData[1].inventorySprite;
                        ItemImages[3].sprite = instance.projectilesData[0].inventorySprite;
                        ItemImages[4].sprite = instance.projectilesData[0].inventorySprite;
                    }
                    else if (Pointerindex == 2)
                    {
                        ItemImages[0].sprite = instance.projectilesData[0].inventorySprite;
                        ItemImages[1].sprite = instance.projectilesData[1].inventorySprite;
                        ItemImages[3].sprite = instance.projectilesData[0].inventorySprite;
                        ItemImages[4].sprite = instance.projectilesData[1].inventorySprite;
                    }

                    break;
                case 3:
                    if (Pointerindex == 0)
                    {
                        ItemImages[0].sprite = instance.projectilesData[1].inventorySprite;
                        ItemImages[1].sprite = instance.projectilesData[2].inventorySprite;
                        ItemImages[3].sprite = instance.projectilesData[1].inventorySprite;
                        ItemImages[4].sprite = instance.projectilesData[2].inventorySprite;
                    }
                    else if (Pointerindex == 1)
                    {
                        ItemImages[0].sprite = instance.projectilesData[2].inventorySprite;
                        ItemImages[1].sprite = instance.projectilesData[0].inventorySprite;
                        ItemImages[3].sprite = instance.projectilesData[2].inventorySprite;
                        ItemImages[4].sprite = instance.projectilesData[1].inventorySprite;
                    }
                    else if (Pointerindex == 2)
                    {
                        ItemImages[0].sprite = instance.projectilesData[0].inventorySprite;
                        ItemImages[1].sprite = instance.projectilesData[1].inventorySprite;
                        ItemImages[3].sprite = instance.projectilesData[1].inventorySprite;
                        ItemImages[4].sprite = instance.projectilesData[2].inventorySprite;
                    }
         
                       
                        break;

                default:
                        if (Pointerindex == 0)
                        {
                        ItemImages[0].sprite = instance.projectilesData[instance.projectilesData.Count - 2].inventorySprite;
                        ItemImages[1].sprite = instance.projectilesData[instance.projectilesData.Count - 1].inventorySprite;
                        ItemImages[3].sprite = instance.projectilesData[1].inventorySprite;
                        ItemImages[4].sprite = instance.projectilesData[2].inventorySprite;
                        }
                        else if (Pointerindex == 1)
                        {
                        ItemImages[0].sprite = instance.projectilesData[instance.projectilesData.Count - 1].inventorySprite;
                        ItemImages[1].sprite = instance.projectilesData[0].inventorySprite;
                        ItemImages[3].sprite = instance.projectilesData[2].inventorySprite;
                        ItemImages[4].sprite = instance.projectilesData[3].inventorySprite;
                        }
                        else if (Pointerindex == instance.projectilesData.Count-2)
                        {
                        ItemImages[0].sprite = instance.projectilesData[instance.projectilesData.Count - 4].inventorySprite;
                        ItemImages[1].sprite = instance.projectilesData[instance.projectilesData.Count - 3].inventorySprite;
                        ItemImages[3].sprite = instance.projectilesData[instance.projectilesData.Count - 1].inventorySprite;
                        ItemImages[4].sprite = instance.projectilesData[0].inventorySprite;
                        }
                        else if (Pointerindex == instance.projectilesData.Count - 1)
                        {
                        ItemImages[0].sprite = instance.projectilesData[instance.projectilesData.Count - 3].inventorySprite;
                        ItemImages[1].sprite = instance.projectilesData[instance.projectilesData.Count - 2].inventorySprite;
                        ItemImages[3].sprite = instance.projectilesData[0].inventorySprite;
                        ItemImages[4].sprite = instance.projectilesData[1].inventorySprite;
                        }
                        else
                        {
                        ItemImages[0].sprite = instance.projectilesData[Pointerindex - 2].inventorySprite;
                        ItemImages[1].sprite = instance.projectilesData[Pointerindex - 1].inventorySprite;
                        ItemImages[3].sprite = instance.projectilesData[Pointerindex + 1].inventorySprite;
                        ItemImages[4].sprite = instance.projectilesData[Pointerindex + 2].inventorySprite;
                        }

                    break;
            }*/
        } 
    }
   
    private int IncrementIndex(int baseIndex, int value)
    {
        int projectilesNb = instance.projectilesData.Count;
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

    /*
    private int IncrementIndex(int index)
    {
        if (index >= instance.projectilesData.Count - 1)
        {
            return 0;
        }

        return index + 1;
    }

    private int DecrementIndex(int index)
    {
        if (index <= 0)
        {
            return instance.projectilesData.Count - 1;
        }

        return index - 1;
    }*/
}

    

    


