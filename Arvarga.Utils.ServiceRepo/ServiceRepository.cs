/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arvarga.Utils.ServiceRepo
{
    internal class ServiceInfo
    {
        public IService _service;
        public bool _initedFlag;
    }

    /// <summary>
    /// Repository for (micro)services.
    /// </summary>
    public class ServiceRepository : IServiceRepositoryFull
    {
        enum InitializeState
        {
            NotInitialized = 0,
            SomeAdded,
            InitCallsStarted,
            InitCallsComplete
        }
        private InitializeState myInitializeState = InitializeState.NotInitialized;

        // Dictionary with services, key is full name of the interface (if a service has multiple IService interfaces, it may appear more than once)
        private Dictionary<string, ServiceInfo> myDict = new Dictionary<string, ServiceInfo>();
        private object myDictLock = new object();
        private ServiceInfo myCurrentlyInitializingService;

        /// <summary>
        /// Store the service instance provided.  InitServices() is needed at the end.
        /// </summary>
        public void Add(IService service)
        {
            lock (myDictLock)
            {
                if (myInitializeState >= InitializeState.InitCallsStarted)
                {
                    throw new ApplicationException($"ServiceRepository already initialized, cannot add more services {this}");
                }

                // use the same serviceInfo object
                ServiceInfo serviceInfo = new ServiceInfo { _service = service, _initedFlag = false };
                // a service can have one or more IService interfaces
                IEnumerable<Type> serviceIfaces = GetInterfaceNamesOfService(service.GetType());
                foreach (Type type in serviceIfaces)
                {
                    myDict[type.FullName] = serviceInfo;
                }

                myInitializeState = InitializeState.SomeAdded;

                // handle aggregate services, add child services (recursive)
                if (service is IAggregateService)
                {
                    AddServices((service as IAggregateService).GetChildServices());
                }
            }
        }

        private IService Get(string serviceTypeName)
        {
            IService ret = null;
            lock (myDictLock)
            {
                if (myInitializeState < InitializeState.InitCallsStarted)
                {
                    throw new ApplicationException($"ServiceRepository not yet initialized, cannot retrieve services {this}");
                }
                if (!myDict.ContainsKey(serviceTypeName))
                {
                    throw new ApplicationException($"Configuration error: Service '{serviceTypeName}' not found in service repository {this}");
                }
                ret = myDict[serviceTypeName]._service;
            }   
            return ret;
        }

        /*private IService Get(Type serviceType)
        {
            return Get(serviceType.FullName);
        }*/

        /// <summary>
        /// Retrieve a service instance, by its IService type.
        /// Example:
        /// IDoesSomething doesSomething = ServiceRepository.Get<IDoesSomething>();
        /// </summary>
        /// <typeparam name="T">Type of the service</typeparam>
        /// <param name="mustBeUsable">During init, not all serivces are inited yet, and usually that is fine.  
        /// However, if there is strict need for a service to be used during init, it must be already inited, enforce it with True here (the order matters!)</param>
        /// <returns></returns>
        public T Get<T>(bool mustBeUsable = false) where T : IService
        {
            string serviceTypeName = typeof(T).FullName;
            T service = (T)Get(serviceTypeName);
            if (mustBeUsable)
            {
                // make sure the service is in the list (throws if not)
                lock (myDictLock)
                {
                    // make sure it is inited
                    if (!myDict.ContainsKey(serviceTypeName) || !myDict[serviceTypeName]._initedFlag)
                    {
                        throw new ApplicationException($"Service '{serviceTypeName}' not yet initialized! (while initing {myCurrentlyInitializingService?._service.GetType().FullName}) {this}");
                    }
                }
            }
            return service;
        }

        /// <summary>
        /// Store the service instances provided, and call Init() on them in the given order.
        /// Should be called only once.
        /// </summary>
        /// <param name="services"></param>
        public void InitWithServices(IList<IService> services)
        {
            lock (myDictLock)
            {
                myDict.Clear();
            }

            AddServices(services);

            InitServices();
        }

        /// <summary>
        /// Store the service instances provided.  InitServices() is needed at the end.
        /// </summary>
        /// <param name="services"></param>
        public void AddServices(IList<IService> services)
        {
            // store services in Dict (in the order given)
            lock (myDictLock)
            { 
                foreach(IService serv in services)
                {
                    Add(serv);
                }
            }
        }

        /// <summary>
        /// Call Init() on the added service instances, in the order of their addition.  
        /// Should be called only once.
        /// If one service Init throws, the other are invoked nontheless, but an exception is thrown.
        /// </summary>
        /// <param name="services"></param>
        public void InitServices()
        {
            lock (myDictLock)
            {
                if (myInitializeState >= InitializeState.InitCallsStarted)
                {
                    throw new ApplicationException($"ServiceRepository already initialized {this}");
                }
                myInitializeState = InitializeState.InitCallsStarted;
            }

            Dictionary<string, ServiceInfo> dictLocalCopy = null;
            lock (myDictLock)
            {
                dictLocalCopy = myDict;
            }
            // init the services (in the order given)
            Exception exFromServiceInit = null;
            foreach(ServiceInfo serv in dictLocalCopy.Values)
            {
                lock (serv)
                {
                    if (!serv._initedFlag)
                    {
                        myCurrentlyInitializingService = serv;
                        try
                        {
                            // invoke Init
                            serv._service.Init(this);
                            serv._initedFlag = true;
                        }
                        catch (Exception ex)
                        {
                            if (exFromServiceInit == null)  // keep only the first exception
                            {
                                exFromServiceInit = new ApplicationException($"Exception during initializing service '{serv._service.GetType().Name}': {ex.Message} this: {this.ToString()}", ex);
                            }
                        }
                        myCurrentlyInitializingService = null;
                    }
                }
            }

            lock (myDictLock)
            {
                myInitializeState = InitializeState.InitCallsComplete;
            }

            if (exFromServiceInit != null)
            {
                // there was at least an exception from a service init, throw it
                throw exFromServiceInit;
            }
        }

        /// <summary>
        /// Create a service instance from a DLL through reflection, and add it to the repository.  InitServices() is needed at the end.
        /// It is used to avoid compile-time dependencies to the service implementations.
        /// </summary>
        /// <example>
        /// AddServiceInstanceFromDll("HBO.ApplicationServer.BusinessLogic.dll", "HBO.ApplicationServer.Utility.CacheManagerService");
        /// </example>
        /// <param name="dllName"></param>
        /// <param name="implementationTypeFullName"></param>

        public void AddServiceInstanceFromDll(string dllName, string implementationTypeFullName)
        {
            object impl = Activator.CreateInstanceFrom(dllName, implementationTypeFullName).Unwrap();
            IService implServ = impl as IService; 
            if (implServ != null)
            {
                Add(implServ);
            }
        }

        private IEnumerable<Type> GetInterfaceNamesOfService(Type serviceType)
        {
            return serviceType.GetInterfaces().Where(t => t.GetInterfaces().Where(t2 => t2.Equals(typeof(IService))).Any());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"ServiceRepo: {myInitializeState}, {myDict.Count} services (");
            foreach (string itype in myDict.Keys)
            {
                string lasttype = itype.Split('.').Last();
                sb.Append(lasttype);
                if (myDict[itype]._initedFlag) sb.Append("+"); else sb.Append("-");  // inited flag indicator
                sb.Append(", ");
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
