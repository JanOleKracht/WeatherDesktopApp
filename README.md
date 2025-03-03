# WeatherDesktopApp 🌦️

## Description

WeatherApp is a beginner-friendly **C# WPF** application that fetches real-time weather data using the **OpenWeatherMap API**. The project demonstrates how to work with APIs, handle **JSON responses**, use asynchronous programming, and integrate a graphical user interface (GUI) with WPF.

This project is one of my first learning experiences with APIs and C#, primarily created to help me understand backend development and asynchronous programming.

**Note:** The GUI is not yet fully developed, as the primary focus has been on the backend logic for learning purposes.


## Features

- Fetches and displays current weather conditions (temperature, humidity, description, etc.)

- Shows a 5-day weather forecast

- Converts UTC time to local time based on city timezone

- Supports searching for different cities

- Error handling for invalid city names and network issues

- Background image changes dynamically based on the current weather conditions of the selected city


## Technologies Used

- C# (WPF for UI)

- OpenWeatherMap API (for weather data)

- Newtonsoft.Json (for JSON parsing)

- Async/Await (for non-blocking API calls)


## Installation

### Prerequisites

- .NET Framework / .NET Core (depending on your setup)

- Visual Studio (or any C# IDE)


## etup Steps

**1. Clone the repository:**

- git clone https://github.com/JanOleKracht/WeatherDesktopApp.git
  

**2. Set up an OpenWeather API Key:**

- Go to [OpenWeather](https://openweathermap.org/api) and create a free account.

- Get your API key.

- Insert your API into  ```private string apiKey```
  

**3. Open the project in Visual Studio.**

**4. Run the application by pressing F5.**


## Usage

**1. Enter** a city name in the search bar.

**2.** Press **Enter** or **click** the search button.

**3.** The current weather details and 5-day forecast will be displayed.


## Screenshots
<img src="https://github.com/user-attachments/assets/a9ba56d1-58d6-4179-a749-4f6c1a4f4eb3" width="550">


<img src="https://github.com/user-attachments/assets/8a8a6358-95d2-4246-bbda-a24437a73f20" width="550">


<img src="https://github.com/user-attachments/assets/a6e80d21-3d69-47e6-8790-0b3b72272f23" width="550">



## Future Improvements

- Implement MVVM architecture for better separation of concerns

- Add unit tests for API calls and UI

- Improve UI design with better visuals

- Allow users to choose between Celsius and Fahrenheit

## Contributing
This is a personal learning project, so contributions are not expected. However, if you have any feedback or suggestions, feel free to reach out.

## License
This project is licensed under the MIT License. You are free to use, modify, and distribute this project as long as you include the license and give credit to the original author.

## Developer
GitHub Username: JanOleKracht













