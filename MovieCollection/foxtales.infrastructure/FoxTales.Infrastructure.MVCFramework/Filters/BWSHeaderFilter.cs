using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Filters;

namespace FoxTales.Infrastructure.MVCFramework.Filters
{
    /// <summary>
    /// Filter used to copy the attribute BWS-ApplicationLink from Request to Response. This should be appended to Get methods.
    /// </summary>
    public class BWSHeaderFilter : ActionFilterAttribute
    {
        private const string HeaderAttributeName = "BWS-ApplicationLink";
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Request.Method == HttpMethod.Get)
            {
                string[] enumerable = new string[1];

                try
                {
                    if (actionExecutedContext.Request.Headers != null)
                    {
                        IEnumerable<string> applicationLink = actionExecutedContext.Request.Headers.GetValues(HeaderAttributeName);
                        enumerable = applicationLink as string[] ?? applicationLink.ToArray();
                    }
                }
                catch (Exception)
                {
                    enumerable = new[] {"None"};
                }
                finally
                {
                    if (actionExecutedContext.Response.Headers != null)
                    {
                        actionExecutedContext.Response.Headers.Add("Access-Control-Expose-Headers", HeaderAttributeName);
                        actionExecutedContext.Response.Headers.Add(HeaderAttributeName, enumerable);
                    }
                }
            }
            else
            {
                base.OnActionExecuted(actionExecutedContext);
            }
        }
    }
}