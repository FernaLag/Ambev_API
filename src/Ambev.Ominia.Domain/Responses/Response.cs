namespace Ambev.Ominia.Domain.Responses;

public class Response : GenericResponse<object>
{
    public Response() { }

    public Response(object data)
    {
        Data = data;
    }

    public Response(string message)
    {
        Message = message;
    }

    public Response(object data, string message)
    {
        Message = message;
        Data = data;
    }

    public Response(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}