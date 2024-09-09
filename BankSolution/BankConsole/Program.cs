using System.Text.RegularExpressions;
using BankConsole;

if(args.Length == 0)
{
    EmailService.SendMail();
} else 
{
    ShowMenu();
}

void ShowMenu()
{
    Console.Clear();
    Console.WriteLine("Selecciona una opción:");
    Console.WriteLine("1 - Crear un usuario nuevo.");
    Console.WriteLine("2 - Eliminar un usuario existente.");
    Console.WriteLine("3 - Salir.");

    int option = 0;
    do
    {
        string input = Console.ReadLine();
        if (!int.TryParse(input, out option))
            Console.WriteLine("Debes ingresar un número (1, 2 o 3).");
        else if (option > 3)
            Console.WriteLine("Debes ingresar un número válido (1, 2 o 3).");
    }
    while (option == 0 || option > 3);

    switch (option)
    {
        case 1:
            CreateUser();
        break;
        case 2:
            DeleteUser();
        break;
        case 3:
            Environment.Exit(0);
        break;
    }
}

void CreateUser()
{
    Console.Clear();
    Console.WriteLine("Ingresa la información del usuario:");

    int id = 0;
    bool isOk = false;

    while(!isOk)
    {
        Console.WriteLine("ID: ");
        if (int.TryParse(Console.ReadLine(), out id) && id > 0)
        {
            if (Storage.SearchID(id))
            {
                Console.WriteLine("El ID ingresado ya existe, prueba con otro valor.");
                isOk = false;
            } else
                isOk = true;
        }else 
            Console.WriteLine("El valor ingresado no es correcto, prueba de nuevo.");
    }

    Console.WriteLine("Nombre: ");
    string name = Console.ReadLine();

    bool isOkEmail = false;
    string email = "";

    Regex regex = new Regex(@"^[a-zA-Z]+@[a-zA-Z]+\.[a-zA-Z]{2,3}$");
    while(!isOkEmail)
    {
        Console.WriteLine("Email: ");
        email = Console.ReadLine();
        if (regex.IsMatch(email))
            isOkEmail = true;
        else
            Console.WriteLine("La cadena ingresada no es correcta, prueba de nuevo.");
    }



    decimal balance = 0;
    bool isOkBalance = false;

    while(!isOkBalance)
    {
        Console.WriteLine("Saldo");
        if (decimal.TryParse(Console.ReadLine(), out balance) && balance > 0)
        {
            isOkBalance = true;
        } else
            Console.WriteLine("El valor ingresado no es correcto, prueba de nuevo.");
    }

    char userType;
    bool isOkUserType = false;

    while(!isOkUserType)
    {
        Console.WriteLine("Escribe 'c' si el usuario es Cliente, 'e' si es Empleado: ");
        if (char.TryParse(Console.ReadLine(), out userType) && (userType == 'c' || userType == 'e'))
        {
            isOkUserType = true;
            
            User newUser;

            if (userType.Equals('c'))
            {
                Console.WriteLine("Regimen Fiscal: ");
                char TaxRegime = char.Parse(Console.ReadLine());

                newUser = new Client(id, name, email, balance, TaxRegime);
            }
            else
            {
                Console.WriteLine("Departamento: ");
                string department = Console.ReadLine();

                newUser = new Employee(id, name, email, balance, department);
            }

            Storage.AddUser(newUser);
            Console.WriteLine("Usuario creado.");
            Thread.Sleep(2000);
            ShowMenu();
        } else 
            Console.WriteLine("El valor ingresado no es correcto, prueba de nuevo.");
    }   
}

void DeleteUser()
{
    Console.Clear();

    int id = 0;
    bool isOk = false;

    while(!isOk)
    {
        Console.Write("Ingresa el ID del usuario a eliminar: ");
        if (int.TryParse(Console.ReadLine(), out id) && id > 0)
        {
            if (Storage.SearchID(id))
            {
                isOk = true;
                string result = Storage.DeleteUser(id);

                if (result.Equals("Success"))
                {
                    Console.Write("Usuario eliminado.");
                    Thread.Sleep(2000);
                    ShowMenu();
                }
            } else
                Console.WriteLine("El ID ingresado no existe, prueba con otro valor.");
                isOk = false;
        }else 
            Console.WriteLine("El valor ingresado no es correcto, prueba de nuevo.");
    }
}


