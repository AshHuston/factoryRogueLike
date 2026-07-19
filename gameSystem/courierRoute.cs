using factoryRL.GameObjects;
using factoryRL.GameObjects.Resources;

namespace factoryRL.courierRoute;

public class CourierRoute
{
    public WorkStation Source{ get; set; }
    public WorkStation Target;
    public ResourceItemType ResourceType { get; set; }

    public CourierRoute(WorkStation _source, WorkStation _target, ResourceItemType _itemType)
    {
        Source = _source;
        Target = _target;
        ResourceType = _itemType;
    }

    public CourierRoute(CourierRoute other)
    {
        Source = other.Source;
        Target = other.Target;
        ResourceType = other.ResourceType;
    }

    public bool CanPickup()
    {
        return Source.Inventory.Has(ResourceType);
    }

}
