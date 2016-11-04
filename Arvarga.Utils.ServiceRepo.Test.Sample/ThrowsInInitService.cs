/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

﻿﻿using System;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{
    public class ThrowsInInitService : IThrowsInInitService
    {
        public void Init(IServiceRepository serviceRepository)
        {
            throw new FromInitException();
        }
    }
}
