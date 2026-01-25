using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Helpers;
using System.Text.Json;

namespace RaspberryPi.Application.Services;

public sealed class DatabaseAppService : IDatabaseAppService
{
    private readonly IFactAppService _factAppService;
    private readonly IFeedbackAppService _feedbackAppService;
    private readonly IEmailAppService _emailAppService;
    private readonly IGeoLocationAppService _geolocationAppService;

    public DatabaseAppService(IFactAppService factAppService,
                               IEmailAppService emailAppService,
                               IFeedbackAppService feedbackAppService,
                               IGeoLocationAppService geolocationAppService)
    {
        _emailAppService = emailAppService ?? throw new ArgumentNullException(nameof(emailAppService));
        _factAppService = factAppService ?? throw new ArgumentNullException(nameof(factAppService));
        _feedbackAppService = feedbackAppService ?? throw new ArgumentNullException(nameof(feedbackAppService));
        _geolocationAppService = geolocationAppService ?? throw new ArgumentNullException(nameof(geolocationAppService));
    }

    public async Task<string> GenerateDatabaseBackupAsJsonStringAsync()
    {
        var factsTask = _factAppService.GetAllFactsAsync();
        var geoLocationTask = _geolocationAppService.GetAllGeoLocationsFromDatabaseAsync();
        var feedbackTask = _feedbackAppService.GetAllAsync();
        var emailsTask = _emailAppService.GetAllAsync();

        await Task.WhenAll(factsTask, geoLocationTask, feedbackTask, emailsTask);

        var dbBackup = new DbBackupDto
        {
            Facts = factsTask.Result,
            GeoLocations = geoLocationTask.Result,
            FeedbackMessages = feedbackTask.Result,
            EmailsOutbox = emailsTask.Result
        };

        // Override default to have indented JSON for better readability
        var options = new JsonSerializerOptions(JsonDefaults.Options);
        options.WriteIndented = true;
        return JsonSerializer.Serialize(dbBackup, options);
    }

    public async Task<int> ImportDatabaseBackupAsync(DbBackupDto backup)
    {
        ArgumentNullException.ThrowIfNull(backup);

        var geoLocationTask = _geolocationAppService.ImportBackupAsync(backup.GeoLocations);
        var factTask = _factAppService.ImportBackupAsync(backup.Facts);
        var feedbackTask = _feedbackAppService.ImportBackupAsync(backup.FeedbackMessages);
        var emailsTask = _emailAppService.ImportBackupAsync(backup.EmailsOutbox);

        await Task.WhenAll(geoLocationTask, factTask, feedbackTask, emailsTask);

        var count = geoLocationTask.Result +
                    factTask.Result +
                    feedbackTask.Result +
                    emailsTask.Result;

        return count;
    }
}