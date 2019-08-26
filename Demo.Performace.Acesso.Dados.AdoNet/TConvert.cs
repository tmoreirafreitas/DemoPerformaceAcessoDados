using System;
using System.ComponentModel;

namespace Demo.Performace.Acesso.Dados.AdoNet
{
    public static class TConvert
    {
        public static T ChangeType<T>(object value)
        {
            return (T) ChangeType(typeof(T), value);
        }

        public static object ChangeType(Type t, object value)
        {
            var tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value);
        }
    }
}
