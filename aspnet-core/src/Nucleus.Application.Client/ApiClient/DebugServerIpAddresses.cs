namespace Nucleus.ApiClient
{
    public static class DebugServerIpAddresses
    {
        //You can use Emulators.Android, Emulators.Gennymotion or LocalhostIp based on your needs.
        public static string Current => LocalhostIp;

        //You can configure your computer's IP adress for external access (if Current = LocalhostIp)
        private const string LocalhostIp = Emulators.Android;

        //Loopback addresses by emulators.
        private static class Emulators
        {
            public const string Android = "192.168.1.206";
            public const string Gennymotion = "10.0.3.2";
        }
    }
}