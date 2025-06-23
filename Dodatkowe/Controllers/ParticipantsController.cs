using Dodatkowe.DTOs;
using Dodatkowe.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dodatkowe.Controllers;


[ApiController]
[Route("api/participants")]
public class ParticipantsController(IDbService service) : ControllerBase
{
    private readonly IDbService _service = service;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterParticipantDto dto)
    {
        await _service.RegisterParticipantAsync(dto);
        return Ok("Registered");
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel(CancelRegistrationDto dto)
    {
        await _service.CancelRegistrationAsync(dto);
        return Ok("Cancelled");
    }

    [HttpGet("{id}/report")]
    public async Task<IActionResult> GetReport(int id)
    {
        var result = await _service.GetParticipantReportAsync(id);
        return Ok(result);
    }
}