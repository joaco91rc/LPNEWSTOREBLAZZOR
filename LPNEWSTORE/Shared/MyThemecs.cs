using MudBlazor;

namespace LPNEWSTORE.Shared
{


    public class MyTheme : MudTheme
    {
        public MyTheme()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#3a8700",          // --hitech-pantone2421c
                Secondary = "#d9f4c7",        // --hitech-verde-pastel
                Background = "#2c3138",       // --hitech-pantoneblack (fondo general)
                AppbarBackground = "#385d81", // --hitech-2151c (barra superior)
                DrawerBackground = "#2c3138", // --drawer-bg-color
                DrawerText = "#ffffff",       // --drawer-text-color (blanco)
                DrawerIcon = "#aaf129"        // --hitech-pantone375c (iconos drawer)
                                              // No existe DrawerHoverBackground, ese estilo va por CSS
            };

            PaletteDark = new PaletteDark
            {
                Primary = "#3a8700",          // mismo primary para dark
                Secondary = "#d9f4c7",
                Background = "#2c3138",       // fondo dark
                AppbarBackground = "#385d81",
                DrawerBackground = "#2c3138",
                DrawerText = "#ffffff",
                DrawerIcon = "#aaf129"
            };
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" }
                }
            };
        }
    }

    }
