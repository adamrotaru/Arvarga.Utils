using System;
using System.Collections.Generic;

namespace Arvarga.Utils.ServiceRepo
{
    /// <summary>
    /// A service that keeps together several 'smaller' services.
    /// </summary>
    public interface IAggregateService : IService
    {
        IList<IService> GetChildServices();
    }
}
