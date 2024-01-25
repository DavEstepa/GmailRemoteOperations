using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailReader.Domain.Utilities
{
    public static class ProtectedExecutionMethod
    {
        public static async Task<TResult?> ExecuteAsync<TInput, TResult>(Func<TInput?, Task<TResult?>> methodReference, TInput? input)
        {
			try
			{
				var result = await methodReference(input);
                return result;

            }
			catch (Exception)
			{
				return default(TResult);
			}
        }

        public static async Task<bool> ExecuteAsync(Func<Task> methodReference)
        {
            try
            {
                await methodReference();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
