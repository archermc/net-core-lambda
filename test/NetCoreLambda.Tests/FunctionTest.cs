using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.XRay.Recorder.Core;
using NSubstitute;

namespace NetCoreLambda.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
        }

        [Fact]
        public void TetGetMethod()
        {
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            var xraySub = Substitute.For<IAWSXRayRecorder>();

            xraySub.BeginSubsegment(Arg.Any<string>());
            xraySub.EndSubsegment();

            Functions functions = new Functions(xraySub);


            request = new APIGatewayProxyRequest();
            context = new TestLambdaContext();
            response = functions.Get(request, context);
            Assert.Equal(200, response.StatusCode);
        }
    }
}
