using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Demo.Performace.Acesso.Dados.AdoNet
{
    public static class DataReaderExtensions
    {
        public static IList<T> MapToList<T>(this IDataReader dr) where T : new()
        {
            var retVal = new List<T>();
            var entity = typeof(T);
            try
            {
                if (dr != null)
                {
                    var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    var propDic = props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    while (dr.Read())
                    {
                        var newObject = new T();
                        for (var i = 0; i < dr.FieldCount; i++)
                        {
                            if (!propDic.ContainsKey(dr.GetName(i).ToUpper())) continue;
                            var info = propDic[dr.GetName(i).ToUpper()];
                            if (info == null || !info.CanWrite) continue;
                            var val = dr.GetValue(i);
                            info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
                        }
                        retVal.Add(newObject);
                    }
                }
            }
            catch (Exception ex)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }

            return retVal;
        }

        public static T MapToSingle<T>(this IDataReader dr) where T : new()
        {
            var retVal = new T();
            var entity = typeof(T);
            try
            {
                if (dr != null)
                {
                    var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    var propDic = props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    dr.Read();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        if (!propDic.ContainsKey(dr.GetName(i).ToUpper())) continue;
                        var info = propDic[dr.GetName(i).ToUpper()];
                        if (info == null || !info.CanWrite) continue;
                        var val = dr.GetValue(i);
                        info.SetValue(retVal, (val == DBNull.Value) ? null : val, null);
                    }
                }
            }
            catch (Exception ex)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }

            return retVal;
        }
    }
}