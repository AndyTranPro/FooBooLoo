namespace FooBooLooGameAPI.DTO;

//This class is used for the response of the API
public class ResponseDto
{
    public string message { get; set; }
    public object data { get; set; }
    public int statusCode { get; set; }
    public bool success { get; set; }

    public ResponseDto(string message, object data, int statusCode, bool success)
    {
        this.message = message;
        this.data = data;
        this.statusCode = statusCode;
        this.success = success;
    }
}