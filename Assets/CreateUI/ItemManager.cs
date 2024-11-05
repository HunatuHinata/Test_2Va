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

	[Header("Item��")]
	[SerializeField] QuestSO m_questSO;
	[SerializeField] GameObject m_searchObject;
	[SerializeField] GameObject m_itemPrefab;
	[SerializeField] GameObject m_parent;

	[Header("�{�^��")]
	[SerializeField] GameObject m_addOrReplaceButton;
	[SerializeField] GameObject m_deleteButton;
	[SerializeField] GameObject m_editButton;

	[Header("�쐬�p")]
	[SerializeField] ItemCreat m_itemCreat;

	[Header("�m�F��ʗp")]
	[SerializeField] ItemConfirmation m_itemConfirmation;

	//[Header("�t�H���g")]
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

	//Item�̐���
	void GeneratItem(Quest quest)
	{
        GameObject itemObject = GameObject.Instantiate(m_itemPrefab, m_parent.transform);
        itemObject.GetComponent<ItemView>().Creat(quest);
        m_items.Add(itemObject);
    }

	//�{�^���̃��Z�b�g��
	void OnResetButtons()
	{
		m_addOrReplaceButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "�ǉ�";
		m_editButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "�ҏW";
		m_deleteButton.SetActive(true);
	}

	//�{�^���̕ҏW���[�h��
	void OnEditModeButtons()
	{
		m_addOrReplaceButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "�ύX";
		m_editButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "������";
		m_deleteButton.SetActive(false);
	}

	void Update()
    {
		ItemSearch();

		if (Input.GetMouseButtonDown(0)) ItemSelect();
	}

	//Item������
	void ItemSearch()
	{
#if false //TextMeshProUGUI�ł̍쐬���@�i�������߂�ǂ������j
		TextMeshProUGUI tmp_Input = m_searchObject.GetComponent<TextMeshProUGUI>();
		string work = string.Empty;
		for (int i = 0; i < tmp_Input.text.Length - 1; i++) work += tmp_Input.text[i];

#else	//TMP_InputField���g�p������@�i�������̕����ȒP�j
		TMP_InputField tmp_Input = m_searchObject.GetComponent<TMP_InputField>();
		string work = tmp_Input.text;
#endif
		//�������[�h�̏ꍇ
		if (m_search == work) return;
		m_search = work;

		//�������[�h�Ɉꕶ�����q�b�g���Ȃ���Δ�\��
		foreach (GameObject item in m_items) item.SetActive(true);

		if (m_search == string.Empty) return;
		List<GameObject> items = m_items.Where(o => o.name.IndexOf(m_search) < 0).ToList();
		foreach (GameObject item in items) item.SetActive(false);
	}

	//Item��I��
	void ItemSelect()
	{
		PointerEventData ped = new PointerEventData(EventSystem.current);
		ped.position = Input.mousePosition;
		List<RaycastResult> result = new List<RaycastResult>();
		EventSystem.current.RaycastAll(ped, result);

		if (result.Any(o => o.gameObject.CompareTag("NoSelect"))) return;

		//ItemUI�̃^�O���Ȃ��ꍇ
		//�O��̑I������Item�̃n�C���C�g������
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

			//FoldoutUI�̃^�O������ꍇ
			//�N�G�X�g�̏ڍׂ�\������
			if (result.Any(o => o.gameObject.CompareTag("FoldoutUI")))
				m_selectObject.transform.GetComponent<ItemView>().ItemDisplaySwitching();
		}
	}

	//�{�^��
	public void SelectButton(bool bSelect)
	{
		m_itemConfirmation.HideConfirmationWindow();

		if (m_bAddMode) return;

		AfterAction(bSelect);
		AfterAction = null;
	}

	//�ǉ�or�ύX�I���{�^��
	public void AddOrChangeButton()
	{
		Quest work = m_itemCreat.GetItem();
		if (work == null)
		{
			m_itemConfirmation.SetDisplayContent(MESSAGE.INPUT_NULL);
			return;
		}

		//�ǉ�
		if (m_bAddMode)
		{
			m_questSO.quests.Add(work);
			GeneratItem(work);
		}
		//�ύX
		else
		{
			Quest item = m_questSO.quests.Find(n => n.GetQuest().name == m_targetObject.name);
			m_itemConfirmation.SetDisplayContent(MESSAGE.CHANGING_CONFIRMATION, item, work);
		}
	}

	//�ҏW�{�^��
	public void EditButton()
	{
		//�ҏW
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
		//������
		else
		{
			AfterAction(true);
		}
	}

	//�폜�{�^��
	public void DeleteButton()
	{
		//�폜
		if (m_selectObject && m_bAddMode) 
		{
			Quest item = m_questSO.quests.Find(n => n.GetQuest().name == m_selectObject.name);
			m_targetObject = m_selectObject;
			m_selectObject = null;
			m_bAddMode = false;

			//�f���Q�[�g�ɕۑ�
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
