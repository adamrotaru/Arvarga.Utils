/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

using System.Collections.Generic;

namespace Arvarga.Utils.ServiceRepo
{
    /// <summary>
    /// Repository for (micro)services, full access interface.
    /// </summary>
    public interface IServiceRepositoryFull : IServiceRepository
    {
        /// <summary>
        /// Store the service instance provided, and call Init() on them in the given order
        /// </summary>
        /// <param name="services"></param>
        void InitWithServices(IList<IService> services);

        /// <summary>
        /// Store the service instance provided.  InitServices() is needed at the end.
        /// </summary>
        void Add(IService service);

        /// <summary>
        /// Store the service instances provided.  InitServices() is needed at the end.
        /// </summary>
        /// <param name="services"></param>
        void AddServices(IList<IService> services);

        /// <summary>
        /// Create a service instance from a DLL through reflection, and add it to the repository.  InitServices() is needed at the end.
        /// It is used to avoid compile-time dependencies to the service implementations.
        /// </summary>
        /// <example>
        /// AddServiceInstanceFromDll("HBO.ApplicationServer.BusinessLogic.dll", "HBO.ApplicationServer.Utility.CacheManagerService");
        /// </example>
        /// <param name="dllName"></param>
        /// <param name="implementationTypeFullName"></param>
        void AddServiceInstanceFromDll(string dllName, string implementationTypeFullName);

        /// <summary>
        /// Call Init() on the added service instances, in the order of their addition.  
        /// Should be called only once.
        /// </summary>
        /// <param name="services"></param>
        void InitServices();
    }
}
