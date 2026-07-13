using factoryRL.GameObjects;
using factoryRL.GameObjects.Resources;

namespace factoryRL.courierRoute;

public class CourierRoute
{
    public WorkStation Source{ get; set; }
    public WorkStation Target;
    public ResourceItemType ResourceType { get; set; }

    public bool CanPickup()
    {
        return Source.Inventory.Has(ResourceType);
    }

}
