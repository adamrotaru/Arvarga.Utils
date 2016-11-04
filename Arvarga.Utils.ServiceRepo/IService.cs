/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

﻿using System;

namespace Arvarga.Utils.ServiceRepo
{
    /// <summary>
    /// Interface for all internal (micro)services.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Initialization, called after the service instance has been added to the repository, and all service instances are available, but not yet initialized.
        /// It should retrieve the services it needs and store a reference to them. 
        /// </summary>
        /// <param name="serviceRepository"></param>
        void Init(IServiceRepository serviceRepository);
    }
}
