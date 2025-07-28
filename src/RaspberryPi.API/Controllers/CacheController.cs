using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace RaspberryPi.API.Controllers;

/// <summary>
/// Provides methods for managing memory cache.
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class CacheController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;

    public CacheController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    /// <summary>
    /// Gets a snapshot of the cache statistics if available.
    /// </summary>
    /// <returns>An instance of MemoryCacheStatistics containing a snapshot of the cache statistics.</returns>
    [HttpGet]
    public MemoryCacheStatistics? Statistics()
    {
        return _memoryCache.GetCurrentStatistics();
    }

    /// <summary>
    /// Gets the value from the memory cache.
    /// </summary>
    /// <param name="key">The key to retrieve the value for.</param>
    /// <returns>The value associated with the specified key, or null if not found.</returns>
    [HttpGet]
    public IActionResult Get(string key)
    {
        if (_memoryCache.TryGetValue(key, out var value))
        {
            return Ok(value);
        }

        return NotFound();
    }
}