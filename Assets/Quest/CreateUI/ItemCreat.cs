using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class ItemCreat
{
	[SerializeField] GameObject settingObject;

	QuestSO m_questSO;

	//QuestSO���Z�b�g
	public void SetQuestListRef(ref QuestSO questSO)
	{
		m_questSO = questSO;
	}

	//�ǉ�
	public Quest AddItem()
	{
		Quest quest = null;
		quest = InputContentAcquisition();

		if (quest.GetQuest().name == string.Empty || quest.GetQuest().detail == string.Empty) return null;

		m_questSO.quests.Add(quest);
		return quest;
	}

	//�ҏW
	public void EditItem(ref Quest refQuest)
	{
		Quest quest;
		quest = InputContentAcquisition();
		refQuest = quest;
	}

	//���͓��e���o��
	Quest InputContentAcquisition()
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

	//�q�I�u�W�F�N�g�̌���
	Transform FindChildTransform(in string findName)
	{
		return settingObject.transform.Find(findName);
	}

	//Toggle�R���|�[�l���g�̃`�F�b�N�̎擾
	bool IsToggleOn(in Transform targetTransform)
	{
		return targetTransform.GetComponent<Toggle>().isOn;
	}
}
