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
        public string Message;
        public T? Data;
    }
}
