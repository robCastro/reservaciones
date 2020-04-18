namespace reservacion.Api.JwtCustomModels{
    public class JwtCustomConstants
    {
        public const string Issuer = "RerservacionApp";
        public const string Audience = "UserReservacionApp";

        //Debería estar en un lugar secreto, por restricciones de tiempo la dejo acá
        public const string secretKey = "LlaveDeAlMenos16CaracteresDeLargo";

    }
}