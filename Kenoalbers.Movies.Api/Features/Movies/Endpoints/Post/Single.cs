namespace Kenoalbers.Movies.Api.Features.Movies.Endpoints.Post;

using Entities;
using Repository;

public class Single(IRepository repository) : Endpoint<Shared.NameRequest, Shared.Response, Mapper>
{
    public override void Configure()
    {
        Post("/movies");
        Description(builder => builder
                .Accepts<Shared.NameRequest>("application/json")
                .Produces<Shared.Response>(201)
                .ProducesProblemFE(400)
                .ProducesProblemFE<InternalErrorResponse>(500),
            clearDefaults: true);
        Summary(summary => { summary.ExampleRequest = new Shared.NameRequest { Name = "Jurassic Park" }; });
        AllowAnonymous();
    }

    public override async Task HandleAsync(Shared.NameRequest request, CancellationToken cancellationToken)
    {
        var createMovie = repository.Create(Map.ToEntity(request));

        await SendAsync(Map.FromEntity(createMovie), (int)HttpStatusCode.Created, cancellationToken);
    }
}

public class Mapper : Mapper<Shared.NameRequest, Shared.Response, Movie>
{
    public override Movie ToEntity(Shared.NameRequest request) =>
        new() { Name = request.Name };

    public override Shared.Response FromEntity(Movie movie) =>
        new() { Id = movie.Id, Name = movie.Name };
}