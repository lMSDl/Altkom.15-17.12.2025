
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



app.UseOwin(pipe => pipe(dic => OwinReponse));


app.Run();



async Task OwinReponse(IDictionary<string, object> dictionary)
{
    string response = "Hello in OWIN World!";

    Stream body = (Stream)dictionary["owin.ResponseBody"];
    IDictionary<string, string[]> headers = (IDictionary<string, string[]>)dictionary["owin.ResponseHeaders"];

    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(response);

    headers["Content-Length"] = [bytes.Length.ToString()];
    headers["Content-Type"] = ["text/plain"];
    await body.WriteAsync(bytes, 0, bytes.Length);
}