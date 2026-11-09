TOODED EFCORE — ЗАПУСК

В архиве исходный проект WinForms (.NET 8), две миграции EF Core 8,
профиль публикации win-x64 и проект установщика ToodedInstaller.
Код форм взят из задания; добавлены итоговые изменения из части 6.
Интерфейс и названия классов сохранены на эстонском.

1. Что установить
Windows x64, Visual Studio 2022 с поддержкой .NET 8 и компонентом
«.NET desktop development». Для базы нужен SQL Server Express LocalDB
(экземпляр MSSQLLocalDB). Это вариант A из пункта 7.1 задания.
Для проекта установщика нужна Microsoft Visual Studio Installer Projects 2022.

2. Как запустить
Распаковать ZIP. Открыть Tooded_EFCore.sln в Visual Studio.
Дождаться восстановления NuGet-пакетов.
Сделать Tooded_EFCore стартовым проектом и нажать F5.
При первом запуске Database.Migrate() создаёт базу Tooded_DB,
таблицы и три категории. После этого добавляются два пользователя.
Миграции уже включены: повторять Add-Migration не нужно.
При необходимости их можно применить через PMC: Update-Database.

Вход:
admin@admin.ee    / admin123
kasutaja@pood.ee  / kasutaja123

Сначала войти как admin и добавить товары через Tootehaldus.
Тестовые товары не добавлены, потому что их нет в начальных данных задания.

3. Отправка почты
В Tooded_EFCore\ArveForm.cs заменить значения saatja и rakenduseParool
на свой Gmail и пароль приложения Google, как в пункте 4.2 задания.
Обычный пароль Gmail не подходит. Настоящий пароль в архив не добавлен.
Перед загрузкой проекта на GitHub снова заменить пароль заглушкой.
Для отправки сначала добавить товары в корзину и нажать Genereeri PDF.

4. Установщик
Профиль Properties\PublishProfiles\FolderProfile.pubxml настроен:
Release, win-x64, Self-contained. .NET входит в публикацию.
LocalDB нужно установить на целевом компьютере отдельно.

В Visual Studio:
— ПКМ по Tooded_EFCore → Publish → FolderProfile → Publish.
— Выбрать конфигурацию Release.
— ПКМ по ToodedInstaller → Build.
Результат сборки: ToodedInstaller\Release\ToodedInstaller.msi и setup.exe.
Установщик содержит Publish Items, иконку и ярлыки в меню Пуск
и на рабочем столе. Удаление: Settings → Apps → Tooded App → Uninstall.

5. Где находятся данные
Изображения: %LOCALAPPDATA%\ToodedApp\Images
Счета:       %LOCALAPPDATA%\ToodedApp\Arved
База:        (localdb)\MSSQLLocalDB, Tooded_DB

ПРОВЕРКА ПЕРЕД СДАЧЕЙ
Проверены синтаксис C# и структура файлов проекта.
Компиляция и запуск здесь не выполнялись: среда Linux без .NET SDK,
Windows, LocalDB и Visual Studio. Готовых EXE, MSI и setup.exe в архиве нет.
Проект установщика также требует проверки сборкой в Visual Studio.
Отправка письма не проверена без данных Gmail.

На Windows нужно пройти контрольный список из задания:
— первый запуск создаёт категории и пользователей;
— admin добавляет, изменяет и удаляет товар с картинкой и без неё;
— неверные количество и цена показывают сообщение;
— магазин показывает категории, картинки и недоступность товара с остатком 0;
— корзина сохраняется между окнами, PDF показывает правильную сумму;
— письмо приходит с PDF во вложении;
— пользователь kasutaja не может открыть управление товарами;
— закрытие меню завершает приложение;
— установщик работает на другом компьютере без Visual Studio,
  но с установленным LocalDB; оба ярлыка запускают приложение.
