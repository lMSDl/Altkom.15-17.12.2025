using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class LimiterFilter : IAsyncActionFilter
    {

        private int _limitPerMinute;
        public LimiterFilter(int limitPerMinute)
        {
            _limitPerMinute = limitPerMinute;
        }

        private int _counter;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(_counter >= _limitPerMinute)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status429TooManyRequests);
                return;
            }

            Interlocked.Increment(ref _counter);
            try
            {
                await next();
            }
            finally
            {
                // Decrement the counter after a minute
                _ = Task.Delay(TimeSpan.FromMinutes(1)).ContinueWith(_ => Interlocked.Decrement(ref _counter));
            }



        }
    }
}
