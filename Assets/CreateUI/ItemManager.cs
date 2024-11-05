using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
#if false
	[System.Serializable]
	class TextMeshProsSeting
	{
		[SerializeField] TextMeshProUGUI[] m_meshProUGUIs;
		[SerializeField] TMP_FontAsset m_tMP_FontAsset;
		public void SetFonts()
		{
			foreach (TextMeshProUGUI item in m_meshProUGUIs)
				item.font = m_tMP_FontAsset;
		}
	} 
#endif

	[Header("Item類")]
	[SerializeField] QuestSO m_questSO;
	[SerializeField] GameObject m_searchObject;
	[SerializeField] GameObject m_itemPrefab;
	[SerializeField] GameObject m_parent;

	[Header("ボタン")]
	[SerializeField] GameObject m_addOrReplaceButton;
	[SerializeField] GameObject m_deleteButton;
	[SerializeField] GameObject m_editButton;

	[Header("作成用")]
	[SerializeField] ItemCreat m_itemCreat;

	[Header("確認画面用")]
	[SerializeField] ItemConfirmation m_itemConfirmation;

	//[Header("フォント")]
	//[SerializeField] TextMeshProsSeting m_meshProsSeting;

	List<GameObject> m_items = new List<GameObject>();
	GameObject m_selectObject;
	GameObject m_targetObject;
	string m_search;
	bool m_bAddMode;
	delegate void After(bool bCheck);
	After AfterAction;

	void Start()
    {
		m_bAddMode = true;
		AfterAction = null;
		m_selectObject = null;
		m_targetObject = null;
		m_search = string.Empty;

		OnResetButtons();

		//m_searchObject.GetComponent<TextMeshProUGUI>().text = string.Empty;
		m_searchObject.GetComponent<TMP_InputField>().text = string.Empty;
		foreach (Quest quest in m_questSO.quests)
			GeneratItem(quest);
	}

	//Itemの生成
	void GeneratItem(Quest quest)
	{
        GameObject itemObject = GameObject.Instantiate(m_itemPrefab, m_parent.transform);
        itemObject.GetComponent<ItemView>().Creat(quest);
        m_items.Add(itemObject);
    }

	//ボタンのリセット時
	void OnResetButtons()
	{
		m_addOrReplaceButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "追加";
		m_editButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "編集";
		m_deleteButton.SetActive(true);
	}

	//ボタンの編集モード時
	void OnEditModeButtons()
	{
		m_addOrReplaceButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "変更";
		m_editButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "取り消し";
		m_deleteButton.SetActive(false);
	}

	void Update()
    {
		ItemSearch();

		if (Input.GetMouseButtonDown(0)) ItemSelect();
	}

	//Itemを検索
	void ItemSearch()
	{
#if false //TextMeshProUGUIでの作成方法（こっちめんどくさい）
		TextMeshProUGUI tmp_Input = m_searchObject.GetComponent<TextMeshProUGUI>();
		string work = string.Empty;
		for (int i = 0; i < tmp_Input.text.Length - 1; i++) work += tmp_Input.text[i];

#else	//TMP_InputFieldを使用する方法（こっちの方が簡単）
		TMP_InputField tmp_Input = m_searchObject.GetComponent<TMP_InputField>();
		string work = tmp_Input.text;
#endif
		//同じワードの場合
		if (m_search == work) return;
		m_search = work;

		//検索ワードに一文字もヒットしなければ非表示
		foreach (GameObject item in m_items) item.SetActive(true);

		if (m_search == string.Empty) return;
		List<GameObject> items = m_items.Where(o => o.name.IndexOf(m_search) < 0).ToList();
		foreach (GameObject item in items) item.SetActive(false);
	}

	//Itemを選択
	void ItemSelect()
	{
		PointerEventData ped = new PointerEventData(EventSystem.current);
		ped.position = Input.mousePosition;
		List<RaycastResult> result = new List<RaycastResult>();
		EventSystem.current.RaycastAll(ped, result);

		if (result.Any(o => o.gameObject.CompareTag("NoSelect"))) return;

		//ItemUIのタグがない場合
		//前回の選択したItemのハイライトを消す
		if (m_targetObject != m_selectObject)
			m_selectObject?.transform.GetComponent<ItemView>().SetHighlightAnimation(false);

        if (!result.Any(o => o.gameObject.CompareTag("ItemUI")))
		{
			m_selectObject = null;
		}
		else
		{
			RaycastResult itemObject = result.Find(o => o.gameObject.CompareTag("ItemUI"));
			m_selectObject = itemObject.gameObject;

			if (m_targetObject != m_selectObject)
				m_selectObject?.transform.GetComponent<ItemView>().SetHighlightAnimation(true);

			//FoldoutUIのタグがある場合
			//クエストの詳細を表示する
			if (result.Any(o => o.gameObject.CompareTag("FoldoutUI")))
				m_selectObject.transform.GetComponent<ItemView>().ItemDisplaySwitching();
		}
	}

	//ボタン
	public void SelectButton(bool bSelect)
	{
		m_itemConfirmation.HideConfirmationWindow();

		if (m_bAddMode) return;

		AfterAction(bSelect);
		AfterAction = null;
	}

	//追加or変更終了ボタン
	public void AddOrChangeButton()
	{
		Quest work = m_itemCreat.GetItem();
		if (work == null)
		{
			m_itemConfirmation.SetDisplayContent(MESSAGE.INPUT_NULL);
			return;
		}

		//追加
		if (m_bAddMode)
		{
			m_questSO.quests.Add(work);
			GeneratItem(work);
		}
		//変更
		else
		{
			Quest item = m_questSO.quests.Find(n => n.GetQuest().name == m_targetObject.name);
			m_itemConfirmation.SetDisplayContent(MESSAGE.CHANGING_CONFIRMATION, item, work);
		}
	}

	//編集ボタン
	public void EditButton()
	{
		//編集
		if (m_bAddMode)
		{
			if (!m_selectObject) return;

			OnEditModeButtons();
			m_targetObject = m_selectObject;
			m_selectObject = null;
			m_bAddMode = false;
			AfterAction = null;
			m_targetObject.GetComponent<ItemView>().SetHighlightAnimation(true, true);

			Quest item = m_questSO.quests.Find(n => n.GetQuest().name == m_targetObject.name);
			m_itemCreat.SetContents(item);

			AfterAction = (bool bCheck) => {
				if (bCheck)
				{
					int itemNum = m_questSO.quests.FindIndex(n => n.GetQuest().name == m_targetObject.name);
					Quest input = m_itemCreat.GetItem();
					m_questSO.quests[itemNum] = input;
					m_targetObject.GetComponent<ItemView>().Initialize();

					OnResetButtons();
					m_targetObject.GetComponent<ItemView>().SetHighlightAnimation(false);
					m_targetObject = null;
					m_bAddMode = true;
				}
				m_itemCreat.ResetsInputItem();
			};
		}
		//取り消し
		else
		{
			AfterAction(true);
		}
	}

	//削除ボタン
	public void DeleteButton()
	{
		//削除
		if (m_selectObject && m_bAddMode) 
		{
			Quest item = m_questSO.quests.Find(n => n.GetQuest().name == m_selectObject.name);
			m_targetObject = m_selectObject;
			m_selectObject = null;
			m_bAddMode = false;

			//デリゲートに保存
			AfterAction = (bool bCheck) => {
				if (bCheck)
				{
					Quest item = m_questSO.quests.Find(n => n.GetQuest().name == m_targetObject.name);
					int itemNum = m_questSO.quests.FindIndex(n => n.GetQuest().name == m_targetObject.name);
					m_questSO.quests.Remove(item);
					Destroy(m_targetObject); 
				}
				m_targetObject = null;
				m_bAddMode = true;
			};

			m_itemConfirmation.SetDisplayContent(MESSAGE.DELETING_CONFIRMATION, item);
		}
	}
}
