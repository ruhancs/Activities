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
        public class Query : IRequest<Result<PagedList<ActivityDto>>>
        {
            public PagingParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
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

            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                //tranforma activity para ActivityDto
                var query = _context.Activities
                    .ProjectTo<ActivityDto>(
                        _mapper.ConfigurationProvider,
                        new {currentUsername = _userAccessor.GetUsername()}
                        )
                    .AsQueryable();

                return Result<PagedList<ActivityDto>>.Success(
                    await PagedList<ActivityDto>.CreateAsync(
                        query, 
                        request.Params.PageNumber, 
                        request.Params.PageSize)
                );
            }
        }
    }
}