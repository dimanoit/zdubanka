using Domain.Requests.Common;

namespace Domain.Requests;

public record EventParticipantRequest(string EventId, string OrganizerId) : PageRequestBase;
public record EventParticipantRestRequest(string EventId) : PageRequestBase;
