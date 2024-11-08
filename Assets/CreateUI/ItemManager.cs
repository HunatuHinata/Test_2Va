using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestConverter
{
	const byte POSSIBLE = 0x01;
	const byte NO_NEW = 0x02;
	const byte CLEAR = 0x04;


}

public class ItemManager : MonoBehaviour
{
#if DEBUG
	[Header("TMP_UGUI変更用")]
	[SerializeField] GameObject[] m_meshProUGUIs;
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

	//OKorNOボタンの
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

			//追加モード以外は無効
			if (m_bAddMode)
			{
				if (m_targetObject != m_selectObject)
					m_selectObject?.transform.GetComponent<ItemView>().SetHighlightAnimation(true); 
			}

			//FoldoutUIのタグがある場合
			//クエストの詳細を表示する
			if (result.Any(o => o.gameObject.CompareTag("FoldoutUI")))
				m_selectObject.transform.GetComponent<ItemView>().ItemDisplaySwitching();
		}
	}

	//選択ボタン
	public void SelectButton(bool bSelect)
	{
		m_itemConfirmation.HideConfirmationWindow();

		if (m_bAddMode) return;

		AfterAction(bSelect);
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

		if (m_bAddMode)
		{
			m_questSO.quests.Add(work);
			GeneratItem(work);
		}
		else
		{
			//Quest保存先から同じものを取得
			Quest item = GetQuest(m_targetObject.name);
			m_itemConfirmation.SetDisplayContent(MESSAGE.CHANGING_CONFIRMATION, item, work);
		}
	}

	//編集ボタン
	public void EditButton()
	{
		if (m_bAddMode)
		{
			if (!m_selectObject) return;

			OnEditModeButtons();
			m_targetObject = m_selectObject;
			m_selectObject = null;
			m_bAddMode = false;
			AfterAction = null;
			m_targetObject.GetComponent<ItemView>().SetHighlightAnimation(true, true);

			Quest item = GetQuest(m_targetObject.name);
			m_itemCreat.SetContents(item);

			//変更時のデリゲート
			AfterAction = (bool bCheck) =>
			{
				if (!bCheck) return;

				int itemNum = m_questSO.quests.FindIndex(n => n.GetQuest().name == m_targetObject.name);
				Quest input = m_itemCreat.GetItem();
				m_questSO.quests[itemNum] = input;
				m_targetObject.GetComponent<ItemView>().Initialize(input);

				OnResetButtons();
				m_targetObject.GetComponent<ItemView>().SetHighlightAnimation(false);
				m_targetObject = null;
				m_bAddMode = true;

				m_itemCreat.ResetsInputItem();
				AfterAction = null;
			};
		}
		//取り消し
		else
		{
			Quest item = GetQuest(m_targetObject.name);
			m_itemCreat.SetContents(item);
			AfterAction(true);
		}
	}

	//削除ボタン
	public void DeleteButton()
	{
		//削除
		if (m_selectObject && m_bAddMode) 
		{
			m_targetObject = m_selectObject;
			m_selectObject = null;
			m_targetObject.GetComponent<ItemView>().SetHighlightAnimation(true, true);
			m_bAddMode = false;

			//削除時のデリゲート
			AfterAction = (bool bCheck) => 
			{
				if (bCheck)
				{
					Quest item = m_questSO.quests.Find(n => n.GetQuest().name == m_targetObject.name);
					int itemNum = m_questSO.quests.FindIndex(n => n.GetQuest().name == m_targetObject.name);
					m_questSO.quests.Remove(item);
					Destroy(m_targetObject); 
				}

				m_targetObject.GetComponent<ItemView>().SetHighlightAnimation(false);
				m_targetObject = null;
				m_bAddMode = true;
				AfterAction = null;
			};

			Quest item = GetQuest(m_targetObject.name);
			m_itemConfirmation.SetDisplayContent(MESSAGE.DELETING_CONFIRMATION, item);
		}
	}

	//クエストを取得
	Quest GetQuest(string questName)
	{
		return m_questSO.quests.Find(n => n.GetQuest().name == questName);
	}
}
