# FooBooLoo

This project is implementation of the FizzBuzz-like Game, allowing users to create custom FizzBuzz games, start sessions, and play the game while interacting with the back-end API. The front-end is built using React with TypeScript and integrates with a .NET 8 Web API. This project is portable and deployed by using Docker.

## Table of Contents

- [Installation & Setup](#installation-&-setup)
- [Running the Development Server](#running-the-development-server)
- [Running in Docker Environment](#running-in-docker-environment)
- [Running Tests](#running-tests)

## Installation & Setup

To set up the project, follow these steps:

1. Clone the repository:
    ```bash
    git clone https://github.com/AndyTranPro/FooBooLoo.git
    cd FooBooLoo
    ```

2. Install all the necessary packages in the frontend directory:
    ```bash
    cd frontend
    npm install
    ```
3. Modify **Username** and **Password** in the **appsettings.Development.json** file located in the **backend/FooBooLooGameAPI** directory if necessary to ensure that they match your local PostgreSQL environment.

### For Windows:

1. **Download Docker Desktop:**
   - Visit the [Docker Desktop for Windows download page](https://www.docker.com/products/docker-desktop/) and download the installer.
   - Run the installer and follow the on-screen instructions.

2. **Install Docker Desktop:**
   - During installation, ensure the option to "Use WSL 2 instead of Hyper-V" is selected if you plan to use Windows Subsystem for Linux (WSL 2). Otherwise, Docker will use Hyper-V.
   - After installation, Docker Desktop will automatically start. You can access it from the system tray.

3. **Set Up WSL 2 (Optional):**
   - If using WSL 2, ensure it's enabled on your system.
   - Install a Linux distribution from the Microsoft Store (like Ubuntu) to use with WSL 2.

4. **Verify Installation:**
   - Open a terminal (Command Prompt, PowerShell, or WSL terminal).
   - Run the following command to verify that Docker is installed correctly:
     ```bash
     docker --version
     ```

### For macOS:

1. **Download Docker Desktop:**
   - Visit the [Docker Desktop for Mac download page](https://www.docker.com/products/docker-desktop/) and download the installer.
   - Open the `.dmg` file and drag the Docker icon to your Applications folder.

2. **Install Docker Desktop:**
   - Launch Docker Desktop from the Applications folder.
   - Follow the on-screen instructions to complete the installation.

3. **Verify Installation:**
   - Open the Terminal application.
   - Run the following command to verify that Docker is installed correctly:
     ```bash
     docker --version
     ```

### For Linux:

1. **Install Docker Engine:**
   - Open a terminal and run the following commands:
     ```bash
     sudo apt-get update
     sudo apt-get install ca-certificates curl gnupg
     sudo mkdir -m 0755 -p /etc/apt/keyrings
     curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
     echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
     sudo apt-get update
     sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
     ```

2. **Run Docker Commands Without `sudo` (Optional):**
   - To avoid using `sudo` with Docker commands, add your user to the Docker group:
     ```bash
     sudo usermod -aG docker $USER
     ```
   - Log out and log back in to apply the group change.

3. **Verify Installation:**
   - Run the following command to verify that Docker is installed correctly:
     ```bash
     docker --version
     ```

## Running the Development Server

To start the development server and run the application locally, you need to open two terminals for both **backend** and **frontend** directories:
1. **To run the backend server:**

   ```bash
   cd ./backend/FooBooLooGameAPI/
   dotnet run
   ```
2. **To run the frontend server:**
   ```bash
   cd ./frontend
   npm run dev
   ```
3. Modify **Username** and **Password** in the **appsettings.Development.json** file located in the **backend/FooBooLooGameAPI** directory if necessary to ensure that they match your local PostgreSQL environment.

## Running in Docker Environment

To run the application inside Docker container, you need to open a terminal at the root directory:
1. **To dockerize the application:**

   ```bash
   docker-compose up
   ```
   After running this command, Docker starts to create essential containers for all the services (database, backend, and frontend).
2. **To access the application:**

   You can type **http://localhost:3000** which is the frontend server on your browser to interact with the application via the frontend interface.

3. **To stop the docker container:**
   
   You can just simply **crl + C** in the terminal where you run the **docker-compose up** command.

## Running Tests

1. **To run all backend tests:**

   ```bash
   cd ./backend/FooBooLooGameAPI.Tests/
   dotnet test ./FooBooLooGameAPI.Tests.csproj
   ```
2. **To run all frontend tests:**

   ```bash
   cd ./frontend
   npm test
   ```
## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

This project has the [MIT](https://choosealicense.com/licenses/mit/) license.