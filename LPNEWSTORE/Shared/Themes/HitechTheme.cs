using MudBlazor;

namespace LPNEWSTORE.Shared.Themes
{
    public class HitechTheme :MudTheme
    {
        
            public HitechTheme()
            {
            PaletteLight = new PaletteLight
            {
                Primary = "#3a8700",           // igual que dark
                Secondary = "#b4cc9a",         // igual que dark
                Background = "#ffffff",        // fondo claro
                AppbarBackground = "#f5f5f5",  // fondo claro para appbar
                DrawerBackground = "#f5f5f5",  // fondo claro para drawer
                DrawerText = "#000000",        // texto oscuro en drawer
                DrawerIcon = "#3a8700",        // iconos verdes igual
                AppbarText = "#3a8700" ,        // texto appbar verde igual que en dark
                LinesDefault = "#3a8700",     // Líneas suaves claras
                TableLines = "#3a8700",
                TableHover = "#aedb91"
            };

            PaletteDark = new PaletteDark
                {
                    Primary = "#3a8700",          // mismo primary para dark
                    Secondary = "#d9f4c7",
                    Background = "#2c3138",       // fondo dark
                    AppbarBackground = "#2c3138",
                    DrawerBackground = "#2c3138",
                    DrawerText = "#ffffff",
                    DrawerIcon = "#3a8700",
                    AppbarText = "#3a8700",
                    LinesDefault = "#3a8700",     // Líneas suaves claras
                    TableLines = "#3a8700",
                    TableHover = "#aedb91"

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

