using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class ItemCreat
{
	[SerializeField] GameObject settingObject;

	QuestSO m_questSO;

	public void SetQuestListRef(ref QuestSO questSO)
	{
		m_questSO = questSO;
	}

	public void AddQuest()
	{
		Quest quest;
		InputQuest(out quest);
		m_questSO.quests.Add(quest);
	}

	public void ReplacementQuest(ref Quest refQuest)
	{
		Quest quest;
		InputQuest(out quest);
		refQuest = quest;
	}

	void InputQuest(out Quest outQuest)
	{
		outQuest = new Quest();
		Transform questTransform;
		string name, explanation;

		//���O������
		FindChilled("QuestName/Input", out questTransform);
		name = questTransform.GetComponent<TMP_InputField>().text;
		FindChilled("QuestExplanation/Input", out questTransform);
		explanation = questTransform.GetComponent<TMP_InputField>().text;
		outQuest.SetQuest(name, explanation);
		//���
		FindChilled("Conditions/ToggleAppearance", out questTransform);
		if (IsToggleOn(questTransform)) outQuest.SetKey(Quest.KEY.POSSIBLE);
		FindChilled("Conditions/ToggleNew", out questTransform);
		if (!IsToggleOn(questTransform)) outQuest.SetKey(Quest.KEY.NO_NEW);
		FindChilled("Conditions/ToggleClear", out questTransform);
		if (IsToggleOn(questTransform)) outQuest.SetKey(Quest.KEY.CLEAR);
	}

	//�q�I�u�W�F�N�g�̎擾(����)
	void FindChilled(in string name, out Transform findTransform)
	{
		findTransform = settingObject.transform.Find(name);
	}

	//Toggle�R���|�[�l���g�̃`�F�b�N�̎擾
	bool IsToggleOn(in Transform questTransform)
	{
		return questTransform.GetComponent<Toggle>().isOn;
	}
}
