﻿
using TDMDEindopdracht.Domain.Model;
using TDMDEindopdracht.Domain.Services;

namespace TDMDEindopdracht
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }

}
