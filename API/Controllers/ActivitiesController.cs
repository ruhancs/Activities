using Application.Activities;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetActivities([FromQuery]PagingParams pagingParams)
        {
            return HandleResult(await Mediator.Send(new ListUseCase.Query{Params = pagingParams}));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            var result = await Mediator.Send(new DetailsUseCase.Query{ Id = id });
        
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromBody]Activity activity)
        {
            return HandleResult(await Mediator.Send(new CreateUseCase.Command{Activity = activity}));
        }

        // IsActivityHost definida em Infrastructure.Security.IsHostRequirement
        // e habilitada em IdentityServiceExtensions
        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id,[FromBody]Activity activity)
        {
            activity.Id = id;

            return HandleResult(await Mediator.Send(new EditUseCase.Command{Activity = activity}));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteUseCase.Command{Id = id}));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command{Id = id}));
        }
    }
}