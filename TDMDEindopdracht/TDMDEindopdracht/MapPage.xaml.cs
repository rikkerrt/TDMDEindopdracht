using TDMDEindopdracht.Domain.Services;

namespace TDMDEindopdracht;

public partial class MapPage : ContentPage
{
	public MapPage(MapPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}