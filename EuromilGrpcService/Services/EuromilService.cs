using euromilregister;
using Grpc.Core;

namespace EuromilGrpcService.Services;

public class EuromilService:Euromil.EuromilBase
{
    private readonly ILogger<EuromilService> _logger;

    public EuromilService(ILogger<EuromilService> logger)
    {
        _logger = logger;
    }

    public override Task<RegisterReply> RegisterEuroMil(RegisterRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Recieved request to register Key {request.Key} with CheckID {request.Checkid}.");

        string replyMessage = "";

        if (!ValidateKey(request.Key))
        {
            replyMessage = "Error. Key is not Valid.";
            _logger.LogError("Error. Key is not Valid.");
        }
        else if (!ValidateId(request.Checkid))
        {
            replyMessage = "Error. CheckID is not Valid.";
            _logger.LogError("Error. CheckID is not Valid.");
        }
        else
        {
            replyMessage = $"Success. Key ({request.Key}) is registered with CheckID ({request.Checkid})";
            _logger.LogInformation($"Success. Key ({request.Key}) is registered with CheckID ({request.Checkid})");
        }

        return Task.FromResult(new RegisterReply
        {
            Message = replyMessage
        });
    }
    
    private bool ValidateKey(string key)
    {
        string[] splitKey = key.Split(" ");
        int[] numKey = new int[5];
        int[] numStar = new int[2];

        if (splitKey.Count() != 7)
            return false;

        for (int i = 0; i < 7; i++)
        {
            if (i < 5)
            {
                if (!Int32.TryParse(splitKey[i], out numKey[i]) || numKey[i] <= 0 || numKey[i] > 50)
                    return false;
            }
            else
            {
                if (!Int32.TryParse(splitKey[i], out numStar[i - 5]) || numStar[i - 5] <= 0 || numStar[i - 5] > 12)
                    return false;
            }
        }

        if (numKey.Distinct().Count() != 5 || numStar.Distinct().Count() != 2)
        {
            return false;
        }

        return true;
    }

    private bool ValidateId(string checkid)
    {
        if (checkid.Length != 16 || !Double.TryParse(checkid, out _))
        {
            return false;
        }

        return true;
    }
}