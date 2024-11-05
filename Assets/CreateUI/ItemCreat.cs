using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class ItemCreat
{
	[SerializeField] GameObject m_settingObject;

	//�擾
	public Quest GetItem()
	{
		Quest quest = null;
		quest = GetInputContentAcquisition();

		if (quest.GetQuest().name == string.Empty || quest.GetQuest().detail == string.Empty) return null;

		return quest;
	}

	//����Item�����Z�b�g
	public void ResetsInputItem()
	{
		Transform workTransform;

		//���O������
		workTransform = FindChildTransform("QuestName/Input");
		workTransform.GetComponent<TMP_InputField>().text = "";
		workTransform = FindChildTransform("QuestExplanation/Input");
		workTransform.GetComponent<TMP_InputField>().text = "";

		//���
		workTransform = FindChildTransform("Conditions/ToggleAppearance");
		workTransform.GetComponent<Toggle>().isOn = false;
		workTransform = FindChildTransform("Conditions/ToggleNew");
		workTransform.GetComponent<Toggle>().isOn = true;
		workTransform = FindChildTransform("Conditions/ToggleClear");
		workTransform.GetComponent<Toggle>().isOn = false;
	}

	//���͓��e���o��
	Quest GetInputContentAcquisition()
	{
		Quest workQuest = new Quest();
		Transform workTransform;
		string name, explanation;

		//���O������
		workTransform = FindChildTransform("QuestName/Input");
		name = workTransform.GetComponent<TMP_InputField>().text;
		workTransform = FindChildTransform("QuestExplanation/Input");
		explanation = workTransform.GetComponent<TMP_InputField>().text;
		workQuest.SetQuest(name, explanation);

		//���
		workTransform = FindChildTransform("Conditions/ToggleAppearance");
		if (IsToggleOn(workTransform)) workQuest.SetKey(Quest.KEY.POSSIBLE);
		workTransform = FindChildTransform("Conditions/ToggleNew");
		if (!IsToggleOn(workTransform)) workQuest.SetKey(Quest.KEY.NO_NEW);
		workTransform = FindChildTransform("Conditions/ToggleClear");
		if (IsToggleOn(workTransform)) workQuest.SetKey(Quest.KEY.CLEAR);

		return workQuest;
	}

	//���e���Z�b�g
	public void SetContents(in Quest quest)
	{
		Transform workTransform;

		//���O������
		workTransform = FindChildTransform("QuestName/Input");
		workTransform.GetComponent<TMP_InputField>().text = quest.GetQuest().name;
		workTransform = FindChildTransform("QuestExplanation/Input");
		workTransform.GetComponent<TMP_InputField>().text = quest.GetQuest().detail;

		//���
		workTransform = FindChildTransform("Conditions/ToggleAppearance");
		workTransform.GetComponent<Toggle>().isOn = quest.IsKey(Quest.KEY.POSSIBLE);
		workTransform = FindChildTransform("Conditions/ToggleNew");
		workTransform.GetComponent<Toggle>().isOn = !quest.IsKey(Quest.KEY.NO_NEW);
		workTransform = FindChildTransform("Conditions/ToggleClear");
		workTransform.GetComponent<Toggle>().isOn = quest.IsKey(Quest.KEY.CLEAR);
	}

	//�q�I�u�W�F�N�g�̌���
	Transform FindChildTransform(in string findName)
	{
		return m_settingObject.transform.Find(findName);
	}

	//Toggle�R���|�[�l���g�̃`�F�b�N�̎擾
	bool IsToggleOn(in Transform targetTransform)
	{
		return targetTransform.GetComponent<Toggle>().isOn;
	}
}
