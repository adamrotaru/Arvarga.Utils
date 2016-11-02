﻿using System;
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