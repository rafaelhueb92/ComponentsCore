namespace Components.Helpers
{

    public static class StringHelpers
    {

        public static string Encrypt(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return "";

            texto += "|97c558f6-c548-4950-b625-b5fc5c5bdfed";
            var md5 = System.Security.Cryptography.MD5.Create();
            var data = md5.ComputeHash(System.Text.Encoding.GetEncoding(0).GetBytes(texto));
            var sbString = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sbString.Append(data[i].ToString("x2"));
            return sbString.ToString();
        }

    }

}
