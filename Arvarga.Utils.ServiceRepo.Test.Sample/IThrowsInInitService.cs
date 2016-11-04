/*
 * See the LICENSE file for copyright and license conditions.
 * (c) 2016 Adam Rotaru
 */

﻿﻿using System;

namespace Arvarga.Utils.ServiceRepo.Test.Sample
{
    public interface IThrowsInInitService : IService
    {
    }

    public class FromInitException : Exception
    {
    }
}
