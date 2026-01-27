using System.Net;

public class Response<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public object Meta { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    public T Data { get; set; }

    // ✅ Private Constructor - prevents direct instantiation
    // يمنع إنشاء الـ object بشكل مباشر وعشوائي
    private Response()
    {
        Errors = new List<string>();
    }

    // ====== Static Factory Methods ======

    /// <summary>
    /// Creates a successful response with data
    /// </summary>
    public static Response<T> Success(T data, string message = "Operation Successful", object meta = null)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data), "Success response must contain data");

        return new Response<T>
        {
            Data = data,
            Meta = meta,
            Succeeded = true,
            StatusCode = HttpStatusCode.OK,
            Message = message
        };
    }

    /// <summary>
    /// Creates a failure response with single error message
    /// </summary>
    public static Response<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        if (string.IsNullOrWhiteSpace(message))
            message = "Operation Failed";

        return new Response<T>
        {
            Succeeded = false,
            Message = message,
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Creates a failure response with multiple validation errors
    /// </summary>
    public static Response<T> Failure(List<string> errors, string message = "Validation Failed", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        if (errors == null || errors.Count == 0)
            errors = new List<string> { "Unknown error occurred" };

        return new Response<T>
        {
            Succeeded = false,
            Message = message,
            Errors = errors,
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Creates a Not Found response (404)
    /// </summary>
    public static Response<T> NotFound(string message = "Resource Not Found")
    {
        return new Response<T>
        {
            Succeeded = false,
            Message = message,
            StatusCode = HttpStatusCode.NotFound
        };
    }

    /// <summary>
    /// Creates an Unauthorized response (401)
    /// </summary>
    public static Response<T> Unauthorized(string message = "Unauthorized Access")
    {
        return new Response<T>
        {
            Succeeded = false,
            Message = message,
            StatusCode = HttpStatusCode.Unauthorized
        };
    }

    /// <summary>
    /// Creates a Forbidden response (403)
    /// </summary>
    public static Response<T> Forbidden(string message = "Access Forbidden")
    {
        return new Response<T>
        {
            Succeeded = false,
            Message = message,
            StatusCode = HttpStatusCode.Forbidden
        };
    }

    /// <summary>
    /// Creates a Created response for POST operations (201)
    /// </summary>
    public static Response<T> Created(T data, string message = "Resource Created Successfully", object meta = null)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data), "Created response must contain data");

        return new Response<T>
        {
            Data = data,
            Meta = meta,
            Succeeded = true,
            StatusCode = HttpStatusCode.Created,
            Message = message
        };
    }

    /// <summary>
    /// Creates a Deleted response (200)
    /// </summary>
    public static Response<T> Deleted(string message = "Resource Deleted Successfully")
    {
        return new Response<T>
        {
            Succeeded = true,
            Message = message,
            StatusCode = HttpStatusCode.OK
        };
    }

    /// <summary>
    /// Creates a validation error response (422)
    /// </summary>
    public static Response<T> ValidationError(List<string> errors)
    {
        if (errors == null || errors.Count == 0)
            errors = new List<string> { "Validation failed" };

        return new Response<T>
        {
            Succeeded = false,
            Message = "Validation Failed",
            Errors = errors,
            StatusCode = HttpStatusCode.UnprocessableEntity
        };
    }

    /// <summary>
    /// Creates a Bad Request response (400)
    /// </summary>
    public static Response<T> BadRequest(string message = "Bad Request")
    {
        return new Response<T>
        {
            Succeeded = false,
            Message = message,
            StatusCode = HttpStatusCode.BadRequest
        };
    }

    /// <summary>
    /// Creates an Internal Server Error response (500)
    /// </summary>
    public static Response<T> InternalServerError(string message = "Internal Server Error")
    {
        return new Response<T>
        {
            Succeeded = false,
            Message = message,
            StatusCode = HttpStatusCode.InternalServerError
        };
    }
}