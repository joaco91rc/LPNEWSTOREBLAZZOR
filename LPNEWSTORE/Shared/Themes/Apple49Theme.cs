using MudBlazor;

namespace LPNEWSTORE.Shared.Themes
{
    public class Apple49Theme :MudTheme
    {
        public Apple49Theme()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#86868B",          // mismo Primary, gris medio
                Secondary = "#007AFF",        // mismo Secondary, azul brillante
                Background = "#FFFFFF",       // fondo claro blanco
                AppbarBackground = "#F0F0F5", // appbar más claro que drawer para contraste
                DrawerBackground = "#E0E0E5", // drawer claro, gris muy suave
                DrawerText = "#222222",       // texto oscuro para buen contraste
                DrawerIcon = "#007AFF",       // mismo azul para iconos
                Surface = "#F7F7FA",          // superficie muy clara
                AppbarText = "#444444"    ,    // texto appbar gris oscuro
                TableLines = "#007AFF",
                TableHover = "#007AFF"
            };
            PaletteDark = new PaletteDark
            {
                Primary = "#86868B",           
                Secondary = "#007AFF",         
                Background = "#1D1D1F",
                
                AppbarBackground = "#86868B",
                DrawerBackground = "#86868B",
                DrawerText = "#FFFFFF",
                DrawerIcon = "#007AFF",
                Surface = "#2C3138",
                AppbarText = "#C0C0C0",
                TableLines = "#007AFF",
                TableHover = "#007AFF"
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
