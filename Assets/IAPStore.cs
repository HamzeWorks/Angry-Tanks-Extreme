using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPStore : MonoBehaviour
{

    public GameObject restorePurchasteBTN;

    public string[] GoldProductIDs;
    public string[] GemProductIDs;



    // Start is called before the first frame update
    void Awake()
    {
        DisableRestorePurchaseBTN();
    }

    public void DisableRestorePurchaseBTN()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            restorePurchasteBTN.SetActive(false);
        }
    }

    // Update is called once per frame
    public void OnPurchaseComplete(Product product)
    {
        //Gold Purchase states
        if (product.definition.id == GoldProductIDs[0])
        {
            // Purchase Pack number 1
            ShopManager.Gold += 1000;
            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
            PlayerPrefs.Save();
            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();
        }
        else if (product.definition.id == GoldProductIDs[1])
        {
            // Purchase Pack number 2
            ShopManager.Gold += 10000;
            PlayerPrefs.SetInt("Gold", ShopManager.Gold);
            PlayerPrefs.Save();
            ShopManager.instance.goldtxt.text = ShopManager.Gold.ToString();
        }
        #region commented GoldPack 3 and 4
        /*  
           else if (product.definition.id == GoldProductIDs[2])
           {
               // Purchase Pack number 3

           }
           else if (product.definition.id == GoldProductIDs[3])
           {
               // Purchase Pack number 4

           }
           */
        #endregion

        //Gem Purchase States
        if (product.definition.id == GemProductIDs[0])
        {            
            // Purchase Pack number 1
            ShopManager.Gem += 100;
            PlayerPrefs.SetInt("Gem", ShopManager.Gem);
            PlayerPrefs.Save();
            ShopManager.instance.gemtxt.text = ShopManager.Gem.ToString();
            print("here");
        }
        else if (product.definition.id == GemProductIDs[1])
        {
            // Purchase Pack number 2
            ShopManager.Gem += 1000;
            PlayerPrefs.SetInt("Gem", ShopManager.Gem);
            PlayerPrefs.Save();
            ShopManager.instance.gemtxt.text = ShopManager.Gem.ToString();
        }
        #region commented GemPack 3 and 4

        /* 
          else if (product.definition.id == GemProductIDs[2])
          {
              // Purchase Pack number 3
          }
          else if (product.definition.id == GemProductIDs[3])
          {
              // Purchase Pack number 4
          }
          */
        #endregion
    }
    public void OnPurchaseFailure(Product product,PurchaseFailureReason reason)
    {
        Debug.Log("purchase of " + product.definition.id + " failed because of " + reason);
    }
}
