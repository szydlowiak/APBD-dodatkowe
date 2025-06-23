using Dodatkowe.DTOs;
using Dodatkowe.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dodatkowe.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController(IDbService service) : ControllerBase
{
    private readonly IDbService _service = service;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventById(int id)
    {
        var result = await _service.GetEventByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateEvent(CreateEventDto dto)
    {
        var id = await _service.CreateEventAsync(dto);
        var createdEvent = await _service.GetEventByIdAsync(id);

        return CreatedAtAction(nameof(GetEventById), new { id }, createdEvent);
    }

    [HttpPost("assign-speaker")]
    public async Task<IActionResult> AssignSpeaker(AssignSpeakerDto dto)
    {
        await _service.AssignSpeakerToEventAsync(dto);
        return Ok("Speaker assigned");
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingEvents()
    {
        var result = await _service.GetUpcomingEventsAsync();
        return Ok(result);
    }
    
}