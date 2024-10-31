using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class ItemCreat
{
	[SerializeField] GameObject settingObject;

	QuestSO m_questSO;

	//QuestSOをセット
	public void SetQuestListRef(ref QuestSO questSO)
	{
		m_questSO = questSO;
	}

	//追加
	public Quest AddItem()
	{
		Quest quest = null;
		quest = InputContentAcquisition();

		if (quest.GetQuest().name == string.Empty || quest.GetQuest().detail == string.Empty) return null;

		m_questSO.quests.Add(quest);
		return quest;
	}

	//編集
	public void EditItem(ref Quest refQuest)
	{
		Quest quest;
		quest = InputContentAcquisition();
		refQuest = quest;
	}

	//入力内容を出す
	Quest InputContentAcquisition()
	{
		Quest workQuest = new Quest();
		Transform workTransform;
		string name, explanation;

		//名前＆説明
		workTransform = FindChildTransform("QuestName/Input");
		name = workTransform.GetComponent<TMP_InputField>().text;
		workTransform = FindChildTransform("QuestExplanation/Input");
		explanation = workTransform.GetComponent<TMP_InputField>().text;
		workQuest.SetQuest(name, explanation);

		//状態
		workTransform = FindChildTransform("Conditions/ToggleAppearance");
		if (IsToggleOn(workTransform)) workQuest.SetKey(Quest.KEY.POSSIBLE);
		workTransform = FindChildTransform("Conditions/ToggleNew");
		if (!IsToggleOn(workTransform)) workQuest.SetKey(Quest.KEY.NO_NEW);
		workTransform = FindChildTransform("Conditions/ToggleClear");
		if (IsToggleOn(workTransform)) workQuest.SetKey(Quest.KEY.CLEAR);

		return workQuest;
	}

	//子オブジェクトの検索
	Transform FindChildTransform(in string findName)
	{
		return settingObject.transform.Find(findName);
	}

	//Toggleコンポーネントのチェックの取得
	bool IsToggleOn(in Transform targetTransform)
	{
		return targetTransform.GetComponent<Toggle>().isOn;
	}
}
