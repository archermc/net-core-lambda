using System;
using System.Collections.Generic;
using System.Net;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Amazon.XRay.Recorder.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace NetCoreLambda
{
    public class Functions
    {
        ITimeProcessor processor = new TimeProcessor();
        IAWSXRayRecorder xray;

        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions(IAWSXRayRecorder xray = null)
        {
            if (xray == null) {
                this.xray = AWSXRayRecorder.Instance;
            } else {
                this.xray = xray;
            }
        }

        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LogMessage(context, "Starting lambda execution");

            APIGatewayProxyResponse response;
            try {
                var result = TraceFunction(processor.CurrentTimeUTC, "GetTime");

                response = CreateResponse(result);

                LogMessage(context, "Lambda execution succeeded");
            } catch(Exception e) {
                LogMessage(context, $"Lambda execution failed - {e.Message}");
                response = CreateResponse(null);
            }

            return response;
        }

        APIGatewayProxyResponse CreateResponse(DateTime? result)
        {
            int statusCode = (result != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            string body = (result != null) ?
                JsonConvert.SerializeObject(result) : string.Empty;

            var response = new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = body,
                Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" },
                        { "Access-Control-Allow-Origin", "*" }
                    }
            };

            return response;
        }

        void LogMessage(ILambdaContext ctx, string msg)
        {
            ctx.Logger.LogLine(
                $"{ctx.AwsRequestId}:{ctx.FunctionName} - {msg}"
            );
        }

        private T TraceFunction<T>(Func<T> func, string subSegmentName) {
            this.xray.BeginSubsegment(subSegmentName);
            T result = func();
            this.xray.EndSubsegment();

            return result;
        }
    }
}
