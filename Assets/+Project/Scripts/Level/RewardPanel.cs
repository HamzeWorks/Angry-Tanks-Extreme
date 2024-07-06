using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using TankStars.Menu;

namespace TankStars.Level
{
    public class RewardPanel : MonoBehaviour
    {
        [SerializeField] GameObject rewardPanelBase;
        [SerializeField] Transform rewardPanel;
        [SerializeField] TextMeshProUGUI playerStateText;
        [SerializeField] Button backToMenuButton;
        [SerializeField] Button nextLevelBtn;
        [SerializeField] Sprite winPanelSprite;
        [SerializeField] Sprite losePanelSprite;
        [SerializeField]
        Sprite victory, defeat;
        public Image title;
        [SerializeField]
        Image Tank;

        public Sprite[] tank_sprites;

        [SerializeField]
        GameObject All_Stars;
        [SerializeField]
        GameObject[] Stars_Arr;

        [SerializeField]
        Slider PlayerHP;

        [SerializeField]
        Text retry_OR_next;

        [SerializeField]
        Text Score;

        string cur_scoretxt;
        string full_scoretxt;
        float scorekeeper;

        public GameObject victorySound,BG_Music;

        private void Awake()
        {
            BG_Music.SetActive(true);

            backToMenuButton.onClick.AddListener(() =>
            {
                GameManager.LoadLevel(SceneEnum.MainMenu);
            });
            nextLevelBtn.onClick.AddListener(() =>
            {
                Application.LoadLevel(Application.loadedLevel);
            });
            GameplayManager.instance.onFinish += NetworkManager_onFinishGame;
          //  nextLevelBtn.gameObject.SetActive(false);
        }

        private void NetworkManager_onFinishGame(Tank tank , bool win)
        {
            
          //  rewardPanel.DOPunchScale(new Vector3(.2f, -.2f), 1f, 3);
            Debug.Log("Unlocked Level Index = " + GameManager.unlockedLevelIndex);
            if (win)
            {
                transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = winPanelSprite;
                //TODO: fix this
                GameManager.instance._coin += 100;
                Debug.Log("GameManager.unlockedLevelIndex + 2" + (GameManager.unlockedLevelIndex + 2));
                Debug.Log("UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                Debug.Log("GameManager.unlockedLevelIndex + 1" +( GameManager.unlockedLevelIndex + 1));
                Debug.Log("UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 2" + (UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 2));
                scorekeeper = tank.currentHealth + tank.maxHealth * 10;
                StartCoroutine(TypeWriter());


                int BI = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                //TODO: hardCode
                if (GameManager.unlockedLevelIndex + 2 == BI)
                {
                    if (GameManager.unlockedLevelIndex + 1 < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 2)
                    {
                        GameManager.unlockedLevelIndex++;

                        nextLevelBtn.gameObject.SetActive(true);
                        nextLevelBtn.onClick.AddListener(() =>
                        {
                            GameManager.instance.SetGameMode(SceneEnum.Level_AI, GameManager.unlockedLevelIndex);
                        });

                    }
                }
                else if(BI>=3 && BI < GameManager.unlockedLevelIndex + 2)
                {
                    nextLevelBtn.gameObject.SetActive(true);
                    nextLevelBtn.onClick.AddListener(() =>
                    {
                        GameManager.instance.SetGameMode(SceneEnum.Level_AI, BI+1);
                    });

                }

                switch (PlayerPrefs.GetString("tankNAME"))
                {
                    case "tank_yellow(Clone)":
                        Tank.sprite = tank_sprites[0];
                        break;
                    case "tank_green1(Clone)":
                        Tank.sprite = tank_sprites[1];
                        break;
                    case "tank_green2(Clone)":
                        Tank.sprite = tank_sprites[2];
                        break;
                    case "tank_blue(Clone)":
                        Tank.sprite = tank_sprites[3];
                        break;
                }    
              
                playerStateText.text = "You Win";
                playerStateText.color = Color.green;
                PlayerPrefs.SetInt("WinCount",PlayerPrefs.GetInt("WinCount",0)+1);
                StartCoroutine(WinPanelComes());
            }
            else
            {
                transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = losePanelSprite ;
                playerStateText.text = "You Lose!";
                playerStateText.color = Color.red;
                PlayerPrefs.SetInt("LoseCount", PlayerPrefs.GetInt("LoseCount", 0) + 1);
                title.sprite = defeat;
                All_Stars.SetActive(false);
                retry_OR_next.text = "RETRY";
                Score.transform.gameObject.SetActive(false);
                switch (PlayerPrefs.GetString("tankNAME"))               
                {
                    case "tank_yellow(Clone)":
                        Tank.sprite = tank_sprites[0];
                        break;
                    case "tank_green1(Clone)":
                        Tank.sprite = tank_sprites[1];
                        break;
                    case "tank_green2(Clone)":
                        Tank.sprite = tank_sprites[2];
                        break;
                    case "tank_blue(Clone)":
                        Tank.sprite = tank_sprites[3];
                        break;
                }
                StartCoroutine(LosePanelComes());
            }
        }
        private void OnEnable()
        {
            All_Stars.SetActive(false);
          /*  for (int i = 0; i < 3; i++)
            {
                Stars_Arr[i].SetActive(false);
            }
            */
        }
        IEnumerator TypeWriter()
        {
            full_scoretxt = scorekeeper.ToString();
            for(int i = 0; i < full_scoretxt.Length; i++)
            {
                
                cur_scoretxt = full_scoretxt.Substring(0, i);
                Score.text = full_scoretxt;
                yield return new WaitForSeconds(0.1f);

            }
            
        }
        IEnumerator WinPanelComes()
        {
            BG_Music.SetActive(false);
            All_Stars.SetActive(true);

            yield return new WaitForSeconds(2.5f);
            
            rewardPanelBase.SetActive(true);
            title.sprite = victory;
            Instantiate(victorySound, transform);
            All_Stars.SetActive(true);
            if (PlayerHP.value >= 0.75f)
            {
                for (int i = 0; i < 3; i++)
                {
                    Stars_Arr[i].SetActive(true);
                }
            }
            else if (PlayerHP.value < 75 && PlayerHP.value >= 45)
            {
                for (int i = 0; i < 2; i++)
                {
                    Stars_Arr[i].SetActive(true);
                }
            }
            else if (PlayerHP.value < 45)
            {
                for (int i = 0; i < 1; i++)
                {
                    Stars_Arr[i].SetActive(true);
                }
            }
            yield return new WaitForSeconds(1);
            
            StartCoroutine(TypeWriter());

        }
        IEnumerator LosePanelComes()
        {
            BG_Music.SetActive(false);
            Instantiate(victorySound, transform);

            yield return new WaitForSeconds(2.5f);
            rewardPanelBase.SetActive(true);
            title.sprite = defeat;
            All_Stars.SetActive(false);
            // StartCoroutine(TypeWriter());
            /*  if (PlayerHP.value >= 0.75f)
              {
                  for (int i = 0; i < 3; i++)
                  {
                      Stars_Arr[i].SetActive(true);
                  }
              }
              else if (PlayerHP.value < 75 && PlayerHP.value >= 45)
              {
                  for (int i = 0; i < 2; i++)
                  {
                      Stars_Arr[i].SetActive(true);
                  }
              }
              else if (PlayerHP.value < 45)
              {
                  for (int i = 0; i < 1; i++)
                  {
                      Stars_Arr[i].SetActive(true);
                  }
              }
              */
        }
    }
}