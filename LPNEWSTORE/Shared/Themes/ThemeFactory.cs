using Entities;
using MudBlazor;

namespace LPNEWSTORE.Shared.Themes
{
    public static class ThemeFactory
    {
        public static MudTheme CrearTema(EmpresaTema tema)
        {
            if (tema == null)
                return new RoyalTechTheme();

            var primary = ColorOrDefault(tema.PrimaryColor, "#1EA7FF");
            var secondary = ColorOrDefault(tema.SecondaryColor, "#57D4FF");
            var appbar = ColorOrDefault(tema.AppbarColor, tema.ModoOscuro ? "#0F1B33" : "#EAF6FF");
            var drawer = ColorOrDefault(tema.DrawerColor, tema.ModoOscuro ? "#0D172B" : "#F4FAFF");

            return new MudTheme
            {
                PaletteLight = new PaletteLight
                {
                    Primary = primary,
                    Secondary = secondary,
                    Background = "#F7FBFF",
                    Surface = "#FFFFFF",

                    AppbarBackground = appbar,
                    AppbarText = "#0B1F3A",

                    DrawerBackground = drawer,
                    DrawerText = "#0B1F3A",
                    DrawerIcon = primary,

                    LinesDefault = "#B7E3FF",
                    TableLines = "#B7E3FF",
                    TableHover = "#E3F4FF"
                },

                PaletteDark = new PaletteDark
                {
                    Primary = primary,
                    Secondary = secondary,
                    Background = "#0A0F1F",
                    Surface = "#10182B",

                    AppbarBackground = appbar,
                    AppbarText = "#D9F4FF",

                    DrawerBackground = drawer,
                    DrawerText = "#EAF9FF",
                    DrawerIcon = secondary,

                    LinesDefault = primary,
                    TableLines = primary,
                    TableHover = "#163A5A"
                },

                Typography = new Typography
                {
                    Default = new DefaultTypography
                    {
                        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" }
                    }
                }
            };
        }

        private static string ColorOrDefault(string color, string fallback)
        {
            return string.IsNullOrWhiteSpace(color) ? fallback : color;
        }
    }
}