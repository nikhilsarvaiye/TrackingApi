namespace Services.Validators
{
    using FluentValidation;
    using Models;

    public class UserNotificationValidator : AbstractValidator<UserNotification>
    {
        public UserNotificationValidator()
        {
            CascadeMode = CascadeMode.Stop;
        }
    }
}
