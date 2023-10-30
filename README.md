# Тестовое задание KASPERSKY №2 (усложненная версия)

## Условие

Требуется реализовать два приложения, используя .NET Core / .NET:
- сервисное приложение, реализующее простой REST API, который предоставляет ресурс для создания задач на сканирование файлов в директории и получения статуса задачи по идентификатору
- утилиту, работающую из командной строки, отправляющую сервисному приложению команды на создание и просмотр состояния задач.

В рамках задачи определено 3 типа "подозрительного" содержимого в файле:
- файл с расширением .js, содержащий строку <script>evil_script()</script>
- любой файл, содержащий строку: rm -rf %userprofile%\Documents
- любой файл, содержащий строку: Rundll32 sys.dll SysEntry

После завершения команды создание задачи на сканирование должен быть выведен уникальный идентификатор задачи.
После завершения команды просмотра статуса задачи может быть выведено два результата: задача еще выполняется или отчет о сканировании, в котором присутствует следующая информация:

- путь к директории, сканирование которой производилось
- общее количество обработанных файлов
- количество обнаружений на каждый тип "подозрительного" содержимого
- количество ошибок анализа файлов (например, не хватает прав на чтение файла)
- время выполнения утилиты.

Пример запуска сервиса и исполнения утилиты из командной строки:
```
> scan_service
```
> Scan service was started.
> Press <Enter> to exit...

```
> scan_util scan %userprofile%\Documents
```
> Scan task was created with ID: 1234

```
> scan_util status 1234
```
> Scan task in progress, please wait

```
> scan_util status 1234
```

> ====== Scan result ======  
> Directory: C:\Users\TestUser\Documents  
> Processed files: 150  
> JS detects: 5  
> rm -rf detects: 1   
> Rundll32 detects: 2  
> Errors: 1  
> Exection time: 00:00:31  
>
> ==================

**Примечание:**
- сервисное приложение не имеет постоянного хранилища состояния (каждый запуск как чистый лист)
- в каждом файле может присутствовать только один тип "подозрительного" содержимого
- сервисное приложение и утилита работают на одном и том же устройстве
- рекомендуется максимальное использования (утилизация) вычислительных ресурсов устройства, на котором выполняется утилита.

## Руководство по использованию

- Клонировать репозиторий себе на ПК
- Открыть решение в любой IDE, поддерживающей язык C#
- Запустить проект `scan_service`
- В файле `Program.cs` проекта `scan_util` присвоить переменной `baseUrl` адрес, на котором располагается `scan_service`
- "Собрать" (build) проект `scan_util`
- Добавить путь до директории с файлом `scan_util.exe` в `PATH`

После чего в коммандную строку (запущенной от имени администратора) можно будет вводить следующего рода комманды:
```
scan_util scan C:\Users\user\Documents
scan_util status 1234
```

## Примечания

Должен уточнить следующее:
- Программа не может корректно обрабатывать строки формата `%<something>%`
- API должно быть запущено вручную, утилита же запускается через коммандную строку