using BarcodeStandard;
using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Minio;
using Serilog;

namespace Rise.Server.Controllers;

// this proxy controller is used to get images from the minio server because OPS has not made the minio server public
[ApiController]
[Route("api/[controller]")]
public class ProxyController : ControllerBase
{
    private readonly IMinioService minio;

    public ProxyController(IMinioService minio)
    {
        this.minio = minio;
    }

    [HttpGet("image")]
    public async Task<IActionResult> GetImage([FromQuery] string? url)
    {
        Log.Information("Getting image with url:{url}✨", url);
        StreamContent stream = await minio.GetImageAsync(url ?? "");
        string contentType = stream.Headers.ContentType?.MediaType ?? "application/octet-stream";
        return File(await stream.ReadAsStreamAsync(), contentType);
    }

}