using System;
using System.Diagnostics;
using System.Web.Mvc;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace FoxTales.Infrastructure.MVCFramework.Filters
{
    /// <summary>
    ///     Filter used to track the time consumed processing a request.
    /// </summary>
    internal class SpeedProfilerFilter : ActionFilterAttribute
    {
        private Guid _requestId;
        private Stopwatch _stopwatch;

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _requestId = Guid.NewGuid();
            _stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _stopwatch.Stop();
            // bad naming convention x- introduced by Thomas, this is kept since we don't know if anyone are addressing these tags!
            filterContext.HttpContext.Response.AddHeader("X-BWS-ActionTimeElapsed", string.Format("{0} seconds", _stopwatch.Elapsed.TotalSeconds));
            filterContext.HttpContext.Response.AddHeader("X-BWS-RequestId", string.Format(_requestId.ToString(), _stopwatch.Elapsed.TotalSeconds));
        }
    }
}