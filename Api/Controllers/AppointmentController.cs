using Api.Extensions;
using Api.Filters;
using Application.Interfaces;
using Domain.Entities;
using Domain.Requests;
using Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorization, ApiController, Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IAccountService _accountService;
    
    public AppointmentController(IAppointmentService appointmentService, IAccountService accountService)
    {
        _appointmentService = appointmentService;
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment(
        [FromBody]
        AppointmentCreationRequest appointment,
        CancellationToken cancellationToken)
    {
        var userId = await HttpContext.GetCurrentUserIdAsync(_accountService);
        var createdAppointment = await _appointmentService.CreateAsync(appointment, userId, cancellationToken);
        return Ok(createdAppointment);
    }

    [HttpGet]
    public async Task<AppointmentResponse> GetCurrentUserAppointments(int skip, int take,
        CancellationToken cancellationToken)
    {
        var userId = await HttpContext.GetCurrentUserIdAsync(_accountService);

        var events = await _appointmentService.GetUsersAppointmentsAsync(
            new AppointmentRetrieveRequest
        {
            UserId = userId,
            Skip = skip,
            Take = take 
        }, cancellationToken);

        return events;
    }

    [HttpPatch]
    public async Task ApplyOnAppointmentAsync(string appointmentId, CancellationToken cancellationToken)
        => await _appointmentService.ApplyOnAppointmentAsync(appointmentId, await HttpContext.GetCurrentUserIdAsync(_accountService), cancellationToken);

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