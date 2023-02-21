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
    bool m_LerpRight = false;
    bool m_LerpLeft = false;
    private float m_padding;
    private Animator animator;

    public int Pointerindex;


    // Start is called before the first frame update
    void Start()
    {
      

        animator = container.GetComponent<Animator>();

        ReloadUI();
        //Items = new List<RectTransform>();
        /*
        for (int i = 0; i < Items.Count; i++)
        {
            Items.Add(container.GetChild(i).GetComponent<RectTransform>());
        }
        */
      

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

        if (Input.GetKeyDown(KeyCode.V) && InventoryScript.instance.recipe.Count < 3)
        {
            SelectIngredient();
        }


    }


    void SelectIngredient()
    {
        bool increment = false;
        if (InventoryScript.instance.projectilesData[Pointerindex] == InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 1])
            increment = true;


        InventoryScript.instance.recipe.Add(InventoryScript.instance.projectilesData[Pointerindex]);



        InventoryScript.instance.projectilesData.RemoveAt(Pointerindex);

        if (increment)
            IncrementPointor(1);

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
            Items[0].GetComponentInChildren<Image>().sprite = Defaultsprite;
            Items[1].GetComponentInChildren<Image>().sprite = Defaultsprite;
            Items[2].GetComponentInChildren<Image>().sprite = Defaultsprite;
            Items[3].GetComponentInChildren<Image>().sprite = Defaultsprite;
            Items[4].GetComponentInChildren<Image>().sprite = Defaultsprite;
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
                        Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                    }
                    else if (Pointerindex == 1)
                    {
                        Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                    }
                    else if (Pointerindex == 2)
                    {
                        Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                    }

                    break;
                case 3:
                    if (Pointerindex == 0)
                    {
                        Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                        Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                    }
                    else if (Pointerindex == 1)
                    {
                        Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                        Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                        Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                    }
                    else if (Pointerindex == 2)
                    {
                        Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[3].inventorySprite;
                        Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                        Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[3].inventorySprite;
                        Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                    }
         
                       
                        break;

                default:
                        if (Pointerindex == 0)
                        {
                            Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 2].inventorySprite;
                            Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 1].inventorySprite;
                            Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                            Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                        }
                        else if (Pointerindex == 1)
                        {
                            Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 1].inventorySprite;
                            Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                            Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[2].inventorySprite;
                            Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[3].inventorySprite;
                        }
                        else if (Pointerindex == InventoryScript.instance.projectilesData.Count-2)
                        {
                            Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 4].inventorySprite;
                            Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 3].inventorySprite;
                            Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 1].inventorySprite;
                            Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                        }
                        else if (Pointerindex == InventoryScript.instance.projectilesData.Count - 1)
                        {
                            Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 3].inventorySprite;
                            Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[InventoryScript.instance.projectilesData.Count - 2].inventorySprite;
                            Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[0].inventorySprite;
                            Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[1].inventorySprite;
                        }
                        else
                        {
                            Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[Pointerindex - 2].inventorySprite;
                            Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[Pointerindex - 1].inventorySprite;
                            Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[Pointerindex + 1].inventorySprite;
                            Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[Pointerindex + 2].inventorySprite;
                        }

                    break;
            }
            
        }
           
    }
           
             
}

    

    


