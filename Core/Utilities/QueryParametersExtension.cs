using System;

namespace Core.Utilities
{
    /// <summary>
    /// Static method called while implementing search functionality
    /// See for example AdminRepository/GetAllUsers for more details
    /// </summary>
    public static class QueryParametersExtension
    {
        public static bool HasQuery(this QueryParameters queryParameters)
        {
            return !String.IsNullOrEmpty(queryParameters.Query);
        }
    }
}