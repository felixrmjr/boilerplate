using Business.Domain.Interfaces.Repositories;
using Business.Domain.Models.Others;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Business.Api.Filter
{
    public class RequestLoggingFilter : IActionFilter
    {
        private readonly ILogRequestRepository _logRequestRepository;
        private Stopwatch _stopWatch;

        public RequestLoggingFilter(ILogRequestRepository logRequestRepository)
        {
            _logRequestRepository = logRequestRepository;
            _stopWatch = new Stopwatch();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Path.Value.Contains("/api/"))
            {
                _stopWatch.Reset();
                _stopWatch.Start();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.Path.Value.Contains("/api/"))
            {
                _stopWatch.Stop();
                long executionTime = _stopWatch.ElapsedMilliseconds;
                PathString rota = context.HttpContext.Request.Path;
                string controller = context.Controller.ToString().Replace("Business.Api.Controllers.", string.Empty);
                string httpVerb = context.HttpContext.Request.Method;
                string queryString = context.HttpContext.Request.QueryString.ToString();
                string userId = context.HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault(w => w.Type == "Id").Value;
                string userIp = context.HttpContext.Connection.RemoteIpAddress.ToString();
                int statusCode = context.HttpContext.Response.StatusCode;
                string requestBody = string.Empty;
                using (StreamReader stream = new StreamReader(context.HttpContext.Request.Body))
                {
                    stream.BaseStream.Position = 0;
                    requestBody = stream.ReadToEnd();
                }
                if (userIp == "::1")
                    userIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == AddressFamily.InterNetwork).ToString();
                LogRequest logRequisicao = new LogRequest(rota, controller, httpVerb, queryString, requestBody, userId, userIp, statusCode, executionTime);
                _logRequestRepository.Post(logRequisicao);
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
