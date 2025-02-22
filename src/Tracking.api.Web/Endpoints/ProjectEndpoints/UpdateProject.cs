﻿using Microsoft.Build.Framework;
using Tracking.api.UseCases.Services.Projects.Commands;

namespace Tracking.api.Web.Endpoints.ProjectEndpoints;

public class UpdateProject: Endpoint<UpdateProject.UpdateProjectRequest, Result<string>>
{
  private readonly IMediator _mediator;

  public UpdateProject(IMediator mediator)
  {
    _mediator = mediator;
  }
  public override void Configure()
  {
    Put(UpdateProjectRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Update's a Project.";
      s.Description = "Update's a Project.";
      s.ExampleRequest = new UpdateProjectRequest { ProjectId = Guid.Empty, Description = "Project Description"};
    });
    Tags([nameof(ProjectEndpoints)]);
  }
  public override async Task<Result<string>> ExecuteAsync(UpdateProjectRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new UpdateProjectCommand(req.ProjectId, req.Description ?? string.Empty), ct);
    if (result.IsSuccess)
      Response = result.Value;
    return result;
  }
  public class UpdateProjectRequest
  {
    public const string Route = "/api/Project/{ProjectId}/Info";
    public static string BuildRoute(string projectId) => Route.Replace("{ProjectId}", projectId.ToString());
    [Required]
    public Guid ProjectId { get; set; }
    public string? Description { get; set; }
  }
}
