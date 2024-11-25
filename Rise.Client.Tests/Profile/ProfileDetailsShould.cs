using System.Threading.Tasks;
using Rise.Client.Auth;
using Rise.Client.Components;
using Rise.Shared.User;
using Xunit.Abstractions;

namespace Rise.Client.Profile;

public class ProfileDetailsShould : TestContext
{
    public ProfileDetailsShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddSingleton<IUserService, FakeUserService>();
    }

    [Fact]
    public async Task RendersUserProfileCorrectly()
    {
        var mockUser = await Services.GetService<IUserService>()!.GetCurrentUser();

        var component = RenderComponent<ProfileDetails>(parameters => parameters
            .Add(p => p.User, mockUser)
        );

        var img = component.Find("img");
        var paragraphs = component.FindAll("p");

        Assert.Equal(mockUser.Picture, img.GetAttribute("src"));
        Assert.Equal("Profile Picture", img.GetAttribute("alt"));
        Assert.Contains(mockUser.FullName, paragraphs[0].TextContent);
        Assert.Contains(mockUser.Email, paragraphs[1].TextContent);
    }

    [Fact]
    public async Task RendersChangePasswordButton()
    {
        var mockUser = await Services.GetService<IUserService>()!.GetCurrentUser();

        var component = RenderComponent<ProfileDetails>(parameters => parameters
            .Add(p => p.User, mockUser)
        );

        var button = component.FindComponent<SmallButton>();

        Assert.NotNull(button);
        Assert.Equal("Wachtwoord veranderen", button.Instance.Text);
    }
}