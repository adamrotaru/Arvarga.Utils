/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

﻿﻿using System;

namespace Arvarga.Utils.ServiceRepo
{
    /// <summary>
    /// Repository for (micro)services, read-only interface.
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
    }
}
