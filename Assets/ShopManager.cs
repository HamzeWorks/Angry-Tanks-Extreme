using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    public GameObject shoppanel;
    static public int Gem = 5;
    static public int Gold = 1000;

    public Text gemtxt, goldtxt;

   static public ShopManager instance;
    public GameObject tanksContainer;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("GiveCurrency"))
        {
            Gold = PlayerPrefs.GetInt("Gold");
            Gem = PlayerPrefs.GetInt("Gem");
            print(PlayerPrefs.GetString("GiveCurrency"));
            gemtxt.text = Gem.ToString();
            goldtxt.text = Gold.ToString();
            
        }
        else
        {
            PlayerPrefs.SetInt("Gem", 0);
            PlayerPrefs.SetInt("Gold", 250);
            Gold = PlayerPrefs.GetInt("Gold");
            Gem = PlayerPrefs.GetInt("Gem");
            gemtxt.text = Gem.ToString();
            goldtxt.text = Gold.ToString();
            PlayerPrefs.SetString("GiveCurrency", "Done");
            PlayerPrefs.Save();
            print(PlayerPrefs.GetString("GiveCurrency"));
        }



    }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;

        }
       //PlayerPrefs.DeleteAll();

    }


   

    // Update is called once per frame
    public void OpenGoldShop()
    {
        shoppanel.SetActive(true);
        tanksContainer.SetActive(false);
        GetComponent<AudioSource>().Play();

    }
    public void showgoldgemtext()
    {
        gemtxt.text = Gem.ToString();
        goldtxt.text = Gold.ToString();
    }
   

    // Update is called once per frame
    public void CloseGoldShop()
    {
        shoppanel.SetActive(false);
        tanksContainer.SetActive(true);

        GetComponent<AudioSource>().Play();

    }

}
