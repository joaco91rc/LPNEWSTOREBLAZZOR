namespace LPNEWSTORE.Utils
{
    public static class PrecioHelper
    {
        public record PreciosCalculados(decimal PrecioEfectivo, decimal Precio3Cuotas, decimal Precio6Cuotas);

        public static PreciosCalculados CalcularDesdeLista(decimal precioLista)
        {
            if (precioLista < 0)
                throw new ArgumentOutOfRangeException(nameof(precioLista));

            var efectivo = Math.Round(precioLista * 0.85m, 2, MidpointRounding.AwayFromZero);
            var tres = Math.Round(precioLista / 3m, 2, MidpointRounding.AwayFromZero);
            var seis = Math.Round(precioLista / 6m, 2, MidpointRounding.AwayFromZero);

            return new PreciosCalculados(efectivo, tres, seis);
        }
    }
}
