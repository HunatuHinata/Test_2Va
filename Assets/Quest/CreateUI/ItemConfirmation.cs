using System;
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
	[SerializeField] Vector2 m_offsetItem;
	[SerializeField] Vector2[] m_offsetMessage;
	[SerializeField] Vector2[] m_offsetButton;
	[SerializeField] string[] m_messages;

	//表示内容のセット
	public void SetDisplayContent(in MESSAGE message, in Quest before = null, in Quest after = null)
	{
		HideChilds();
		switch (message)
		{
			case MESSAGE.INPUT_NULL:
				WarningAtEntry();
				break;
			case MESSAGE.DELETING_CONFIRMATION:
				DeletingConfirmation(before);
				break;
			case MESSAGE.CHANGING_CONFIRMATION:
				break;
		}
		m_confirmation.gameObject.SetActive(true);
	}

	//警告メッセージ
	void WarningAtEntry()
	{
		int num = (int)MESSAGE.INPUT_NULL;
		TextMeshProUGUI messageUGUI = GetChildComponent<TextMeshProUGUI>("Message(TMP)");
		messageUGUI.text = m_messages[num];
		messageUGUI.transform.GetComponent<RectTransform>().anchoredPosition = m_offsetMessage[num];
		messageUGUI.gameObject.SetActive(true);

		Button ok_Button = GetChildComponent<Button>("Buttons/OK");
		ok_Button.transform.GetComponent<RectTransform>().anchoredPosition = m_offsetButton[num];
		ok_Button.gameObject.SetActive(true);
	}

	//変更確認
	void ChangingConfirmation(in Quest before, in Quest after)
	{
		int num = (int)MESSAGE.CHANGING_CONFIRMATION;

		//矢印
		GetChildComponent<GameObject>("Arrow").SetActive(true);

		GetChildComponent<RectTransform>("Items").anchoredPosition = m_offsetParentItems;
		GameObject deleteItem = GameObject.Instantiate(m_itemPrefub, GetChildComponent<Transform>("Items"));
		deleteItem.GetComponent<RectTransform>().anchoredPosition = m_offsetItem;
		deleteItem.GetComponent<RectTransform>().localScale = m_scaleItem;
		deleteItem.GetComponent<ItemView>().Creat(before, true);
		GameObject replacementItem = GameObject.Instantiate(m_itemPrefub, GetChildComponent<Transform>("Items"));
		replacementItem.GetComponent<RectTransform>().anchoredPosition = -m_offsetItem;
		replacementItem.GetComponent<RectTransform>().localScale = m_scaleItem;
		replacementItem.GetComponent<ItemView>().Creat(before, true);

		TextMeshProUGUI messageUGUI = GetChildComponent<TextMeshProUGUI>("Message(TMP)");
		messageUGUI.text = m_messages[num];
		messageUGUI.transform.GetComponent<RectTransform>().anchoredPosition = m_offsetMessage[num];
		messageUGUI.gameObject.SetActive(true);

		Button ok_Button = GetChildComponent<Button>("Buttons/OK");
		ok_Button.transform.GetComponent<RectTransform>().anchoredPosition = m_offsetButton[num];
		ok_Button.gameObject.SetActive(true);
		Button no_Button = GetChildComponent<Button>("Buttons/NO");
		no_Button.transform.GetComponent<RectTransform>().anchoredPosition = -m_offsetButton[num];
		no_Button.gameObject.SetActive(true);
	}

	//削除確認
	void DeletingConfirmation(in Quest quest)
	{
		int num = (int)MESSAGE.DELETING_CONFIRMATION;

		//Itemオブジェクト
		GetChildComponent<RectTransform>("Items").anchoredPosition = m_offsetParentItems;
		GameObject deleteItem = GameObject.Instantiate(m_itemPrefub, GetChildComponent<Transform>("Items"));
		deleteItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		deleteItem.GetComponent<RectTransform>().localScale = m_scaleItem;
		deleteItem.GetComponent<ItemView>().Creat(quest, true);

		//メッセージ
		TextMeshProUGUI messageUGUI = GetChildComponent<TextMeshProUGUI>("Message(TMP)");
		messageUGUI.text = m_messages[num];
		messageUGUI.transform.GetComponent<RectTransform>().anchoredPosition = m_offsetMessage[num];
		messageUGUI.gameObject.SetActive(true);

		//ボタン
		Button ok_Button = GetChildComponent<Button>("Buttons/OK");
		ok_Button.transform.GetComponent<RectTransform>().anchoredPosition = m_offsetButton[num];
		ok_Button.gameObject.SetActive(true);
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
		if (GetChildComponent<Transform>("Items").childCount == 0) return;

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
