using MethodTimer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace RaspberryPi.API.Controllers;

/// <summary>
/// Anonymous feedback messages
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackAppService _feedbackAppService;
    private readonly IServiceScopeFactory _serviceFactory;

    public FeedbackController(IFeedbackAppService feedbackAppService, IServiceScopeFactory serviceFactory)
    {
        _feedbackAppService = feedbackAppService;
        _serviceFactory = serviceFactory;
    }

    /// <summary>
    /// Submit a feedback
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    [Time]
    [HttpPost]
    public async Task Submit([Required][FromBody][StringLength(5000)]string message)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
        var headersJson = JsonSerializer.Serialize(headers);
        await _feedbackAppService.SubmitFeedbackAsync(message, ipAddress, headersJson);
    }

    /// <summary>
    /// Submit a feedback in a fire and forget way
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    [Time]
    [HttpPost]
    public IActionResult SubmitAndForget([Required][FromBody][StringLength(5000)] string message)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
        var headersJson = JsonSerializer.Serialize(headers);

        _ = Task.Run(async () =>
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var feedbackAppService = scope.ServiceProvider.GetRequiredService<IFeedbackAppService>();
                await feedbackAppService.SubmitFeedbackAsync(message, ipAddress, headersJson);
            }
        });

        return Accepted();
    }

    /// <summary>
    /// Get all feedbacks from database
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<IEnumerable<FeedbackMessage>> GetAll()
    {
        return await _feedbackAppService.GetAllAsync();
    }

    /// <summary>
    /// Deletes a feeback
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Time]
    [HttpDelete]
    [Authorize(Roles = "root")]
    public async Task Delete([Required]Guid id)
    {
        await _feedbackAppService.DeleteAsync(id);
    }

    /// <summary>
    /// Deletes all feedbacks
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpDelete]
    [Authorize(Roles = "root")]
    public async Task DeleteAll()
    {
        await _feedbackAppService.DeleteAllAsync();
    }
}