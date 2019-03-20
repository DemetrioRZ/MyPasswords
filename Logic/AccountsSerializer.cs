using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Interfaces.Logic;
using Model;

namespace Logic
{
    public class AccountsSerializer : IAccountsSerializer
    {
        private readonly DataContractJsonSerializer _jsonSerializer = new DataContractJsonSerializer(typeof(ICollection<Account>));

        public async Task<string> SerializeAsync(ICollection<Account> accounts)
        {
            return await Task.Run(() =>
            {
                using (var ms = new MemoryStream())
                {
                    _jsonSerializer.WriteObject(ms, accounts);
                    using (var sr = new StreamReader(ms))
                    {
                        return sr.ReadToEnd();
                    }
                }
            });
        }

        public async Task<ICollection<Account>> DeserializeAsync(string json)
        {
            return await Task.Run(() =>
            {
                using (var ms = new MemoryStream())
                {
                    using (var sw = new StreamWriter(ms))
                    {
                        sw.Write(json);

                        if (_jsonSerializer.ReadObject(ms) is ICollection<Account> accounts)
                            return accounts;

                        return new List<Account>();
                    }
                }
            });
        }
    }
}