using UnityEngine;

class upgradesystem : MonoBehaviour
{
    static public upgradesystem instance;


    [SerializeField]
    public Tankz[] tanks;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}

[System.Serializable]
public class Tankz
{
    public string NameOfTank;
    [SerializeField]
    public statsoftanks[] Items;
    public int[] missilelvl;

}


[System.Serializable]
public class statsoftanks
{
   public int level;
   public int cost;
   public int power;
   public int health;
   public float speed;
   

}


