using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AttiLA.Data
{
    internal class Utils<T>
    {
        private Utils() { }

        /// <summary>
        /// Utility to get a string with the name of a property
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="memberExpr">Lambda expression to reach the property</param>
        /// <returns></returns>
        internal static string MemberName<TMember>(Expression<Func<T, TMember>> memberExpr)
        {
            return ((MemberExpression)memberExpr.Body).Member.Name;
        }

        /// <summary>
        /// Utility to get the name of the collection associated to the entity type
        /// </summary>
        /// <returns></returns>
        internal static string CollectionName
        {
            get { return typeof(T).Name.ToLower() + "s"; }
        }

    }
}
