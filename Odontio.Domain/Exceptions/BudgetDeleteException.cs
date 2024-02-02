namespace Odontio.Domain.Exceptions;

public class BudgetDeleteException: Exception
{
    private const string DefaultMessage = "No se puede eliminar un presupuesto que tiene citas o pagos asociados.";
    
    public BudgetDeleteException(string message): base(message)
    {
    }

    public BudgetDeleteException(): base(DefaultMessage)
    {
    }
}