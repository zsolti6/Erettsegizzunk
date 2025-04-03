namespace ErettsegizzunkApi
{
    public class TimerService : IHostedService, IDisposable
    {
        private readonly ILogger<TimerService> _logger;
        private Timer _timer;
        private readonly HttpClient _httpClient;

        public TimerService(ILogger<TimerService> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("TimerService started.");

            // Set the timer to trigger every 840,000 milliseconds (14 minutes)
            _timer = new Timer(PerformRequest, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(840000));
            return Task.CompletedTask;
        }

        private async void PerformRequest(object state)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://erettsegizzunk.onrender.com/erettsegizzunk/Levels/get-szintek");
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Request succeeded: {response.StatusCode}");
                }
                else
                {
                    _logger.LogWarning($"Request failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception during GET request: {ex.Message}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("TimerService stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _httpClient?.Dispose();
        }
    }
}
