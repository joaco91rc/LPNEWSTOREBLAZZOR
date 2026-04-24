using MudBlazor;

namespace LPNEWSTORE.Shared.Themes
{
    public class RoyalTechTheme : MudTheme
    {
        public RoyalTechTheme()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#1EA7FF",           // azul royal tech
                Secondary = "#57D4FF",         // cian brillante
                Background = "#F7FBFF",        // fondo claro con tono frío
                Surface = "#FFFFFF",

                AppbarBackground = "#EAF6FF",  // celeste muy suave
                AppbarText = "#0B1F3A",        // azul oscuro

                DrawerBackground = "#F4FAFF",
                DrawerText = "#0B1F3A",
                DrawerIcon = "#1EA7FF",

                LinesDefault = "#B7E3FF",
                TableLines = "#B7E3FF",
                TableHover = "#E3F4FF"
            };

            PaletteDark = new PaletteDark
            {
                Primary = "#1EA7FF",           // azul principal
                Secondary = "#57D4FF",         // cian brillante
                Background = "#0A0F1F",        // negro azulado tipo espacio
                Surface = "#10182B",

                AppbarBackground = "#0F1B33",  // azul oscuro profundo
                AppbarText = "#D9F4FF",

                DrawerBackground = "#0D172B",
                DrawerText = "#EAF9FF",
                DrawerIcon = "#57D4FF",

                LinesDefault = "#1EA7FF",
                TableLines = "#1EA7FF",
                TableHover = "#163A5A"
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
