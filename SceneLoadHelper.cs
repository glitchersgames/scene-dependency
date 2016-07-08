using UnityEngine;
using System.Collections;

public class SceneLoadHelper : MonoBehaviour {

	#region Variables
	private System.Action m_OnComplete;
	#endregion

	#region Lifecycle
	private void Awake()
	{
		Object.DontDestroyOnLoad( gameObject );
	}

	private void OnLevelWasLoaded( int levelID )
	{
		SceneLoadManager.OnSceneReady += OnSceneReady;
	}

	private void OnSceneReady()
	{
		SceneLoadManager.OnSceneReady -= OnSceneReady;
		m_OnComplete();
		Destroy( gameObject );
	}
	#endregion

	public static void Create( System.Action onComplete )
	{
		GameObject sceneHelperObject = new GameObject("SceneLoadHelper");
		SceneLoadHelper helper = sceneHelperObject.AddComponent<SceneLoadHelper>();

		helper.m_OnComplete = onComplete;
	}
}
