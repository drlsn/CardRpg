using System;

namespace Core.Basic
{
    public static class TryCatch
    {
        public static TResult Run<TResult>(Func<TResult> execute)
        {
            try
            {
                return execute();
            }
            catch (Exception ex) 
            {
                return default;
            }
        }
    }
}
