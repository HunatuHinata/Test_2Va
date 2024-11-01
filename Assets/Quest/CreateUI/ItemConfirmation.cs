using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MESSAGE
{
	INPUT_NULL,
	DELETING_CONFIRMATION,
	CHANGING_CONFIRMATION,
}

[System.Serializable]
public class ItemConfirmation
{
	[SerializeField] GameObject m_confirmation;
	[SerializeField] GameObject m_itemPrefub;
	[SerializeField] Vector3 m_scaleItem;
	[SerializeField] Vector2 m_offsetParentItems;
	[SerializeField] Vector2[] m_offsetItem;
	[SerializeField] Vector2[] m_offsetMessage;
	[SerializeField] Vector2[] m_offsetButton;
	[SerializeField] string[] m_messages;

	//表示内容のセット
	public void SetDisplayContent(in MESSAGE message, in Quest before = null, in Quest after = null)
	{
		HideChilds();

		int num = (int)message;
		if (before != null) SettingArrow();
		SettingItems(num, before, after);
		SettingMessage(num);
		//beforが無ければボタンを一つにする
		SettingButtons((num == 0), num);

		m_confirmation.gameObject.SetActive(true);
	}

	void SettingArrow()
	{
		GameObject arrow = GetChildComponent<Transform>("Arrow").gameObject;
		arrow.GetComponent<RectTransform>().anchoredPosition = m_offsetParentItems;
		arrow.SetActive(true);
	}

	//Itemの設定
	void SettingItems(in int num, in Quest before, in Quest after)
	{
		if (before == null) return;
		GetChildComponent<RectTransform>("Items").anchoredPosition = m_offsetParentItems;
		GameObject deleteItem = GameObject.Instantiate(m_itemPrefub, GetChildComponent<Transform>("Items"));
		deleteItem.GetComponent<RectTransform>().anchoredPosition = m_offsetItem[num];
		deleteItem.GetComponent<RectTransform>().localScale = m_scaleItem;
		deleteItem.GetComponent<ItemView>().Creat(before, true);

		if (after == null) return;
		GameObject replacementItem = GameObject.Instantiate(m_itemPrefub, GetChildComponent<Transform>("Items"));
		replacementItem.GetComponent<RectTransform>().anchoredPosition = -m_offsetItem[num];
		replacementItem.GetComponent<RectTransform>().localScale = m_scaleItem;
		replacementItem.GetComponent<ItemView>().Creat(after, true);
	}

	//テキストの設定
	void SettingMessage(in int num)
	{
		TextMeshProUGUI messageUGUI = GetChildComponent<TextMeshProUGUI>("Message(TMP)");
		messageUGUI.text = m_messages[num];
		messageUGUI.transform.GetComponent<RectTransform>().anchoredPosition = m_offsetMessage[num];
		messageUGUI.gameObject.SetActive(true);
	}

	//ボタン設定
	void SettingButtons(in bool bOnly, in int num)
	{
		Button ok_Button = GetChildComponent<Button>("Buttons/OK");
		ok_Button.transform.GetComponent<RectTransform>().anchoredPosition = m_offsetButton[num];
		ok_Button.gameObject.SetActive(true);

		if (bOnly) return;
		Button no_Button = GetChildComponent<Button>("Buttons/NO");
		no_Button.transform.GetComponent<RectTransform>().anchoredPosition = -m_offsetButton[num];
		no_Button.gameObject.SetActive(true);
	}

	//子オブジェクトを非表示
	void HideChilds()
	{
		//非表示対象
		GetChildComponent<Transform>("Arrow").gameObject.SetActive(false);
		GetChildComponent<Transform>("Message(TMP)").gameObject.SetActive(false);

		//ボタンの子オブジェクト
		foreach (Transform child in GetChildComponent<Transform>("Buttons"))
			child.gameObject.SetActive(false);

		//Itemsの子オブジェクトを削除
		if (GetChildComponent<Transform>("Items").childCount == 1) return;

		foreach (Transform item in GetChildComponent<Transform>("Items")) 
			GameObject.Destroy(item.gameObject);
	}

	//確認画面の非表示
	public void HideConfirmationWindow()
	{
		HideChilds();

		m_confirmation.gameObject.SetActive(false);
	}

	//子オブジェクトの検索
	T GetChildComponent<T>(in string findName)
	{
		return m_confirmation.transform.Find(findName).GetComponent<T>();
	}
}
