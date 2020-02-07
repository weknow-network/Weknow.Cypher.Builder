using System;

namespace Weknow
{
    public interface ICypherNamingConfig
    {
        CypherNamingConvention Convention { get;  }
        NamingConventionAffects ConventionAffects { get;  }
        IPluralization Pluralization { get;  }
    }
}