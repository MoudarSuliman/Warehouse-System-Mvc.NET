# Warehouse-System-Mvc.NET

This ASP.NET Core MVC application serves as a basic example of managing movies and users, including functionalities such as user registration, login, product management, and more. It demonstrates the core concepts of MVC architecture, entity manipulation, and basic authentication.

## Features

- **User Management:** Allows user registration, login, and deletion.
- **Product Management:** Enables CRUD operations on movie products, including create, read, update, and delete functionalities.
- **Data Storage:** Utilizes JSON files (`user.json` and `product.json`) for persistence.
- **Validation:** Implements basic validation for user and product entities.

## Controllers Overview

- **HomeController:** Manages the main entry points of the application, including the home and privacy pages.
- **UserController:** Handles user-related actions, including login, registration, and deletion.
- **ProductController:** Responsible for product-related functionalities, including listing, creating, editing, and deleting products.
- **WelcomeController:** A simple controller to demonstrate additional, custom welcome functionality.

## Setup and Running

### Prerequisites

Ensure you have the following installed:
- [.NET 5.0 SDK](https://dotnet.microsoft.com/download) or higher
- An IDE that supports .NET development (e.g., [Visual Studio](https://visualstudio.microsoft.com/), [VS Code](https://code.visualstudio.com/))

### Steps to Run

1. **Clone the Repository**
   
   Clone the repository to your local machine using Git commands or download the ZIP directly from GitHub.

2. **Open the Project**

   Open the solution file (`MvcMovie.sln`) in your IDE.

3. **Restore Dependencies**

   Run the following command in the terminal or Package Manager Console to restore the NuGet packages:
   ```
   dotnet restore
   ```

4. **Run the Application**

   Start the application by running:
   ```
   dotnet run
   ```
   Or, use the IDE's built-in run/debug functionality.

5. **Access the Application**

   Once running, access the application through your web browser at the default URL: `http://localhost:5000/`.

## Additional Information

- **JSON Data Storage:** The application stores user and product data in JSON files. Ensure these files are accessible and have the correct permissions.
- **Model Validation:** The application uses basic model validation. Review and customize validation rules as per your requirements.
- **Error Handling:** A simple error view is configured for handling application errors gracefully.

## License

Specify the license under which this project is released, if applicable.

---

This README provides a comprehensive guide for setting up and navigating through your MVC Movie application. Adjust the content as necessary to match your project's specifics or additional details you wish to include.
