using Microsoft.Maui.Controls.Maps;
using Moq;
using TDMDEindopdracht.Domain.Model;
using TDMDEindopdracht.Domain.Services;

namespace UnitTest1
{
    public class UnitTest1
    {
        [Fact]
        public void MvvmMapElementsPropertyShouldUpdateMapElements()
        {

            // Arrange
            BindableMap _bindableMap = new BindableMap();
            var mapElements = new List<MapElement>
            {
                new Polyline { StrokeColor = Colors.Red, StrokeWidth = 2 }
            };

            // Act
            _bindableMap.MvvmMapElements = mapElements;

            // Assert
            Assert.Single(_bindableMap.MapElements);
            Assert.Same(mapElements[0], _bindableMap.MapElements.First());
        }
        [Fact]
        public async Task GetRoutesForView_LoadsRoutesAndAddsToRouteNames()
        {
            // Arrange
            Mock<IDatabaseRepository> _mockDatabaseRepo = new Mock<IDatabaseRepository>();
            Mock<ILocationPermissionService> _mockPermissionsService = new Mock<ILocationPermissionService>();
            MainPageViewModel _viewModel = new MainPageViewModel(_mockDatabaseRepo.Object, _mockPermissionsService.Object);


            var mockRoutes = new List<string> { "Bredaai", "Roos en daal", "Bergen standje zoom" };
            _mockDatabaseRepo.Setup(repo => repo.getVisitedStations())
                             .ReturnsAsync(mockRoutes);

            // Act
            _viewModel.LoadStations();
            await Task.Delay(100);  // Wacht voor asynchrone methoden

            // Assert
            Assert.Equal(3, _viewModel.Stations.Count);
            Assert.Contains("Route1", _viewModel.Stations);
            Assert.Contains("Route2", _viewModel.Stations);
            Assert.Contains("Route3", _viewModel.Stations);
        }
    }
}