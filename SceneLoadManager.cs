using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public interface ISceneDependencyManager
{
	void OnSceneDependencyReady( ISceneDependency dependency );
}


public class SceneLoadManager : MonoBehaviour, ISceneDependencyManager {

	#region Event
	public delegate void SceneReadyEvent();
	public static event SceneReadyEvent OnSceneReady;
	#endregion

	#region References
	public MonoBehaviour[] m_SceneDependencies;
	#endregion

	#region Variables
	private int m_DependencyReadyCount;

	public static bool IsSceneReady
	{
		get;
		private set;
	}
	#endregion

	#region Lifecycle
	private void Awake()
	{
		IsSceneReady = false;
		m_DependencyReadyCount = 0;

		foreach( MonoBehaviour sceneDependencyBehaviour in m_SceneDependencies )
		{
			ISceneDependency dependency = (sceneDependencyBehaviour as ISceneDependency);
			if( dependency != null )
			{
				dependency.SetSceneDependencyManager( this as ISceneDependencyManager );
			}
			else
			{
				#if UNITY_EDITOR
				Debug.Break();
				#endif

				throw new System.InvalidCastException( string.Format("Object [{0}] does not conform to the ISceneDependency interface", sceneDependencyBehaviour) );
			}
		}
	}
	#endregion

	#region Dependency Methods
	public void OnSceneDependencyReady( ISceneDependency dependency )
	{
		m_DependencyReadyCount++;

		if( m_DependencyReadyCount == m_SceneDependencies.Length )
		{
			OnAllDependenciesReady();
		}
	}

	private void OnAllDependenciesReady()
	{
		IsSceneReady = true;

		if( OnSceneReady != null )
		{
			OnSceneReady();
		}
	}
	#endregion

	#region Static
	public static void LoadScene( string sceneName, System.Action onComplete )
	{
		IsSceneReady = false;

		SceneLoadHelper.Create( onComplete );

		SceneManager.LoadScene( sceneName );

	}
	#endregion
}
