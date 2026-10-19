namespace Maiamnew.Infrastructure
{
    /// <summary>
    /// Institutional details migrated from maiam.co.in. Single source of truth for the
    /// header, footer, contact page and structured data.
    /// </summary>
    public static class SiteInfo
    {
        public const string BaseUrl = "https://maiam.co.in";
        public const string ShortName = "MAIAM";
        public const string Name = "Manappuram Institute of Automotive Skills";
        public const string Tagline = "Empowering Dreams, Shaping the Future of Automotive Careers";
        public const int EstablishedYear = 2019;

        // Campus address
        public const string AddressLine1 = "Manappuram Agro Farms, Painoor";
        public const string AddressLine2 = "Valapad, Thrissur";
        public const string Locality = "Valapad";
        public const string District = "Thrissur";
        public const string State = "Kerala";
        public const string PostalCode = "680567";
        public const double Latitude = 10.400595;
        public const double Longitude = 76.126489;

        // Contact
        public const string Phone1Display = "+91 75948 10851";
        public const string Phone1Tel = "+917594810851";
        public const string Phone2Display = "+91 90726 23103";
        public const string Phone2Tel = "+919072623103";
        public const string TollFreeDisplay = "1800 123 6866";
        public const string TollFreeTel = "18001236866";
        public const string Email = "maiam@manappuramfoundation.org";
        public const string WhatsAppUrl = "https://api.whatsapp.com/send/?phone=917594810851&text=Hi";

        // Maps
        public const string MapEmbedUrl = "https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d1962.131996969135!2d76.126489!3d10.400595!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x6df8ddd3cf5f6588!2sManappuram%20institute%20of%20automotive%20skills%20(MAIAM)!5e0!3m2!1sen!2sin!4v1649223454750!5m2!1sen!2sin";
        public const string DirectionsUrl = "https://www.google.com/maps/dir/?api=1&destination=10.400595%2C76.126489";

        // MAIAM social channels
        public const string FacebookUrl = "https://www.facebook.com/manappuramauto.org";
        public const string InstagramUrl = "https://www.instagram.com/maiam_institute/";
        public const string YouTubeUrl = "https://www.youtube.com/channel/UCy2WeIo9l8cL2EiHKbLxMMQ";
        public const string TwitterUrl = "https://twitter.com/MaAutomotive";

        // Managing organisation
        public const string FoundationName = "Manappuram Foundation";
        public const string FoundationDescriptor = "CSR Body of Manappuram Finance Ltd.";
        public const string FoundationAddress = "Ummaih Complex, Chandapady, Near Police Station, Valapad";
        public const string FoundationTollFreeDisplay = "1800 123 6866";
        public const string FoundationTollFreeTel = "18001236866";
        public const string FoundationEmail = "crmhead@manappuramfoundation.org";
        public const string FoundationFacebookUrl = "https://www.facebook.com/manappuramfoundation.org";
        public const string FoundationInstagramUrl = "https://www.instagram.com/manappuram_foundation/";
        public const string FoundationTwitterUrl = "https://twitter.com/theMAFDN";
        public const string FoundationYouTubeUrl = "https://www.youtube.com/channel/UCJSG6xSK72corpzGatIPM2g";

        public static string AbsoluteUrl(string path) => BaseUrl + (path.StartsWith('/') ? path : "/" + path);
    }
}
