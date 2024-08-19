using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    int a = 5;
    int b = 0;

    if (b == 0)
    {
        //return Results.BadRequest("b 0 olamaz");
        //throw new Exception("B cannot be Zero");
        throw new CannotBeZeroException();
    }

    int c = a / b;

    return Results.Ok(c);
});

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 409;
        context.Response.ContentType = "application/json"; //MediaTypeNames.Application.Json;

        ErrorResponse error = new(ex.Message);

        context.Response.WriteAsync(error.ToString()).Wait();

    }
});

app.Run();

record ErrorResponse(string Message)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

class CannotBeZeroException : Exception
{
    public CannotBeZeroException() : base("B cannot be zero!")
    {

    }

}