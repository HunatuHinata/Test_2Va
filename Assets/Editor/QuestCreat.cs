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
		// 子のパスデータのリスト
		public Quest m_getQuest;

		// 折りたたみが開いているかどうか
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
		// フォールドアウトを表示
		opened = EditorGUILayout.Foldout(opened, "折りたたみ");
		EditorGUI.indentLevel++;

		// 開いているとき
		if (opened)
		{
			// 1つ字下げする
			EditorGUI.indentLevel++;

			// 下の階層のラベルを表示
			EditorGUILayout.LabelField("下の階層");
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
			//(item).Add(new EditorGUILayout.LabelField("下の階層"););
		};
		list.itemsSource = Enumerable.Range(0, questSO.quests.Count()).ToList(); // itemsSourceの要素数がListViewの要素数になる。このケースでは中身は何でも良い
	}

	// パスを表示
	void ShowPaths(QuestSO path)
	{
		// 現在のパスに子がないときは終了
		if (path.quests.Count() == 0) return;

		// インデントレベルを一つ上げる
		EditorGUI.indentLevel++;

		// 現在のインデントレベルを保存
		int currentIndentLevel = EditorGUI.indentLevel;

		// パスの子を一つずつ処理
		for (int i = 0; i < path.quests.Count; i++)
		{
#if false
			using (new EditorGUILayout.HorizontalScope())
			{
				// トグルを表示
				path.children[i].toggle = GUILayout.Toggle(path.children[i].toggle, "", GUILayout.Width(15f));

				// 折りたたみを表示
				path.children[i].foldout = EditorGUILayout.Foldout(path.children[i].foldout, path.children[i].Value);
			}
			// 開いているとき
			if (path.children[i].foldout)
			{
				// さらに子のパスを表示
				//ShowPaths(path.children[i]);
				// インデントレベルを戻す
				EditorGUI.indentLevel = currentIndentLevel;
			} 
#endif
		}
	}
}
