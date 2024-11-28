using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Table;

public partial class Column<T> : ComponentBase
{
    [Parameter] public required string Title { get; set; }
    [Parameter] public required string Value { get; set; }
    [CascadingParameter] public T? Item { get; set; }

    // we use object? as parameter because it gets updated in the for loop, prevents creating a new var for the loop
    public string GetNestedValue(object? obj, string propertyPath)
    {
        // check that the object (T) is not null (shouldn't happen) and that the path eg name or product.name is set
        if (obj == null || string.IsNullOrEmpty(propertyPath))
            return "";

        // to support nested values, the "." operator can be used, eg product.name in bookings, we split the path here and get the type of the now T object
        // this will be done recursively until the string value is obtained
        var properties = propertyPath.Split('.');
        var type = obj.GetType();

        foreach (var property in properties)
        {
            var propInfo = type.GetProperty(property);
            if (propInfo == null) 
            {
                // a string (not optimal) is returned if a property is not found, we can't really throw an exception...
                return "property not found " + property;
            }

            // get the value of the property
            // update the object and type for the next iteration
            obj = propInfo.GetValue(obj, null);
            if (obj == null)
                return "null";
            type = obj.GetType();
        }

        return obj.ToString() ?? "";
    }
}