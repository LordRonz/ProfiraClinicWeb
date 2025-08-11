namespace ProfiraClinicRME.Model
{

    public enum ServiceResultEnum
    {
        FOUND,
        NOT_FOUND,
        SUCCESS,
        FAIL
    }

    /// <summary>
    /// Generic service result
    /// </summary>
    /// 
    public class ServiceResult<T>
    {
        public ServiceResultEnum Status;
        public string Message = "";
        public T? Data = default;

        public static ServiceResult<T> SuccessEmpty(string message ="")
        {
            return new ServiceResult<T>
            {
                Status = ServiceResultEnum.SUCCESS,
                Data = default,
                Message = ""
            };
        }

        public static ServiceResult<T> Success(T data, string message = "")
        {
            return new ServiceResult<T>
            {
                Status = ServiceResultEnum.SUCCESS,
                Data = data,
                Message = ""
            };
        }


        public static ServiceResult<T> Found(T data, string message = "")
        {
            return new ServiceResult<T>
            {
                Status = ServiceResultEnum.FOUND,
                Data = data,
                Message = message
            };
        }

        public static ServiceResult<T> Fail(string message = "")
        {
            return new ServiceResult<T>
            {
                Status = ServiceResultEnum.FAIL,
                Message = message,
                Data = default
            };
        }

        public static ServiceResult<T> NotFound(string message = "")
        {
            return new ServiceResult<T>
            {
                Status = ServiceResultEnum.NOT_FOUND,
                Message = message,
                Data = default
            };
        }
    }

    /// <summary>
    /// Empty result
    /// </summary>
    /// 
    public class ServiceResultEmpty
    {
        public ServiceResultEnum Status;
        public string Message ="";
    }
}
