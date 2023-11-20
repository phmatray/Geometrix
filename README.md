# Geometrix

Geometrix is a web API that generates geometric images based on user-defined parameters. It uses a combination of mirror power, cell group length, cell width pixel, seed, and color themes to create unique and visually appealing geometric patterns.

![A generated image](geometrix-api/Geometrix.WebApi/wwwroot/Images/4-42-F-2-2-dark-indigo-64.png)

## Features

- Generate geometric images with customizable parameters
- Save generated images as PNG files
- Retrieve saved images

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- .NET 8.0 or later
- A suitable IDE such as JetBrains Rider or Visual Studio

### Installing

1. Clone the repository ```git clone https://github.com/phmatray/Geometrix.git```
2. Navigate to the project directory ```cd Geometrix```
3. Restore the packages ```dotnet restore```
4. Build the project ```dotnet build```
5. Run the project ```dotnet run --project Geometrix.WebApi/Geometrix.WebApi.csproj```

## Usage

To generate an image, make a POST request to `/api/GenerateImage` with the following parameters:

- `mirrorPowerHorizontal`: (integer) The mirror power in the horizontal direction.
- `mirrorPowerVertical`: (integer) The mirror power in the vertical direction.
- `cellGroupLength`: (integer) The length of the cell group.
- `cellWidthPixel`: (integer) The width of the cell in pixels.
- `includeEmptyAndFill`: (boolean) Whether to include empty and fill cells.
- `seed`: (integer) The seed for the random number generator.
- `backgroundColor`: (string) The background color in hexadecimal format.
- `foregroundColor`: (string) The foreground color in hexadecimal format.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
