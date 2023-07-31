using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator
{
    public class Wrapper
    {
        public Wrapper()
        { 
            
        }

        public object this[Object obj , string propertyName]
        {
            get { return obj.GetType().GetProperty(propertyName).GetValue(obj, null); }
            set 
            {
                Type type = obj.GetType().GetProperty(propertyName).GetValue(obj, null).GetType();
                obj.GetType().GetProperty(propertyName)
                    .SetValue(obj, Convert.ChangeType(value, type), null);
            }
        }

    }
}
