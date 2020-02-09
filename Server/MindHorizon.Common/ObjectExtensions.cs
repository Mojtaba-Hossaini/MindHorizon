using System;

namespace MindHorizon.Common
{
    public static class ObjectExtensions
    {
        public static void CheckArgumentIsNull(this object o, string name)
        {
            if (o == null)
                throw new ArgumentNullException(name);
        }
    }
}
