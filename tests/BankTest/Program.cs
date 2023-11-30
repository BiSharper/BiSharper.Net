// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using System.Text;
using BiSharper.Rv.Bank;
using BiSharper.Rv.Bank.Models;
using var md5 = MD5.Create();

var stream = File.OpenRead(@"C:\Program Files (x86)\Steam\steamapps\workshop\content\221100\3017746537\addons\CRDTN_Core.pbo");
var pbo = new FileBank(stream, "CRDTN_Core");

int count = 0;
foreach (var dataEntry in pbo.DataEntries)
{
    try
    {
        var data = dataEntry.Read();
        if (data is not null)
        {
            File.WriteAllBytes($"C:\\Program Files (x86)\\Steam\\steamapps\\workshop\\content\\221100\\3017746537\\addons\\lol\\{count}.txt", data);
            count++;
        }
    }
    catch (Exception e)
    {
    }
}




string CalculateMD5(byte[] data)
{
    var hash = md5.ComputeHash(data);
    var sb = new StringBuilder();
    foreach (var t in hash)
    {
        sb.Append(t.ToString("X2"));
    }
    return sb.ToString();
}