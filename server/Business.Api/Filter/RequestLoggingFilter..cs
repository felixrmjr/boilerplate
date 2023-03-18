using Business.Domain.Interfaces.Repositories;
using Business.Domain.Models.Others;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;

namespace Business.Api.Filter
{
    public class RequestLoggingFilter : IAsyncActionFilter
    {
        private readonly ILogRequestRepository _logRequestRepository;
        private Stopwatch _stopWatch;

        public RequestLoggingFilter(ILogRequestRepository logRequestRepository)
        {
            _logRequestRepository = logRequestRepository;
            _stopWatch = new Stopwatch();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Path.Value.Contains("/api/"))
            {
                _stopWatch.Reset();
                _stopWatch.Start();
            }

            if (context.HttpContext.Request.Path.Value.Contains("/api/"))
            {
                _stopWatch.Stop();
                ClaimsPrincipal user = context.HttpContext.User;
                long executionTime = _stopWatch.ElapsedMilliseconds;
                PathString rota = context.HttpContext.Request.Path;
                string controller = context.Controller.ToString().Replace("Business.Api.Controllers.", string.Empty);
                string httpVerb = context.HttpContext.Request.Method;
                string queryString = context.HttpContext.Request.QueryString.ToString();
                string userId = user.Claims.Count() > 0 ? user.Identities.FirstOrDefault().Claims.FirstOrDefault(w => w.Type == "Id").Value : string.Empty;
                string userIp = context.HttpContext.Connection.RemoteIpAddress.ToString();
                int statusCode = context.HttpContext.Response.StatusCode;
                string requestBody = string.Empty;
                context.HttpContext.Request.EnableBuffering();
                using (StreamReader stream = new StreamReader(context.HttpContext.Request.Body))
                {
                    stream.BaseStream.Position = 0;
                    requestBody = await stream.ReadToEndAsync();
                }
                if (userIp == "::1")
                    userIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == AddressFamily.InterNetwork).ToString();
                LogRequest logRequisicao = new LogRequest(rota, controller, httpVerb, queryString, requestBody, userId, userIp, statusCode, executionTime);
                await _logRequestRepository.Post(logRequisicao);

                await next();
            }
        }
    }

    public static class HttpRequestExtension
    {
        public static string GetHeader(this HttpRequest request, string key)
        {
            return request.Headers.FirstOrDefault(x => x.Key == key).Value.FirstOrDefault();
        }
    }
}
