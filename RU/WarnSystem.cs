using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Reflection;
using CSTimers = CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Newtonsoft.Json.Linq;

namespace WarnSystemPlugin
{
    public class AWarnSystemPlugin : BasePlugin
    {
        public override string ModuleName => "WarnSystem";
        public override string ModuleVersion => "1.0";
        private string _warnsConfigFilePath;
        private PluginConfig _config;
        private Dictionary<string, int> _playerWarnings = new Dictionary<string, int>();
        private Dictionary<CCSPlayerController, string> playerCenterMessages = new Dictionary<CCSPlayerController, string>();
        private Dictionary<CCSPlayerController, CSTimers.Timer> playerMessageTimers = new Dictionary<CCSPlayerController, CSTimers.Timer>();             

        public override void Load(bool hotReload)
        {
            _warnsConfigFilePath = Path.Combine(ModuleDirectory, "warns.json");
            LoadWarnsConfig();  
            LoadConfig();
            AddAdminCommands();          
            RegisterListener<Listeners.OnTick>(() =>
            {
                foreach (var kvp in playerCenterMessages)
                {
                    var player = kvp.Key;
                    var message = kvp.Value;
                    if (player != null && player.IsValid)
                    {
                        player.PrintToCenterHtml(message);
                    }
                }
            });          
        }
        private void SaveConfig(PluginConfig config, string filePath)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("# Конфигурационный файл для WarnSystem");
            stringBuilder.AppendLine("# Команда для бана игроков");
            AppendConfigValue(stringBuilder, nameof(config.BanCommand), config.BanCommand);

            stringBuilder.AppendLine("# Максимальное количество предупреждений до бана");
            AppendConfigValue(stringBuilder, nameof(config.MaxWarningsBeforeBan), config.MaxWarningsBeforeBan);

            stringBuilder.AppendLine("# Продолжительность бана в секундах");
            AppendConfigValue(stringBuilder, nameof(config.BanDuration), config.BanDuration);

            stringBuilder.AppendLine("# Причина бана");
            AppendConfigValue(stringBuilder, nameof(config.BanReason), config.BanReason);

            stringBuilder.AppendLine("# Сообщение о бане");
            AppendConfigValue(stringBuilder, nameof(config.BanMessage), config.BanMessage);

            stringBuilder.AppendLine("# Сообщение о предупреждении");
            AppendConfigValue(stringBuilder, nameof(config.WarningMessage), config.WarningMessage);

            stringBuilder.AppendLine("# Сообщение о снятии предупреждений");
            AppendConfigValue(stringBuilder, nameof(config.WarningsClearedMessage), config.WarningsClearedMessage);

            stringBuilder.AppendLine("# Сообщение администратору о снятии предупреждений");
            AppendConfigValue(stringBuilder, nameof(config.AdminWarningsClearedMessage), config.AdminWarningsClearedMessage);

            stringBuilder.AppendLine("# Сообщение администратору, если у игрока нет предупреждений");
            AppendConfigValue(stringBuilder, nameof(config.NoWarningsMessage), config.NoWarningsMessage);

            stringBuilder.AppendLine("# Центрированное сообщение о предупреждении");
            AppendConfigValue(stringBuilder, nameof(config.WarningCenterMessage), config.WarningCenterMessage);

