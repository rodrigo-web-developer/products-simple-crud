using System.Reflection;

namespace SimpleCrud.Extensions
{
    public static class LinqExtensions
    {
        private static MethodInfo ToListAsyncMethod = null;

        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
        {
            if (ToListAsyncMethod == null)
            {
                return Task.FromResult(query.ToList());
            }

            return (Task<List<T>>)ToListAsyncMethod
                .MakeGenericMethod(typeof(T))
                .Invoke(null, new object[] { query, cancellationToken });
        }

        /// <summary>
        /// Use this method to register the ToListAsync method from an external library (e.g., Entity Framework)
        /// </summary>
        /// <param name="methodInfo">The implementation of ToListAsync</param>
        /// <exception cref="InvalidOperationException">throws exception when method is called twice.</exception>
        public static void RegisterToListAsyncMethod(MethodInfo methodInfo)
        {
            if (ToListAsyncMethod == null)
            {
                ToListAsyncMethod = methodInfo;
            }
            else
            {
                throw new InvalidOperationException("ToListAsync method is already registered.");
            }
        }
    }
}
