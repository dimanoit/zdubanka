using System.Security.Claims;
using Api.Extensions;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Requests;
using Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment(
        [FromBody] AppointmentCreationRequest appointment,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

        var createdAppointment = await _appointmentService.CreateAsync(appointment, userId, cancellationToken);
        return Ok(createdAppointment);
    }

    [HttpGet]
    public async Task<AppointmentResponse> GetCurrentUserAppointments(int skip, int take,
        CancellationToken cancellationToken)
    {
        var userId = User.GetId();

        var events = await _appointmentService.GetUsersAppointmentsAsync(
            new AppointmentRetrieveRequest
            {
                UserId = userId,
                Skip = skip,
                Take = take
            }, cancellationToken);

        return events;
    }

    [HttpPatch("{appointmentId}/apply")]
    public async Task ApplyOnAppointmentAsync(string appointmentId, CancellationToken cancellationToken)
    {
        await _appointmentService.ApplyOnAppointmentAsync(appointmentId,
            User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value, cancellationToken);
    }

    [HttpGet("{appointmentId}")]
    public async Task<IActionResult> GetAppointment(string appointmentId, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.GetAsync(appointmentId, cancellationToken);
        if (appointment == null) return NotFound();
        return Ok(appointment);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAppointment([FromBody] Appointment appointment,
        CancellationToken cancellationToken)
    {
        await _appointmentService.UpdateAsync(appointment, cancellationToken);
        return Ok();
    }

    [HttpDelete("{appointmentId}")]
    public async Task<IActionResult> DeleteAppointment(string appointmentId, CancellationToken cancellationToken)
    {
        await _appointmentService.DeleteAsync(appointmentId, cancellationToken);
        return Ok();
    }
}