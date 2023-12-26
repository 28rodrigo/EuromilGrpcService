using System.Threading.Tasks;
using euromilregister;
using Grpc.Net.Client;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://ken.utad.pt:8282");
var client = new Euromil.EuromilClient(channel);
var reply = client.RegisterEuroMil(new RegisterRequest() { Checkid = "7565970292169410", Key = "1 2 3 4 5 6 7" });
Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();