            File.WriteAllText(filePath, stringBuilder.ToString());
        }
        private void AppendConfigValue(StringBuilder stringBuilder, string key, object value)
        {
            var valueStr = value?.ToString() ?? string.Empty;
            stringBuilder.AppendLine($"{key}: \"{EscapeMessage(valueStr)}\"");
        }
        private string EscapeMessage(string message)
        {
            return message.Replace("\"", "\\\""); 
        }
        private void LoadConfig()
        {
            string configFilePath = Path.Combine(ModuleDirectory, "Config.yml");
            if (!File.Exists(configFilePath))
            {
                _config = new PluginConfig();
                SaveConfig(_config, configFilePath); 
            }
            else
            {
                string yamlConfig = File.ReadAllText(configFilePath);
                var deserializer = new DeserializerBuilder().Build();
                _config = deserializer.Deserialize<PluginConfig>(yamlConfig) ?? new PluginConfig();
            }
        }
        private void LoadWarnsConfig()
        {
            if (File.Exists(_warnsConfigFilePath))
            {
                string json = File.ReadAllText(_warnsConfigFilePath);
                _playerWarnings = JsonSerializer.Deserialize<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
            }
        }
        private void SaveWarnsConfig()
        {
            string json = JsonSerializer.Serialize(_playerWarnings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_warnsConfigFilePath, json);
        }        
        private void SaveJsonFile(string filepath, JObject data)
        {
            File.WriteAllText(filepath, data.ToString());
        }
        private void AddAdminCommands()
        { 
            AddCommand("warn", "Warn a player", AdminWarnCommand); 
            AddCommand("unwarn", "Remove warnings from a player", AdminRemoveWarnCommand);                  
        }
        [RequiresPermissions("@admin/warn")]
        private void AdminWarnCommand(CCSPlayerController? caller, CommandInfo info)
        {
            if (caller == null) return;

            var playerSelectMenu = new ChatMenu("Select a player to warn");
            foreach (var player in Utilities.GetPlayers())
            {
                string playerName = player.PlayerName;
                playerSelectMenu.AddMenuOption(playerName, (admin, option) => WarnPlayer(player));
            }
            ChatMenus.OpenMenu(caller, playerSelectMenu);
        }
        private void WarnPlayer(CCSPlayerController playerToWarn)
        {
            string steamId = new SteamID(playerToWarn.SteamID).SteamId64.ToString();

            if (!_playerWarnings.ContainsKey(steamId))
            {
                _playerWarnings[steamId] = 0;
            }

            _playerWarnings[steamId]++;
            int warningsLeft = _config.MaxWarningsBeforeBan - _playerWarnings[steamId];

            SaveWarnsConfig();

            string centerMessage = _config.WarningCenterMessage
                .Replace("{Warnings}", _playerWarnings[steamId].ToString())
                .Replace("{WarningsLeft}", warningsLeft.ToString());
            ShowCenterMessage(playerToWarn, centerMessage, 10);

            if (_playerWarnings[steamId] >= _config.MaxWarningsBeforeBan)
            {
                BanPlayer(steamId, _config.BanDuration);
                _playerWarnings[steamId] = 0;
                SaveWarnsConfig();
                string banMessage = _config.BanMessage.Replace("{PlayerName}", playerToWarn.PlayerName);
                Server.PrintToChatAll(ReplaceColorPlaceholders(banMessage));
            }
            else
            {
                string warningMessage = _config.WarningMessage
                    .Replace("{PlayerName}", playerToWarn.PlayerName)
                    .Replace("{Warnings}", _playerWarnings[steamId].ToString())
                    .Replace("{WarningsLeft}", warningsLeft.ToString());
                Server.PrintToChatAll(ReplaceColorPlaceholders(warningMessage));
            }
        }
        [RequiresPermissions("@admin/unwarn")]
        private void AdminRemoveWarnCommand(CCSPlayerController? caller, CommandInfo info)
        {
            if (caller == null) return;

            var playerSelectMenu = new ChatMenu("Select a player to remove warnings");
            foreach (var player in Utilities.GetPlayers())
            {
                string playerName = player.PlayerName;
                playerSelectMenu.AddMenuOption(playerName, (admin, option) => RemoveWarnings(player, caller));  
            }
            ChatMenus.OpenMenu(caller, playerSelectMenu);
        }
        private void RemoveWarnings(CCSPlayerController playerToClearWarnings, CCSPlayerController? caller)
        {
            string steamId = new SteamID(playerToClearWarnings.SteamID).SteamId64.ToString();

            if (_playerWarnings.ContainsKey(steamId) && _playerWarnings[steamId] > 0)
            {
                _playerWarnings[steamId] = 0;
                SaveWarnsConfig(); 

                if (playerToClearWarnings != null) 
                {
                    ShowCenterMessage(playerToClearWarnings, _config.WarningsClearedMessage, 10);
                }

                if (caller != null) 
                {
                    string adminMessage = _config.AdminWarningsClearedMessage.Replace("{PlayerName}", playerToClearWarnings.PlayerName);
                    caller.PrintToChat(ReplaceColorPlaceholders(adminMessage));
                }
            }
            else
            {
                if (caller != null) 
                {
                    string noWarningsMessage = _config.NoWarningsMessage.Replace("{PlayerName}", playerToClearWarnings.PlayerName);
                    caller.PrintToChat(ReplaceColorPlaceholders(noWarningsMessage));
                }
            }
        }

