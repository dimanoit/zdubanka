using Application.Interfaces;
using Application.Mappers;
using Domain.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public record UpdateAccountCommand(UpdateAccountRequest Request, CancellationToken CancellationToken) : IRequest;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateAccountCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts
            .FirstOrDefaultAsync(ac => ac.Id == command.Request.UserId, cancellationToken);

        if (account == null) return;

        command.Request.UpdateAccount(account);

        _dbContext.Accounts.Update(account);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}