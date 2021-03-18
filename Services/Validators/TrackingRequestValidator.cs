namespace Services.Validators
{
    using FluentValidation;
    using Models;

    public class TrackingRequestValidator : AbstractValidator<TrackingRequest>
    {
        public TrackingRequestValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Type).Required();
        }
    }
}
