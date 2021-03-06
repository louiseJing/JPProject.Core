using JPProject.Domain.Core.Events;

namespace JPProject.Admin.Domain.Events.IdentityResource
{
    public class IdentityResourceUpdatedEvent : Event
    {
        public IdentityServer4.Models.IdentityResource Resource { get; }

        public IdentityResourceUpdatedEvent(IdentityServer4.Models.IdentityResource resource)
            : base(EventTypes.Success)
        {
            Resource = resource;
            AggregateId = resource.Name;
        }
    }
}