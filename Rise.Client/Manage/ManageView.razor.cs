using Microsoft.AspNetCore.Components;

namespace Rise.Client.Manage;

public partial class ManageView<T> : ComponentBase
{
    protected bool isDeleteModalVisible = false;
    private T? selectedItem;
    private string? itemName = null;

    [Parameter] public required RenderFragment ChildContent { get; set; }
    [Parameter] public required string ItemType { get; set; }
    [Parameter] public EventCallback<T> OnDelete { get; set; }
    [Parameter] public EventCallback ShowAddModal { get; set; }

    public void ShowDeleteModal(T item, string itemName)
    {
        selectedItem = item;
        this.itemName = itemName;
        isDeleteModalVisible = true;
        StateHasChanged();
    }

    private void HideDeleteModal()
    {
        isDeleteModalVisible = false;
        selectedItem = default!;
        StateHasChanged();
    }

    private async Task DeleteItem()
    {
        await OnDelete.InvokeAsync(selectedItem);
        HideDeleteModal();
    }
}
