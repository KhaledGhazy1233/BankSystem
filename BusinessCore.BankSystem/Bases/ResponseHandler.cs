using System.Net;

namespace BusinessCore.BankSystem.Bases
{
    /// <summary>
    /// Generic Response wrapper with Static Factory Methods pattern
    /// Provides type-safe, consistent response handling across the application
    /// </summary>

    /// <summary>
    /// Response Handler - provides backward compatibility with old code
    /// Can be used alongside the new static factory methods
    /// </summary>
    public class ResponseHandler
    {
        public ResponseHandler() { }

        public Response<T> Success<T>(T data, object meta = null)
        {
            return Response<T>.Success(data, "Successful Operation", meta);
        }

        public Response<T> Deleted<T>()
        {
            return Response<T>.Deleted();
        }

        public Response<T> Unauthorized<T>(string message = null)
        {
            return Response<T>.Unauthorized(message ?? "UnAuthorized");
        }

        public Response<T> BadRequest<T>(string message = null)
        {
            return Response<T>.BadRequest(message ?? "Bad Request");
        }

        public Response<T> UnprocessableEntity<T>(string message = null)
        {
            return Response<T>.Failure(message ?? "Unprocessable Entity", HttpStatusCode.UnprocessableEntity);
        }

        public Response<T> NotFound<T>(string message = null)
        {
            return Response<T>.NotFound(message ?? "Not Found");
        }

        public Response<T> Created<T>(T entity, object meta = null)
        {
            return Response<T>.Created(entity, "Created", meta);
        }
    }
}