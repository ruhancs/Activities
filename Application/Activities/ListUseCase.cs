using Application.Core;
using Application.interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    //registrar mediator em program
    public class ListUseCase
    {
        public class Query : IRequest<Result<List<ActivityDto>>>{}

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessorRepository _userAccessor;
            public Handler(
                DataContext context, 
                IMapper mapper,
                IUserAccessorRepository userAccessor
                )
            {
                _mapper = mapper;
                _context = context;
                _userAccessor = userAccessor;    
            }

            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                //tranforma activity para ActivityDto
                var activities = await _context.Activities
                    .ProjectTo<ActivityDto>(
                        _mapper.ConfigurationProvider,
                        new {currentUsername = _userAccessor.GetUsername()}
                        )
                    .ToListAsync();

                return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }
}