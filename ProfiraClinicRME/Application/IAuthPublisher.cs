namespace ProfiraClinicRME.Application
{
    public interface IAuthPublisher
    {
        public event EventHandler<AuthEventArgs> AuthEvent;

    }
}
