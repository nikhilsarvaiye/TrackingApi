namespace Services.Validators
{
    using FluentValidation;
    using Models;

    public class NotificationValidator : AbstractValidator<Notification>
    {
        public NotificationValidator()
        {
            CascadeMode = CascadeMode.Stop;
        }
    }
}
