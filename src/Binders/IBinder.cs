using Epoxy.Api;

namespace Epoxy.Binders
{
	public abstract class IBinder
	{
		public IBinder(BinderConfiguration configuration)
		{
			Configuration = configuration;
		}

		public abstract void GenerateNativeBindings(Graph graph);

		public abstract void GenerateLanguageBindings(Graph graph);

		protected BinderConfiguration Configuration { get; private set; }
	}
}
