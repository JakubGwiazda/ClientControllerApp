using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientControllerApp
{
    public static class EnumHelper
    {

        public static string GetDescription<T>(this T enumValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return null;
            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo != null)
            {
                var attributs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if(attributs!= null && attributs.Length > 0)
                {
                    description = ((DescriptionAttribute)attributs[0]).Description;
                }
            }
            return description;
        }

    }
}
