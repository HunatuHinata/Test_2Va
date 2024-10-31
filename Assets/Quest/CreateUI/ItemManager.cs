using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
	[SerializeField] QuestSO m_questSO;
	[SerializeField] GameObject m_searchObject;
	[SerializeField] GameObject m_itemPrefab;
	[SerializeField] GameObject m_parent;
	[SerializeField] ItemCreat m_itemCreat;
	[SerializeField] ItemConfirmation m_itemConfirmation;

	List<GameObject> m_items = new List<GameObject>();
	GameObject m_selectObject;
	GameObject m_targetObject;
	string m_search;
	bool m_bAddMode;
	Action m_function;

	void Start()
    {
		m_bAddMode = true;
		m_function = null;
		m_selectObject = null;
		m_targetObject = null;
		m_itemCreat.SetQuestListRef(ref m_questSO);
		m_search = string.Empty;
		//m_searchObject.GetComponent<TextMeshProUGUI>().text = string.Empty;
		m_searchObject.GetComponent<TMP_InputField>().text = string.Empty;
		foreach (Quest quest in m_questSO.quests)
			GeneratItem(quest);
	}

	void Update()
    {
		ItemSearch();

		if (Input.GetMouseButtonDown(0)) 
			ItemSelect();
	}

	//Itemの生成
	void GeneratItem(Quest quest)
	{
        GameObject itemObject = GameObject.Instantiate(m_itemPrefab, m_parent.transform);
        itemObject.GetComponent<ItemView>().Creat(quest);
        m_items.Add(itemObject);
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

		//検索ワードに一文字でもヒットしなければ非表示
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
		m_selectObject?.transform.GetComponent<ItemView>().SetHighlightAnimation(false);
        if (!result.Any(o => o.gameObject.CompareTag("ItemUI")))
		{
			m_selectObject = null;
			return;
		}
		
		RaycastResult itemObject = result.Find(o => o.gameObject.CompareTag("ItemUI"));
		m_selectObject = itemObject.gameObject;

		m_selectObject?.transform.GetComponent<ItemView>().SetHighlightAnimation(true);

        //FoldoutUIのタグがある場合
        //クエストの詳細を表示する
        if (result.Any(o => o.gameObject.CompareTag("FoldoutUI"))) 
			m_selectObject.transform.GetComponent<ItemView>().ItemDisplaySwitching();
	}

	//ボタン
	public void SelectButton(bool bSelect)
	{
		m_itemConfirmation.HideConfirmationWindow();

		if (!bSelect) return;
	}

	//追加or編集ボタン
	public void AddOrCompletedButton()
	{
		if (m_bAddMode)
		{
			Quest work = m_itemCreat.AddItem();
			if (work == null)
			{
				m_itemConfirmation.SetDisplayContent(MESSAGE.INPUT_NULL);
				return;
			}
			//itemの追加
			GeneratItem(work);
		}
	}

	//編集ボタン
	public void EditButton()
	{
		if (!m_selectObject) return;

	}

	//削除ボタン
	public void DeleteButton()
	{
		if (!m_selectObject) return;

		int result = m_questSO.quests.FindIndex(n => n.GetQuest().name == m_selectObject.name);
		
		m_itemConfirmation.SetDisplayContent(MESSAGE.DELETING_CONFIRMATION);
	}
}
