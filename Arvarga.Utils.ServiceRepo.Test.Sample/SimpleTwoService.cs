/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{
    public class SimpleTwoService : ISimpleTwoService
    {
        public void Init(IServiceRepository serviceRepository)
        {
        }

        public string TwoMethodOne(string param)
        {
            return $"TwoMethodOne: {param}";
        }
    }
}
