using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Customs
{
    public static class SystemCustoms
    {
        public static double SecondsBetween(this DateTime dt0, DateTime dt1)
        {
            return Math.Abs((dt1.ToOADate() - dt0.ToOADate()) * 86400);
        }
    }
}
