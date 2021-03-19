namespace Services.Validators
{
    using FluentValidation;
    using Models;

    public class TrackerRequestValidator : AbstractValidator<TrackerRequest>
    {
        public TrackerRequestValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Type).Required();
        }
    }
}
