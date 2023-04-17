using Api.Extensions;
using Api.Filters;
using Application.Interfaces;
using Domain.Entities;
using Domain.Requests;
using Domain.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

[SocialAuthFilter]
[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly AppSettings _appSettings;

    public AppointmentController(IAppointmentService appointmentService, IOptions<AppSettings> appSettings)
    {
        _appointmentService = appointmentService;
        _appSettings = appSettings.Value;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment(
        [FromBody]
        AppointmentCreationRequest appointment,
        CancellationToken cancellationToken)
    {
        var user = await HttpContext.GetCurrentAccountAsync(_appSettings);
        var createdAppointment = await _appointmentService.CreateAsync(appointment, user.Email, cancellationToken);
        return Ok(createdAppointment);
    }

    [HttpGet]
    public async Task<AppointmentResponse> GetCurrentUserAppointments(int skip, int take, CancellationToken cancellationToken)
    {
        var user = await HttpContext.GetCurrentAccountAsync(_appSettings);
        
        var events = await _appointmentService.GetUsersAppointmentsAsync(new AppointmentRetrieveRequest
        {
            Email = user.Email,
            Skip = skip,
            Take = take
        }, cancellationToken);

        return events;
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