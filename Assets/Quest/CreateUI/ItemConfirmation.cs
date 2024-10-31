using System;
using UnityEngine;
using TMPro;
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
	[SerializeField] Vector2 m_offsetDeleteItem;
	[SerializeField] Vector2 m_offsetReplacementItem;
	[SerializeField] Vector2[] m_offsetMessage;
	[SerializeField] Vector2[] m_offsetButton;
	[SerializeField] string[] m_messages;

	//�\�����e�̃Z�b�g
	public void SetDisplayContent(in MESSAGE message)
	{
		HideChilds();
		switch (message)
		{
			case MESSAGE.INPUT_NULL:
				WarningAtEntry();
				break;
			case MESSAGE.DELETING_CONFIRMATION:
				break;
			case MESSAGE.CHANGING_CONFIRMATION:
				break;
		}
		m_confirmation.gameObject.SetActive(true);
	}

	//�x�����b�Z�[�W
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

	//�ύX�m�F
	void ChangingConfirmation()
	{
		int num = (int)MESSAGE.CHANGING_CONFIRMATION;

		GameObject deleteItem = GetChildComponent<GameObject>("Items/DeleteItem");

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

	//�폜�m�F
	void DeletingConfirmation()
	{
		int num = (int)MESSAGE.DELETING_CONFIRMATION;

		GameObject deleteItem = GetChildComponent<GameObject>("Items/DeleteItem");
		deleteItem.SetActive(true);

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

	//�q�I�u�W�F�N�g���\��
	void HideChilds()
	{
		foreach (Transform child in m_confirmation.transform)
			child.gameObject.SetActive(false);
		foreach (Transform child in GetChildComponent<Transform>("Buttons"))
			child.gameObject.SetActive(false);

		//buttons�I�u�W�F�N�g�͑ΏۊO
		Transform buttons = GetChildComponent<Transform>("Buttons");
		buttons.gameObject.SetActive(true);
	}

	//�m�F��ʂ̔�\��
	public void HideConfirmationWindow()
	{
		m_confirmation.gameObject.SetActive(false);
	}

	//�q�I�u�W�F�N�g�̌���
	T GetChildComponent<T>(in string findName)
	{
		return m_confirmation.transform.Find(findName).GetComponent<T>();
	}
}
