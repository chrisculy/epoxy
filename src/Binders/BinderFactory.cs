using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Epoxy.Binders
{
    public static class BinderFactory
    {
        public static IBinder GetBinder(BinderConfiguration configuration)
        {
            if (!s_binders.ContainsKey(configuration.Language))
                return null;
            
            return (IBinder)Activator.CreateInstance(s_binders[configuration.Language], configuration);
        }

        private static ReadOnlyDictionary<string, Type> s_binders = new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>() {
            { "c#", typeof(CSharpBinder) },
        });
    }
}