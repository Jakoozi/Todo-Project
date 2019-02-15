using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using Hangfire;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Annotations;

namespace TodoApi
{
    public class HangFireAuthorization : IDashboardAuthorizationFilter
    {
        //private readonly IAuthorizationService _authorizationService;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private IAuthorizationService authorizationService;
        private IHttpContextAccessor httpContextAccessor;

        public HangFireAuthorization(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        {
            this.authorizationService = authorizationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        //public bool Authorize([NotNull] DashboardContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public HangfireAuthorization(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        //{
        //    _authorizationService = authorizationService;
        //    _httpContextAccessor = httpContextAccessor;
        //}


        public bool Authorization([NotNull] DashboardContext context)
        {
            return true;//Do Your Stuff here
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            throw new NotImplementedException();
        }
    }
}
