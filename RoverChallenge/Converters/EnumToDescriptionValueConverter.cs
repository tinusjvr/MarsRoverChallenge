﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace RoverChallenge.Converters
{
    public class EnumToDescriptionValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum myEnum = (Enum)value;

            if (myEnum == null)
            {
                return null;
            }

            string description = GetEnumDescription(myEnum);

            if (string.IsNullOrEmpty(description) == false)
            {
                return description;
            }

            return myEnum.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static string GetEnumDescription(Enum enumObj)
        {
            if (enumObj == null)
            {
                return string.Empty;
            }
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }

    }
}
