using Microsoft.Maui.Controls.Maps;
using TDMDEindopdracht.Domain.Services;

namespace TDMDEindopdracht;

public partial class MapPage : ContentPage
{

    private string stationsString;
    private MapPageViewModel viewModels;

    public MapPage(MapPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        this.viewModels = viewModel;
        this.viewModels.CreateRoute += createRoute;
    }
    public void createRoute()
    {
        IEnumerable<Location> locations = viewModels.Locations;

        Polyline routeLine = new Polyline
        {
            StrokeColor = Color.FromHex("1F51FF"),
            StrokeWidth = 10
        };

        foreach (Location location in locations)
        {
            routeLine.Geopath.Add(location);
        }
        MapView.MapElements.Add(routeLine);
    }
}