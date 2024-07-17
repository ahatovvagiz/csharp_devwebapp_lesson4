using System.Security.Cryptography;

namespace WebApplicationHW4.RSATools
{
    public class RSAExtensions
    {
        public static RSA GetPrivateKey()
        {
            var key = File.ReadAllText(@"../WebApplicationHW4/private_key.pem");
            var rsa = RSA.Create();

            rsa.ImportFromPem(key);
            return rsa;
        }

        public static RSA GetPublicKey()
        {
            var key = File.ReadAllText(@"../WebApplicationHW4/public_key.pem");
            var rsa = RSA.Create();

            rsa.ImportFromPem(key);
            return rsa;
        }
    }
}
