using FooBooLooGameAPI.DTO;

namespace FooBooLooGameAPI.Helpers;

public static class ResponseHandler
{
    public static ResponseDto successResponse(object data, string message, int statusCode)
    {
        return new ResponseDto(message, data, statusCode, true);
    }

    public static ResponseDto errorResponse(string error, int statusCode = 400)
    {
        return new ResponseDto(error, null, statusCode, false);
    }
}