using System;
using System.Linq.Expressions;
using System.Reflection;


namespace CompassPlus
{
    public static class Utils
    {
        public static bool DoubleEquals(double left, double right, double acceptableDifference)
        {
            if (acceptableDifference < 0)
            {
                throw new ArgumentOutOfRangeException("acceptableDifference");
            }
            double difference = Math.Abs(left * acceptableDifference);
            return Math.Abs(left - right) <= difference;
        }

        public static T TryParse<T>(string value, TryParseHandler<T> handler) where T : struct
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            T result;
            if (string.IsNullOrEmpty(value) == false && handler(value, out result))
            {
                return result;
            }
            return default(T);
        }

        public static T? TryParseNullable<T>(string value, TryParseHandler<T> handler) where T : struct
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            T result;
            if (handler(value, out result))
            {
                return result;
            }
            return null;
        }

        public delegate bool TryParseHandler<T>(string value, out T result);

        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }
            return property.Name;
        }
    }
}
