using System.Collections.Generic;

namespace Arvarga.Utils.ServiceRepo
{
    /// <summary>
    /// Repository for (micro)services. 
    /// </summary>
    public interface IServiceRepository
    {
        /// <summary>
        /// Retrieve a service instance, by its IService type.
        /// Example:
        /// IDoesSomething doesSomething = ServiceRepository.Get<IDoesSomething>();
        /// </summary>
        /// <typeparam name="T">Type of the service</typeparam>
        /// <param name="mustBeUsable">During init, not all serivces are inited yet, and usually that is fine.  
        /// However, if there is strict need for a service to be used during init, it must be already inited, enforce it with True here (the order matters!)</param>
        /// <returns></returns>
        T Get<T>(bool mustBeUsable = false) where T : IService;

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
