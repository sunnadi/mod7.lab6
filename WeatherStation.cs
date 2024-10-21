using System;
using System.Collections.Generic;

public interface IObserver
{
    void Update(float temperature);
}

public interface ISubject
{
    void RegisterObserver(IObserver observer);  
    void RemoveObserver(IObserver observer);   
    void NotifyObservers();                   
}

public class WeatherStation : ISubject
{
    private List<IObserver> observers;  
    private float temperature;          

    public WeatherStation()
    {
        observers = new List<IObserver>();
    }

    public void RegisterObserver(IObserver observer)
    {
        if (observer == null)
        {
            Console.WriteLine("Ошибка: невозможно зарегистрировать пустого наблюдателя.");
            return;
        }
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
        else
        {
            Console.WriteLine("Ошибка: попытка удалить несуществующего наблюдателя.");
        }
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(temperature);
        }
    }

    public void SetTemperature(float newTemperature)
    {
        if (newTemperature < -100 || newTemperature > 60)  
        {
            Console.WriteLine("Ошибка: некорректное значение температуры.");
            return;
        }

        Console.WriteLine($"Изменение температуры: {newTemperature}°C");
        temperature = newTemperature;
        NotifyObservers();
    }
}

public class WeatherDisplay : IObserver
{
    private string _name;

    public WeatherDisplay(string name)
    {
        _name = name;
    }

    public void Update(float temperature)
    {
        Console.WriteLine($"{_name} показывает новую температуру: {temperature}°C");
    }
}

public class EmailAlert : IObserver
{
    private string _email;

    public EmailAlert(string email)
    {
        _email = email;
    }

    public void Update(float temperature)
    {
        Console.WriteLine($"Отправка email на {_email}: температура изменилась на {temperature}°C");
    }
}

class Program
{
    static void Main(string[] args)
    {
        WeatherStation weatherStation = new WeatherStation();

        WeatherDisplay mobileApp = new WeatherDisplay("Мобильное приложение");
        WeatherDisplay digitalBillboard = new WeatherDisplay("Электронная доска");
        EmailAlert emailAlert = new EmailAlert("alert@weather.com");

        weatherStation.RegisterObserver(mobileApp);
        weatherStation.RegisterObserver(digitalBillboard);
        weatherStation.RegisterObserver(emailAlert);

        weatherStation.SetTemperature(25.0f);
        weatherStation.SetTemperature(30.0f);

        weatherStation.RemoveObserver(digitalBillboard);
        weatherStation.SetTemperature(28.0f);

        weatherStation.RemoveObserver(digitalBillboard);

        weatherStation.SetTemperature(-150.0f); 
    }
}
