using System.CodeDom;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestCreat : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
	[SerializeField]
	private QuestSO questSO;
	private VisualElement root;

	const int NUM = 3;
	bool opened = false;

	private class QuestFoldout {
		// �q�̃p�X�f�[�^�̃��X�g
		public Quest m_getQuest;

		// �܂肽���݂��J���Ă��邩�ǂ���
		public bool m_bFoldout;
	};

	[MenuItem("Window/CreatQuests")]
    public static void ShowExample()
    {
        QuestCreat wnd = GetWindow<QuestCreat>();
        wnd.titleContent = new GUIContent("QuestCreat");
    }

    public void CreateGUI()
    {
        root = rootVisualElement;

		VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
		root.Add(labelFromUXML);

		Initialize();
	}

	private void OnGUI()
	{
		// �t�H�[���h�A�E�g��\��
		opened = EditorGUILayout.Foldout(opened, "�܂肽����");
		EditorGUI.indentLevel++;

		// �J���Ă���Ƃ�
		if (opened)
		{
			// 1����������
			EditorGUI.indentLevel++;

			// ���̊K�w�̃��x����\��
			EditorGUILayout.LabelField("���̊K�w");
		}

		Button createButton = root.Query<Button>("CreatButton").First();
		createButton.clicked += () =>
		{
			Quest work = new Quest();

			string name = root.Query<TextField>("QuestName").First().text;
			string detail = root.Query<TextField>("QuestDetail").First().text;

			bool[] checks = new bool[NUM];
			checks[0] = root.Query<Toggle>("AvailableOrders").First().value;
			checks[1] = root.Query<Toggle>("NEW").First().value;
			checks[2] = root.Query<Toggle>("Clear").First().value;

			byte flag = 0;
			if (checks[0]) flag += 1;
			if (checks[1]) flag += 2;
			if (checks[2]) flag += 4;

			work.SetQuest(name, detail);
		};
	}

	void Initialize()
	{
		ListView list = root.Query<ListView>("list").First();
		list.makeItem = () => new Foldout();
		list.bindItem = (item, i) => {
			((Foldout)item).text = $"{ questSO.quests[i].GetQuest().name}";
			EditorGUI.indentLevel++;
			//(item).Add(new EditorGUILayout.LabelField("���̊K�w"););
		};
		list.itemsSource = Enumerable.Range(0, questSO.quests.Count()).ToList(); // itemsSource�̗v�f����ListView�̗v�f���ɂȂ�B���̃P�[�X�ł͒��g�͉��ł��ǂ�
	}

	// �p�X��\��
	void ShowPaths(QuestSO path)
	{
		// ���݂̃p�X�Ɏq���Ȃ��Ƃ��͏I��
		if (path.quests.Count() == 0) return;

		// �C���f���g���x������グ��
		EditorGUI.indentLevel++;

		// ���݂̃C���f���g���x����ۑ�
		int currentIndentLevel = EditorGUI.indentLevel;

		// �p�X�̎q���������
		for (int i = 0; i < path.quests.Count; i++)
		{
#if false
			using (new EditorGUILayout.HorizontalScope())
			{
				// �g�O����\��
				path.children[i].toggle = GUILayout.Toggle(path.children[i].toggle, "", GUILayout.Width(15f));

				// �܂肽���݂�\��
				path.children[i].foldout = EditorGUILayout.Foldout(path.children[i].foldout, path.children[i].Value);
			}
			// �J���Ă���Ƃ�
			if (path.children[i].foldout)
			{
				// ����Ɏq�̃p�X��\��
				//ShowPaths(path.children[i]);
				// �C���f���g���x����߂�
				EditorGUI.indentLevel = currentIndentLevel;
			} 
#endif
		}
	}
}
