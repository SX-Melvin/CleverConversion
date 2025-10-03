using CleverConversion.Configurations;
using CleverConversion.Dto.OTCS;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace CleverConversion.Services.REST
{
    public class OTCSRestService
    {
        private readonly string _username;
        private readonly string _secret;
        private readonly RestClient _client;
        private readonly string _downloadPath = Path.GetFullPath("./Files");
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public OTCSRestService(IOptions<OTCSConfiguration> options)
        {
            _username = options.Value.Username;
            _secret = options.Value.Secret;
            _client = new RestClient(new RestClientOptions(options.Value.ApiUrl));
            if(!Path.Exists(_downloadPath))
            {
                Directory.CreateDirectory(_downloadPath);
            }
        }

        public async Task<GetTicketResponse> GetTicket()
        {
            GetTicketResponse result = new();

            try
            {
                var request = new RestRequest("v1/auth", Method.Post);

                request.AddParameter("username", _username);
                request.AddParameter("password", _secret);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                var response = await _client.ExecuteAsync<GetTicketResponse>(request);

                _logger.Info("v1/auth: " + response.Content);

                var data = JsonConvert.DeserializeObject<GetTicketResponse>(response.Content);

                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                result.Error = ex.Message;
            }

            return result;
        }

        public async Task<DownloadFileResponse> DownloadFile(long nodeID, string fileName, string? ticket = null)
        {
            DownloadFileResponse result = new();

            try
            {
                if(ticket == null)
                {
                    var getTicket = await GetTicket();
                    if (getTicket.Error != null) 
                    {
                        result.Error = getTicket.Error;
                        return result;
                    }
                    else if(getTicket.Ticket == null)
                    {
                        result.Error = "Ticket is empty";
                        return result;
                    }

                    ticket = getTicket.Ticket;
                }

                var request = new RestRequest($"v1/nodes/{nodeID}/content", Method.Get);
                request.AddHeader("otcsticket", ticket);

                var response = await _client.ExecuteAsync(request);
                var relativePath = fileName;
                var filePath = Path.Combine(_downloadPath, fileName);

                if(File.Exists(filePath))
                {
                    var folderPath = Path.Combine(_downloadPath, nodeID.ToString());
                    filePath = Path.Combine(folderPath, fileName);
                    relativePath = Path.Combine(nodeID.ToString(), fileName);
                    if (!Path.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                }

                await File.WriteAllBytesAsync(filePath, response.RawBytes);
                result.AbsolutePath = filePath;
                result.RelativePath = relativePath;
            }
            catch (Exception ex) 
            { 
                _logger.Error(ex);
                result.Error = ex.Message;
            }

            return result;
        }
    }
}
