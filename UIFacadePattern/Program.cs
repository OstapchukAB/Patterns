#region UML
/*
         +-----------------------+
        |       Facade        |
        |-----------------------|
        | + Operation()         |
        +-----------------------+
               /   |   \  
              /    |    \
             /     |     \
            V      V      V
+----------------+ +---------------+ +---------------+
|   SubsystemA   | |  SubsystemB   | |  SubsystemC   |
|----------------| |---------------| |---------------|
| + OperationA() | | + OperationB()| | + OperationC()|
+----------------+ +---------------+ +---------------+
 */
#endregion
#region Описание
/*
 Описание:

    SubsystemA, SubsystemB, SubsystemC – классы, которые реализуют сложную функциональность.
    Facade – класс, который предоставляет упрощённый интерфейс для работы с подсистемами. Клиент вызывает фасад, а он сам делегирует вызовы методам подсистем.
 */
#endregion
#region Пример
/*
 Ниже приведён пример реализации фасада для системы домашнего кинотеатра, где фасад упрощает управление несколькими компонентами системы:
 */
#endregion
using System;
namespace FacadePatternExample
{
    // Подсистема 1: Проигрыватель DVD
    public class DvdPlayer
    {
        public void TurnOn() => Console.WriteLine("DVD плеер включён.");
        public void Play(string movie) => Console.WriteLine($"DVD плеер воспроизводит фильм: {movie}");
        public void TurnOff() => Console.WriteLine("DVD плеер выключен.");
    }

    // Подсистема 2: Проектор
    public class Projector
    {
        public void TurnOn() => Console.WriteLine("Проектор включён.");
        public void SetInput(string source) => Console.WriteLine($"Проектор установлен на источник: {source}");
        public void TurnOff() => Console.WriteLine("Проектор выключен.");
    }

    // Подсистема 3: Аудиосистема
    public class SoundSystem
    {
        public void TurnOn() => Console.WriteLine("Аудиосистема включена.");
        public void SetVolume(int level) => Console.WriteLine($"Аудиосистема установила громкость: {level}");
        public void TurnOff() => Console.WriteLine("Аудиосистема выключена.");
    }

    // Фасад: упрощённый интерфейс для управления домашним кинотеатром
    public class HomeTheaterFacade
    {
        private readonly DvdPlayer _dvdPlayer;
        private readonly Projector _projector;
        private readonly SoundSystem _soundSystem;

        public HomeTheaterFacade(DvdPlayer dvdPlayer, Projector projector, SoundSystem soundSystem)
        {
            _dvdPlayer = dvdPlayer;
            _projector = projector;
            _soundSystem = soundSystem;
        }

        public void WatchMovie(string movie)
        {
            Console.WriteLine("Подготовка домашнего кинотеатра к просмотру фильма...\n");
            _dvdPlayer.TurnOn();
            _projector.TurnOn();
            _projector.SetInput(@"DVD");
            _soundSystem.TurnOn();
            _soundSystem.SetVolume(20);
            _dvdPlayer.Play(movie);
            Console.WriteLine("\nФильм начался!\n");
        }

        public void EndMovie()
        {
            Console.WriteLine("Завершение просмотра фильма...\n");
            _dvdPlayer.TurnOff();
            _projector.TurnOff();
            _soundSystem.TurnOff();
            Console.WriteLine("Домашний кинотеатр выключен.");
        }
    }

    class Program
    {
        static void Main()
        {
            // Создание подсистем
            DvdPlayer dvdPlayer = new DvdPlayer();
            Projector projector = new Projector();
            SoundSystem soundSystem = new SoundSystem();

            // Создание фасада, который объединяет подсистемы
            HomeTheaterFacade homeTheater = new HomeTheaterFacade(dvdPlayer, projector, soundSystem);

            // Клиентский код использует фасад для управления системой
            homeTheater.WatchMovie("Inception");
            // ... просмотр фильма ...
            homeTheater.EndMovie();
        }
    }
}
