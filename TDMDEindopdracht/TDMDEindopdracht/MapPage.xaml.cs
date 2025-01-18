using TDMDEindopdracht.Domain.Services;

namespace TDMDEindopdracht;

public partial class MapPage : ContentPage
{

    private string stationsString;

    public MapPage(MapPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}