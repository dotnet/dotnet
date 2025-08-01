try
{
    WebApplication.Create();

    Console.WriteLine("Runtime validation success.");
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine("Runtime validation failure.");
    Console.WriteLine(ex.Message);
    return 1;
}