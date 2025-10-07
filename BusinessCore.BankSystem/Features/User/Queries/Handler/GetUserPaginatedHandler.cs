using AutoMapper;
using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.User.Queries.Requests;
using BusinessCore.BankSystem.Features.User.Queries.Response;
using BusinessCore.BankSystem.Wrapper;
using Domainlayer.BankSystem.Entites;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.User.Queries.Handler
{
    public class GetUserPaginatedHandler : ResponseHandler
                                          , IRequestHandler<GetUserPaginatedQuery, PaginatedResult<GetUserPaginatedResponse>>
    {
        #region fields
         private readonly UserManager<ApplicationUser> _userManager;
         private readonly IMapper mapper;

        #endregion
        #region constructors
        public GetUserPaginatedHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            this.mapper = mapper;
        }



        #endregion
        public async Task<PaginatedResult<GetUserPaginatedResponse>> Handle(GetUserPaginatedQuery request, CancellationToken cancellationToken)
        {
           var user = _userManager.Users.AsQueryable();
           var PagianatedList = await mapper.ProjectTo<GetUserPaginatedResponse>(user)
                                        .ToPaginatedListAsync(request.PageNumber, request.PageSize); 
          return PagianatedList;
        }
    }
}
