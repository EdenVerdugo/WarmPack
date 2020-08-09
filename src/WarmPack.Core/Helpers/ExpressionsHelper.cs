using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WarmPack.Helpers
{
    public static class ExpressionsHelper
    {
        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            /*
             * este era mi codigo.
            var type = property.Body.GetType();

            var propertyName = "";
            if (type == typeof(UnaryExpression))
            {
                var expression = (UnaryExpression)property.Body;
                propertyName = (expression.Operand as MemberExpression)?.Member.Name;
            }
            else if (type == typeof(MemberExpression))
            {
                var expression = (MemberExpression)property.Body;
                propertyName = expression?.Member.Name;
            }
            else
            {
                var x = (MemberExpression)property.Body;
                propertyName = x?.Member.Name;
            }
            */

            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            //Debug.Assert(memberExpression != null, "Please provide a lambda expression like 'n => n.PropertyName'");

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;

                return propertyInfo.Name;
            }

            return null;
        }
    }
}
