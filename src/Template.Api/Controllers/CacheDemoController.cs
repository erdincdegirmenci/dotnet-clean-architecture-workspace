using Microsoft.AspNetCore.Mvc;
using Template.Infrastructure.Caching;
using Template.Infrastructure.Enums;

namespace Template.Api.Controllers;

[Route("api/[controller]")]
public class CacheDemoController : BaseController
{
    private readonly ICacheManager _cacheManager;

    public CacheDemoController(ICacheManager cacheManager)
    {
        _cacheManager = cacheManager;
    }

    /// <summary>
    /// Cache’e değer ekler.
    /// </summary>
    [HttpGet("set")]
    public IActionResult SetCache()
    {
        _cacheManager.Set("MyKey", "Hello World!", 5, CacheTimeEnum.Minute, useLocalScope: true);
        return Ok("Cache değeri eklendi.");
    }

    /// <summary>
    /// Cache’den değer okur. Eğer yoksa null döner.
    /// </summary>
    [HttpGet("get")]
    public IActionResult GetCache()
    {
        var value = _cacheManager.Get<string>("MyKey");
        if (value == null)
            return NotFound("Cache değeri bulunamadı.");
        return Ok(value);
    }

    /// <summary>
    /// Cache’den belirli bir key’i siler.
    /// </summary>
    [HttpGet("remove")]
    public IActionResult RemoveCache()
    {
        _cacheManager.Remove("MyKey");
        return Ok("Cache değeri silindi.");
    }

    /// <summary>
    /// Async kullanım örneği
    /// </summary>
    [HttpGet("get-async")]
    public async Task<IActionResult> GetCacheAsync()
    {
        var value = await _cacheManager.GetAsync<string>("MyKey");
        if (value == null)
            return NotFound("Cache değeri bulunamadı.");
        return Ok(value);
    }

}

