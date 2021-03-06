using JPProject.Admin.Domain.Validations.Client;

namespace JPProject.Admin.Domain.Commands.Clients
{
    public class RemoveClientCommand : ClientCommand
    {

        public RemoveClientCommand(string clientId)
        {
            this.Client = new IdentityServer4.Models.Client() { ClientId = clientId };
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveClientCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}