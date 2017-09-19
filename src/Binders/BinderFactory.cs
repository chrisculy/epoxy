using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Epoxy.Binders
{
    public static class BinderFactory
    {
        public static IBinder GetBinder(string language)
        {
            return s_binders.ContainsKey(language) ? s_binders[language] : null;
        }

        private static ReadOnlyDictionary<string, IBinder> s_binders = new ReadOnlyDictionary<string, IBinder>(new Dictionary<string, IBinder>() {
            { "c#", new CSharpBinder() },
        });
    }
}