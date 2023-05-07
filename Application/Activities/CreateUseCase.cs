using Application.Core;
using Application.interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class CreateUseCase
    {
        public class Command :  IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
        private readonly IUserAccessorRepository _userAccessor;
            public Handler(DataContext context, IUserAccessorRepository userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                //user da requisiçao
                var user = await _context.Users.FirstOrDefaultAsync(
                        x => x.UserName == _userAccessor.GetUsername()
                    );
                
                var attendee = new ActivityAttendee {
                    AppUser = user,
                    Activity = request.Activity,
                    IsHost = true
                };

                request.Activity.Attendees.Add(attendee);

                _context.Activities.Add(request.Activity);

                //SaveChangesAsync retorna um int em caso de sucesso
                var result = await _context.SaveChangesAsync() > 0;

                if(!result) return Result<Unit>.Failure("Failed to create activity");

                //nao retorna nada 
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}