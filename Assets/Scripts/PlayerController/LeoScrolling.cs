using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class LeoScrolling : MonoBehaviour
{
    public RectTransform container;
    public Sprite Defaultsprite;
    public List <RectTransform> Items;
    public List<Image> ItemImages;
    bool m_LerpRight = false;
    bool m_LerpLeft = false;
    private float m_padding;
    private Animator animator;

    public int Pointerindex;


    // Start is called before the first frame update
    void Start()
    {
      

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

        if (Input.GetKeyDown(KeyCode.V) && InventoryScript.instance.recipe.Count < 3 && InventoryScript.instance.projectilesData.Count>0)
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

        InventoryScript.instance.recipe.Add(InventoryScript.instance.projectilesData[Pointerindex]);



        InventoryScript.instance.projectilesData.RemoveAt(Pointerindex);

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
            Pointerindex = InventoryScript.instance.projectilesData.Count - 1;
        }
        if(Pointerindex> InventoryScript.instance.projectilesData.Count - 1)
        {
            Pointerindex = 0;
        }
    }

    public void ReloadUI()
    {
        if (InventoryScript.instance.projectilesData.Count == 0)
        {
            ItemImages[0].sprite = Defaultsprite;
            ItemImages[1].sprite = Defaultsprite;
            ItemImages[2].sprite = Defaultsprite;
            ItemImages[3].sprite = Defaultsprite;
            ItemImages[4].sprite = Defaultsprite;
            return;
            
        }
        else
        {
            Items[2].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[Pointerindex].inventorySprite;

            switch (InventoryScript.instance.projectilesData.Count)
            {

                case 1:
                    break;
                case 2:
                    if (Pointerindex == 0)
                    {
                        ItemImages[0].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        ItemImages[1].sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        ItemImages[3].sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        ItemImages[4].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                    }
                    else if (Pointerindex == 1)
                    {
                        ItemImages[0].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        ItemImages[1].sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        ItemImages[3].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        ItemImages[4].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                    }
                    else if (Pointerindex == 2)
                    {
                        ItemImages[0].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        ItemImages[1].sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        ItemImages[3].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        ItemImages[4].sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                    }

                    break;
                case 3:
                    if (Pointerindex == 0)
                    {
                        ItemImages[0].sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        ItemImages[1].sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                        ItemImages[3].sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        ItemImages[4].sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                    }
                    else if (Pointerindex == 1)
                    {
                        ItemImages[0].sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                        ItemImages[1].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        ItemImages[3].sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                        ItemImages[4].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                    }
                    else if (Pointerindex == 2)
                    {
                        ItemImages[0].sprite = InventoryScript.instance.projectilesData[3].inventorySprite;
                        ItemImages[1].sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                        ItemImages[3].sprite = InventoryScript.instance.projectilesData[3].inventorySprite;
                        ItemImages[4].sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                    }
         
                       
                        break;

                default:
                        if (Pointerindex == 0)
                        {
                        ItemImages[0].sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 2].inventorySprite;
                        ItemImages[1].sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 1].inventorySprite;
                        ItemImages[3].sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        ItemImages[4].sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                        }
                        else if (Pointerindex == 1)
                        {
                        ItemImages[0].sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 1].inventorySprite;
                        ItemImages[1].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        ItemImages[3].sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                        ItemImages[4].sprite = InventoryScript.instance.projectilesData[3].inventorySprite;
                        }
                        else if (Pointerindex == InventoryScript.instance.projectilesData.Count-2)
                        {
                        ItemImages[0].sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 4].inventorySprite;
                        ItemImages[1].sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 3].inventorySprite;
                        ItemImages[3].sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 1].inventorySprite;
                        ItemImages[4].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        }
                        else if (Pointerindex == InventoryScript.instance.projectilesData.Count - 1)
                        {
                        ItemImages[0].sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 3].inventorySprite;
                        ItemImages[1].sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 2].inventorySprite;
                        ItemImages[3].sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        ItemImages[4].sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        }
                        else
                        {
                        ItemImages[0].sprite = InventoryScript.instance.projectilesData[Pointerindex - 2].inventorySprite;
                        ItemImages[1].sprite = InventoryScript.instance.projectilesData[Pointerindex - 1].inventorySprite;
                        ItemImages[3].sprite = InventoryScript.instance.projectilesData[Pointerindex + 1].inventorySprite;
                        ItemImages[4].sprite = InventoryScript.instance.projectilesData[Pointerindex + 2].inventorySprite;
                        }

                    break;
            }
            
        }
           
    }
           
             
}

    

    


