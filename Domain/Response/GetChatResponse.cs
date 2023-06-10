using Domain.Models;
using Domain.Response.Common;

namespace Domain.Response;

public record GetChatResponse : PageResponseBase<ChatDto>;