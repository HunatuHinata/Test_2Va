using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class ItemCreat
{
	[SerializeField] GameObject m_settingObject;

	//取得
	public Quest GetItem()
	{
		Quest quest = null;
		quest = GetInputContentAcquisition();

		if (quest.GetQuest().name == string.Empty || quest.GetQuest().detail == string.Empty) return null;

		return quest;
	}

	//入力Itemをリセット
	public void ResetsInputItem()
	{
		Transform workTransform;

		//名前＆説明
		workTransform = FindChildTransform("QuestName/Input");
		workTransform.GetComponent<TMP_InputField>().text = "";
		workTransform = FindChildTransform("QuestExplanation/Input");
		workTransform.GetComponent<TMP_InputField>().text = "";

		//状態
		workTransform = FindChildTransform("Conditions/ToggleAppearance");
		workTransform.GetComponent<Toggle>().isOn = false;
		workTransform = FindChildTransform("Conditions/ToggleNew");
		workTransform.GetComponent<Toggle>().isOn = true;
		workTransform = FindChildTransform("Conditions/ToggleClear");
		workTransform.GetComponent<Toggle>().isOn = false;
	}

	//入力内容を出す
	Quest GetInputContentAcquisition()
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

	//内容をセット
	public void SetContents(in Quest quest)
	{
		Transform workTransform;

		//名前＆説明
		workTransform = FindChildTransform("QuestName/Input");
		workTransform.GetComponent<TMP_InputField>().text = quest.GetQuest().name;
		workTransform = FindChildTransform("QuestExplanation/Input");
		workTransform.GetComponent<TMP_InputField>().text = quest.GetQuest().detail;

		//状態
		workTransform = FindChildTransform("Conditions/ToggleAppearance");
		workTransform.GetComponent<Toggle>().isOn = quest.IsKey(Quest.KEY.POSSIBLE);
		workTransform = FindChildTransform("Conditions/ToggleNew");
		workTransform.GetComponent<Toggle>().isOn = !quest.IsKey(Quest.KEY.NO_NEW);
		workTransform = FindChildTransform("Conditions/ToggleClear");
		workTransform.GetComponent<Toggle>().isOn = quest.IsKey(Quest.KEY.CLEAR);
	}

	//子オブジェクトの検索
	Transform FindChildTransform(in string findName)
	{
		return m_settingObject.transform.Find(findName);
	}

	//Toggleコンポーネントのチェックの取得
	bool IsToggleOn(in Transform targetTransform)
	{
		return targetTransform.GetComponent<Toggle>().isOn;
	}
}
