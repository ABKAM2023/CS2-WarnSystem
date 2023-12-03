# CS2-WarnSystem
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

# Команды
- `!warn` выдать предупреждение игроку.
- `!unwarn` очистить предупреждения игрока.


