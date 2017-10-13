using System;

namespace Epoxy.Utility
{
	public class Scope : IDisposable
	{
		public Scope(Action onDispose)
		{
			m_onDispose = onDispose;
		}

		public void Dispose()
		{
			m_onDispose();
		}

		private Action m_onDispose;
	}
}
