using System;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;

class BlockVK
{
    static void Main(string[] args)
    {
        // Блокировка доступа к ВКонтакте через прокси (если нужно)
        Process.Start("cmd", "/c ping 1.1.1.1 & ping 1.0.0.1 & exit");
        try
        {
            // Ваш код, который работает с реестром
            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
            registryKey.SetValue("ProxyEnable", 1, RegistryValueKind.DWord);
            registryKey.SetValue("ProxyServer", "127.0.0.1:8080", RegistryValueKind.String);
            registryKey.Close();
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine("Ошибка доступа к реестру: " + ex.Message);
        }



        // Блокировка доступа к ВКонтакте в hosts-файле
        string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";
        string[] vkDomains = new string[]
        {
            "vk.com",
            "www.vk.com",
            "m.vk.com",
            "vk.ru",
            "m.vk.ru",
            "api.vk.com",
            "oauth.vk.com",
            "im.vk.com",
            "video.vk.com",
            "vk.me",
            "vk.cc",
            "vk.userapi.com",
            // Добавьте другие домены, если необходимо
        };

        try
        {
            using (StreamWriter writer = new StreamWriter(hostsPath, true))
            {
                foreach (var domain in vkDomains)
                {
                    string blockLine = "127.0.0.1 " + domain;
                    writer.WriteLine(blockLine);
                    Console.WriteLine($"Блокировка домена: {domain}");
                }
            }
            Console.WriteLine("Все домены ВКонтакте успешно заблокированы.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи в файл hosts: {ex.Message}");
        }
    }
}
