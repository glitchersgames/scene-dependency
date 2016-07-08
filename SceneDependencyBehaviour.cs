using UnityEngine;
using System.Collections;

public interface ISceneDependency
{
	void SetSceneDependencyManager( ISceneDependencyManager dependencyManager );
}

public class SceneDependencyBehaviour : MonoBehaviour, ISceneDependency {

	#region Scene Dependency
	private bool m_DependencyReady = false;
	private bool m_DependencyReadyStateSent = false;
	private ISceneDependencyManager m_SceneDependencyManager;

	public void SetSceneDependencyManager( ISceneDependencyManager sceneDependencyManager )
	{
		m_SceneDependencyManager = sceneDependencyManager;
		SendDependencyReady();
	}

	protected void SetDependencyReady()
	{
		m_DependencyReady = true;
		SendDependencyReady();
	}

	protected void SendDependencyReady()
	{
		if( m_DependencyReadyStateSent == false && m_DependencyReady == true && m_SceneDependencyManager != null )
		{
			m_DependencyReadyStateSent = true;
			m_SceneDependencyManager.OnSceneDependencyReady( this as ISceneDependency );
		}
	}
	#endregion
}
