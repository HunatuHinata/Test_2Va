using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButton : MonoBehaviour
{
	[Header("次に行くシーンの名前")]
	[SerializeField] string m_sceneName;

    public void NextScene()
	{
		SceneManager.LoadScene(m_sceneName.ToString());
	}
}
