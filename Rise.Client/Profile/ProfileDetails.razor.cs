using Microsoft.AspNetCore.Components;
using Rise.Shared.User;

namespace Rise.Client.Profile;

public partial class ProfileDetails : ComponentBase
{
    [Parameter, EditorRequired] public required UserDto User { get; set; }

    private void ChangePassword()
    {
        Console.WriteLine("Verander wachtwoord");
        //todo: logica te doen
    }
}