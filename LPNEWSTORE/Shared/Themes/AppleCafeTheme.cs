using MudBlazor;

namespace LPNEWSTORE.Shared.Themes
{
   

    public class AppleCafeTheme : MudTheme
    {
        public AppleCafeTheme()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#cbc0ad",          // Mantengo el beige claro como color principal
                Secondary = "#a99d89",        // Un marrón suave para secundario (más oscuro que el secundario dark)
                Background = "#f5f2e7",       // Fondo claro, crema muy suave
                AppbarBackground = "#FFFFFF", // Appbar en beige claro
                Surface = "#f0ece1",          // Superficies claras y suaves (similar al secundario dark)
                AppbarText = "#cbc0ad",
                DrawerBackground = "#FFFFFF", // Fondo del drawer beige claro
                DrawerText = "#4b4436",       // Texto del drawer en marrón oscuro para buen contraste
                DrawerIcon = "#8b7d6b",       // Íconos en marrón medio, que destaque pero armonice con texto

                LinesDefault = "#d3cdbf",     // Líneas suaves claras
                TableLines = "#c3bfae" ,       // Líneas de tabla también suaves
                TableHover= "#8b7d6b"
            };

            PaletteDark = new PaletteDark
            {
                Primary = "#cbc0ad",
                Secondary = "#e6e1d7",
                Background = "#292627",
                AppbarBackground = "#292627",
                Surface = "#3a3738",
                AppbarText = "#cbc0ad",
                DrawerBackground = "#292627",
                DrawerText = "#cbc0ad",
                DrawerIcon = "#bfa76f",
                LinesDefault = "#cbc0ad",
                TableLines = "#cbc0ad",
                TableHover = "#8b7d6b"
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
