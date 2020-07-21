using System;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Newtonsoft.Json.Linq;

namespace HelloWorldC_VSCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(DateTime.Now);
            GetConfiguration().Wait();
            Console.WriteLine(DateTime.Now);
        }
    
        static async Task GetConfiguration()
        {
            var awsAccessKeyId = "..."; //ConfigurationManager.AppSettings["AWSAccessKeyId"];
            var awsSecretAccessKey = "..."; //ConfigurationManager.AppSettings["AWSSecretAccessKey"];

            var region = Amazon.RegionEndpoint.USEast1;
            AWSCredentials credentials = new BasicAWSCredentials(
                awsAccessKeyId,
                awsSecretAccessKey
            );
            
            var request = new GetParameterRequest()
            {
                Name = "/internal/api/access-logs"
            };

            using (var client = new AmazonSimpleSystemsManagementClient(credentials, region))
            {
                try
                {
                    GetParameterResponse response = await client.GetParameterAsync(request);
                    Console.WriteLine($"Parameter {request.Name} value is: {response.Parameter.Value}");
                    JObject json = JObject.Parse(response.Parameter.Value);

                    Console.WriteLine(json.GetValue("url").ToString());
                    Console.WriteLine(json.GetValue("apiId").ToString());
                    Console.WriteLine(json.GetValue("headerName").ToString());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error occurred: {ex.Message}");
                }
            }
        }
    }
}
