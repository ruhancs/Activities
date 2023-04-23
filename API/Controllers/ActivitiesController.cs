using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            return HandleResult(await Mediator.Send(new ListUseCase.Query()));
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

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id,[FromBody]Activity activity)
        {
            activity.Id = id;

            return HandleResult(await Mediator.Send(new EditUseCase.Command{Activity = activity}));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteUseCase.Command{Id = id}));
        }
    }
}