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
    /// <summary>
    /// A service that uses other services.
    /// </summary>
    public interface ICompoundThreeService : IService
    {
        string ThreeMethodOne(string param);
    }
}
