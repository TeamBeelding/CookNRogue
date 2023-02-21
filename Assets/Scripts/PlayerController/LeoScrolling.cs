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

    public int Pointerindex0;
    public int Pointerindex1;
    public int Pointerindex2;
    public int Pointerindex3;
    public int Pointerindex4;

    // Start is called before the first frame update
    void Start()
    {
        InitPointors();

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
                IncrementPointors(1);
                animator.Play("switchRight");
           

            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                IncrementPointors(-1);
                animator.Play("switchLeft");
     
            }
        }

        if (Input.GetKeyDown(KeyCode.V) && InventoryScript.instance.recipe.Count < 3)
        {
            SelectIngredient();
        }


    }


    void SelectIngredient()
    {
        InventoryScript.instance.recipe.Add(InventoryScript.instance.projectilesData[Pointerindex2]);
        InventoryScript.instance.projectilesData.RemoveAt(Pointerindex2);
        ReloadUI();
    }

    public void ReloadUI()
    {
        if (InventoryScript.instance.projectilesData.Count > 0)
        {
           
            if (Pointerindex0 < InventoryScript.instance.projectilesData.Count)
            {
              
                Items[0].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[Pointerindex0].inventorySprite;
            }
            else
            {
                Items[0].GetComponentInChildren<Image>().sprite = Defaultsprite;
            }

            if (Pointerindex1 < InventoryScript.instance.projectilesData.Count)
            {
                Items[1].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[Pointerindex1].inventorySprite;
               
            }
            else
            {
                Items[1].GetComponentInChildren<Image>().sprite = Defaultsprite;
            }
            

            if (Pointerindex2 < InventoryScript.instance.projectilesData.Count)
            {
                Items[2].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[Pointerindex2].inventorySprite;
                
            }
            else
            {
                Items[2].GetComponentInChildren<Image>().sprite = Defaultsprite;
            }

            if (Pointerindex3 < InventoryScript.instance.projectilesData.Count)
            {
                Items[3].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[Pointerindex3].inventorySprite;
            }
            else
            {
                Items[3].GetComponentInChildren<Image>().sprite = Defaultsprite;
            }

            if (Pointerindex4 < InventoryScript.instance.projectilesData.Count)
            {
                Items[4].GetComponentInChildren<Image>().sprite = InventoryScript.instance.projectilesData[Pointerindex4].inventorySprite;
            }
            else
            {
                Items[4].GetComponentInChildren<Image>().sprite = Defaultsprite;
            }
        }
        else
        {
            Items[0].GetComponentInChildren<Image>().sprite = Defaultsprite;
            Items[1].GetComponentInChildren<Image>().sprite = Defaultsprite;
            Items[2].GetComponentInChildren<Image>().sprite = Defaultsprite;
            Items[3].GetComponentInChildren<Image>().sprite = Defaultsprite;
            Items[4].GetComponentInChildren<Image>().sprite = Defaultsprite;
        }
           
             
    }

    void InitPointors()
    {
        Pointerindex0 = 3;
        Pointerindex1 = 4;
        Pointerindex2 = 0;
        Pointerindex3 = 1;
        Pointerindex4 = 2;
    }

    void IncrementPointors(int factor)
    {
        Pointerindex0 += factor;
        if(Pointerindex0 > 4)
        {
            Pointerindex0 = 0;
        }
        else if(Pointerindex0 < 0)
        {
            Pointerindex0 = 4;
        }

        Pointerindex1 += factor;
        if (Pointerindex1 > 4)
        {
            Pointerindex1 = 0;
        }
        else if (Pointerindex1 < 0)
        {
            Pointerindex1 = 4;
        }

        Pointerindex2 += factor;
        if (Pointerindex2 > 4)
        {
            Pointerindex2 = 0;
        }
        else if (Pointerindex2 < 0)
        {
            Pointerindex2 = 4;
        }

        Pointerindex3 += factor;
        if (Pointerindex3 > 4)
        {
            Pointerindex3 = 0;
        }
        else if (Pointerindex3 < 0)
        {
            Pointerindex3 = 4;
        }

        Pointerindex4 += factor;
        if (Pointerindex4 > 4)
        {
            Pointerindex4 = 0;
        }
        else if (Pointerindex4 < 0)
        {
            Pointerindex4 = 4;
        }
    }

}
