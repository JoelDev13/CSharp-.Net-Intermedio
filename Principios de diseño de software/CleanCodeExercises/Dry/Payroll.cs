
namespace Dry
{
    public class Payroll
    {
        private decimal _bonusPorcentage = 0.05m;
        private decimal _taxPorcentage = 0.18m;


        public decimal CalculateSalaryForFullTime(decimal baseSalary)
            => baseSalary - CalculateTax(baseSalary) + CalculateBonus(baseSalary);


        public decimal CalculateSalaryForPartTime(decimal hourlyRate, int hoursWorked)
        {
            decimal salary = hourlyRate * hoursWorked;
            return salary - CalculateTax(salary) + CalculateBonus(salary);
        }

        public decimal CalculateTax(decimal salary)
            => salary * _taxPorcentage;

        public decimal CalculateBonus(decimal salary)
            => salary * _bonusPorcentage;
    }
}
