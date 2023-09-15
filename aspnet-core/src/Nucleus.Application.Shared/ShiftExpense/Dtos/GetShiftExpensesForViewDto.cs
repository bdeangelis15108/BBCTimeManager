namespace Nucleus.ShiftExpense.Dtos
{
    public class GetShiftExpensesForViewDto
    {
		public ShiftExpensesDto ShiftExpenses { get; set; }

		public string ShiftResourcesName { get; set;}

		public string ExpenseTypesName { get; set;}


    }
}