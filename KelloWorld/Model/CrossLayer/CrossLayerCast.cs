using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KelloWorld.Model.CrossLayer
{
    public static class CrossLayerCast
    {
        /// <summary>
        /// Convert one entity to another by copying from public getters properties to new target entity public setter properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <example> myBlog.Cast<BlogDto>() </example>
        /// <param name="myobj"></param>
        /// <returns>new instance of type `T`</returns>
        public static T Cast<T>(this Object myobj)
        {
            Type sourceType = myobj.GetType();
            Type targetType = typeof(T);
            var result = Activator.CreateInstance(targetType, false);
            var sourceProperties = from src in sourceType.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                                   where src.MemberType == MemberTypes.Property && ((PropertyInfo)src).CanRead
                                   select (PropertyInfo)src;
            var targetProperties = from trg in targetType.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                                   where trg.MemberType == MemberTypes.Property && ((PropertyInfo)trg).CanWrite
                                   select (PropertyInfo)trg;

            var members = (from src in sourceProperties
                           join trg in targetProperties on src.Name equals trg.Name
                           select new { src, trg });
            object value;
            foreach (var pair in members)
            {
                value = pair.src.GetValue(myobj, null);
                pair.trg.SetValue(result, value, null);
            }
            return (T)result;
        }
    }
}
