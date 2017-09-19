using System.Collections.Generic;

namespace Epoxy.Api
{
    public interface ApiContainer
    {
        List<Function> Functions { get; }
        List<NamedElement> Variables { get; }
    }
}