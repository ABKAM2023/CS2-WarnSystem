![GitHub Repo stars](https://img.shields.io/github/stars/ABKAM2023/CS2-WarnSystem?style=for-the-badge)
![GitHub issues](https://img.shields.io/github/issues/ABKAM2023/CS2-WarnSystem?style=for-the-badge)
![GitHub contributors](https://img.shields.io/github/contributors/ABKAM2023/CS2-WarnSystem?style=for-the-badge)
![GitHub all releases](https://img.shields.io/github/downloads/ABKAM2023/CS2-WarnSystem/total?style=for-the-badge)

# CS2-WarnSystem

# EN
The WarnSystem warning system provides functionality for issuing warnings to players who violate the rules of the game server. Additionally, when the maximum number of warnings set by the server administrators is reached, the system automatically applies a blocking measure (ban) to that player. This helps maintain order and cleanliness on the server, ensuring a safe and comfortable gaming environment for all participants.

# Installation
Install Metamod:Source and CounterStrikeSharp. To enable the ban functionality, you need CS2 Admin System.
Download WarnSystem.
Extract the archive and upload it to your game server.
How to Access Commands?
Open the "addons/counterstrikesharp/configs" directory.
Find the "admins.json" file in the specified folder.
Inside the "admins.json" file, add the necessary flags for access to commands. For example:
```
"76561198847871713": {
    "identity": "76561198847871713",
    "flags": [
        "@admin/warn",
        "@admin/unwarn"
    ],
    "immunity": 100
}
```
After doing this, you will have access to the commands.

# Main Configuration (Config.yml)
```
# Configuration file for WarnSystem
# Command for banning players
BanCommand: "mm_ban {0} {1} {2}"
# Maximum number of warnings before a ban
MaxWarningsBeforeBan: "3"
# Ban duration in seconds
BanDuration: "600"
# Ban reason
BanReason: "Multiple Warnings"
# Ban message
BanMessage: "[{Red}ADMIN{White}] Player {Red}{PlayerName}{White} has been banned for multiple warnings."
# Warning message
WarningMessage: "[{Red}ADMIN{White}] Player {Red}{PlayerName}{White} has received a warning. Current number of warnings: {Red}{Warnings}{White}. Warnings remaining until ban: {Red}{WarningsLeft} {White}warnings."
# Warnings cleared message
WarningsClearedMessage: "<font color='green' class='fontSize-l'>Your warnings have been cleared.</font>"
# Administrator warnings cleared message
AdminWarningsClearedMessage: "[{Red}ADMIN{White}] Player {Green}{PlayerName}{White}'s warnings have been cleared."
# Administrator message when a player has no warnings
NoWarningsMessage: "[{Red}ADMIN{White}] Player {Green}{PlayerName}{White} has no warnings."
# Centered warning message
WarningCenterMessage: "<font color='red' class='fontSize-l'>You have received a warning. You now have {Warnings} warnings. Warnings remaining until a ban: {WarningsLeft} warnings.</font>"
```

# From the player's side
![image](https://github.com/ABKAM2023/CS2-WarnSystem/assets/149762275/9befcda5-10dd-42fa-a1c5-0b79bca5e8a6)
![image](https://github.com/ABKAM2023/CS2-WarnSystem/assets/149762275/ca716e10-6a14-45aa-8647-7e7e3bedee4e)

# Commands
`!warn` - issue a warning to a player.
`!unwarn` - clear a player's warnings.

#

# RU
Система предупреждений WarnSystem предоставляет функционал для выдачи предупреждений игрокам, нарушившим правила игрового сервера. Кроме того, при достижении максимального количества предупреждений, установленного администраторами сервера, система автоматически применяет меру блокировки (бана) к данному игроку. Это помогает поддерживать порядок и чистоту на сервере, обеспечивая безопасную и комфортную игровую среду для всех участников.

# Установка
1. Установите [Metamod:Source](https://www.sourcemm.net/downloads.php/?branch=master) и [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp). Для работы бана [CS2 Admin System](https://csdevs.net/resources/cs2-admin-system.424/)
2. Скачайте WarnSystem
3. Распакуйте архив и загрузите его на игровой сервер

# Как получить доступ к командам?
1. Откройте директорию "addons/counterstrikesharp/configs".
2. Найдите файл "admins.json" в указанной папке.
3. Внутри файла "admins.json" добавьте необходимые флаги для доступа к командам. Например:
```
"76561198847871713": {
    "identity": "76561198847871713",
    "flags": [
        "@admin/warn",
        "@admin/unwarn"
    ],
    "immunity": 100
}
```
После этого вам будет предоставлен доступ к командам 

# Основной конфиг (Config.yml)
```
# Конфигурационный файл для WarnSystem
# Команда для бана игроков
BanCommand: "mm_ban {0} {1} {2}"
# Максимальное количество предупреждений до бана
MaxWarningsBeforeBan: "3"
# Продолжительность бана в секундах
BanDuration: "600"
# Причина бана
BanReason: "Множественные предупреждения"
# Сообщение о бане
BanMessage: "[{Red}ADMIN{White}] Игрок {Red}{PlayerName}{White} был забанен за множественные предупреждения."
# Сообщение о предупреждении
WarningMessage: "[{Red}ADMIN{White}] Игрок {Red}{PlayerName}{White} получил предупреждение. Текущее количество предупреждений: {Red}{Warnings}{White}. Осталось до бана: {Red}{WarningsLeft} {White}предупреждений."
# Сообщение о снятии предупреждений
WarningsClearedMessage: "<font color='green' class='fontSize-l'>Ваши предупреждения сняты.</font>"
# Сообщение администратору о снятии предупреждений
AdminWarningsClearedMessage: "[{Red}ADMIN{White}] Предупреждения игрока {Green}{PlayerName}{White} сняты."
# Сообщение администратору, если у игрока нет предупреждений
NoWarningsMessage: "[{Red}ADMIN{White}] У игрока {Green}{PlayerName}{White} нет предупреждений."
# Центрированное сообщение о предупреждении
WarningCenterMessage: "<font color='red' class='fontSize-l'>Вы получили предупреждение. У вас теперь {Warnings} предупреждений. Осталось до бана: {WarningsLeft} предупреждений.</font>"
```
# Со стороны игрока
![image](https://github.com/ABKAM2023/CS2-WarnSystem/assets/149762275/a5cd3f84-cdd1-49df-9bb0-fa1a3beb3140)
![image](https://github.com/ABKAM2023/CS2-WarnSystem/assets/149762275/7f0d0cd8-261a-4dd8-8205-5d0dd0a5439a)

# Команды
- `!warn` выдать предупреждение игроку.
- `!unwarn` очистить предупреждения игрока.