        private void BanPlayer(string steamId, int durationInSeconds)
        {
            string command = string.Format(_config.BanCommand, steamId, _config.BanDuration, _config.BanReason);

            Server.ExecuteCommand(command);
        }
        public void ShowCenterMessage(CCSPlayerController player, string message, float durationInSeconds)
        {
            if (playerMessageTimers.TryGetValue(player, out var existingTimer))
            {
                existingTimer.Kill();
            }

            playerCenterMessages[player] = $"<font color='red' class='fontSize-l'>{message}</font>";

            var messageTimer = AddTimer(durationInSeconds, () =>
            {
                playerCenterMessages.Remove(player);
                playerMessageTimers.Remove(player);
            }, CSTimers.TimerFlags.REPEAT);

            playerMessageTimers[player] = messageTimer;
        }
        private string ReplaceColorPlaceholders(string message)
        {
            if (message.Contains('{'))
            {
                string modifiedValue = message;
                foreach (FieldInfo field in typeof(ChatColors).GetFields())
                {
                    string pattern = $"{{{field.Name}}}";
                    if (message.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                    {
                        modifiedValue = modifiedValue.Replace(pattern, field.GetValue(null).ToString(), StringComparison.OrdinalIgnoreCase);
                    }
                }
                return modifiedValue;
            }

            return message;
        }        
        public class PluginConfig
        {
            public string BanCommand { get; set; } = "mm_ban {0} {1} {2}";
            public int MaxWarningsBeforeBan { get; set; } = 3;
            public int BanDuration { get; set; } = 600;
            public string BanReason { get; set; } = "Множественные предупреждения";
            public string BanMessage { get; set; } = "[{Red}ADMIN{White}] Игрок {Red}{PlayerName}{White} был забанен за множественные предупреждения.";
            public string WarningMessage { get; set; } = "[{Red}ADMIN{White}] Игрок {Red}{PlayerName}{White} получил предупреждение. Текущее количество предупреждений: {Red}{Warnings}{White}. Осталось до бана: {Red}{WarningsLeft} {White}предупреждений.";
            public string WarningCenterMessage { get; set; } = "<font color='red' class='fontSize-l'>Вы получили предупреждение. У вас теперь {Warnings} предупреждений. Осталось до бана: {WarningsLeft} предупреждений.</font>";
            public string WarningsClearedMessage { get; set; } = "<font color='green' class='fontSize-l'>Ваши предупреждения сняты.</font>";
            public string AdminWarningsClearedMessage { get; set; } = "[{Red}ADMIN{White}] Предупреждения игрока {Green}{PlayerName}{White} сняты.";
            public string NoWarningsMessage { get; set; } = "[{Red}ADMIN{White}] У игрока {Green}{PlayerName}{White} нет предупреждений.";
        
        }
        public AWarnSystemPlugin()
        {
            _warnsConfigFilePath = string.Empty; 
            _config = new PluginConfig(); 
        }        
    }
}
