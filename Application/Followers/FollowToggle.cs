
using Application.Core;
using Application.interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
    public class FollowToggle
    {
        public class Command: IRequest<Result<Unit>>
        {
            public string TargetUsername { get; set; }
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
                // usuario que seguira o outro usuario
                var observer = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());
                
                // usuario que sera seguido
                var target = await _context.Users
                    .FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);
                
                if(target is null) return null;

                var following = await _context.UserFollowings
                    .FindAsync(observer.Id, target.Id);

                if(following is null) 
                {
                    following = new Domain.UserFollowing
                    {
                        Observer = observer,
                        Target = target
                    };

                    _context.UserFollowings.Add(following);
                }
                else
                {
                    _context.UserFollowings.Remove(following);
                }

                var success = await _context.SaveChangesAsync() > 0;

                if(success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to follow");

            }
        }
    }
}