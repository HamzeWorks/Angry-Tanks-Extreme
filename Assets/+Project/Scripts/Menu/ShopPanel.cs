using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Amulay.Utility;
using UnityEngine.AddressableAssets;

public class ShopPanel : Singleton<ShopPanel>
{
	[SerializeField] private AssetReference panelReference;
    [SerializeField] private Button backButton;

	private GameObject panel,gold_panel;
    private bool loaded = false;
    protected override void Awake()
    {
        base.Awake();
        backButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        if (loaded == false)
        {
	        panelReference.InstantiateAsync(transform).Completed += (result) => { panel = result.Result;  panel.transform.SetAsFirstSibling(); };
            loaded = true;
        }

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
    }
	
    public void Hide()
    {
        panel?.gameObject.SetActive(false);
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
	public void ShowGoldPanel()
	{
		if (loaded == false)
		{
			panelReference.InstantiateAsync(transform).Completed += (result) => { panel = result.Result;  gold_panel.transform.SetAsFirstSibling(); };
			loaded = true;
			print("Gold Panel injast");
		} for (int i = 0; i < transform.childCount; i++)
			transform.GetChild(i).gameObject.SetActive(true);
	}
	public void HideGoldPanel()
	{
		gold_panel?.gameObject.SetActive(false);
		for (int i = 0; i < transform.childCount; i++)
			transform.GetChild(i).gameObject.SetActive(false);
	}
}
