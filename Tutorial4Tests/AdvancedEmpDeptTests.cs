namespace Tutorial3Tests;

public class AdvancedEmpDeptTests
{
    // 11.
    [Fact]
    public void ShouldReturnMaxSalary()
    {
        var emps = Database.GetEmps();
        decimal? maxSalary = emps.Max(e => e.Sal);
        Assert.Equal(5000, maxSalary);
    }

    // 12.
    [Fact]
    public void ShouldReturnMinSalaryInDept30()
    {
        var emps = Database.GetEmps();
         decimal? minSalary = emps
            .Where(e => e.DeptNo == 30)
            .Min(e => e.Sal);
        Assert.Equal(1250, minSalary);
    }

    // 13.
    [Fact]
    public void ShouldReturnFirstTwoHiredEmployees()
    {

         var emps = Database.GetEmps();
         var firstTwo = emps.OrderBy(e => e.HireDate).Take(2).ToList();
         Assert.Equal(2, firstTwo.Count);
         Assert.True(firstTwo[0].HireDate <= firstTwo[1].HireDate);

    }

    // 14.
    [Fact]
    public void ShouldReturnDistinctJobTitles()
    {
        var emps = Database.GetEmps();
        var jobs = emps.Select(e => e.Job).Distinct().ToList();
        Assert.Contains("PRESIDENT", jobs);
        Assert.Contains("SALESMAN", jobs);
    }

    [Fact]
    public void ShouldReturnEmployeesWithManagers()
    {
        var emps = Database.GetEmps();
        var withMgr = emps.Where(e => e.Mgr != null).ToList();
        Assert.All(withMgr, e => Assert.NotNull(e.Mgr));
    }

    // 16.
    [Fact]
    public void AllEmployeesShouldEarnMoreThan500()
    {
        var emps = Database.GetEmps();
        var result = emps.All(e => e.Sal > 500);
        Assert.True(result);
    }

    // 17.
    [Fact]
    public void ShouldFindAnyWithCommissionOver400()
    {
        var emps = Database.GetEmps();
        var result = emps.Any(e => e.Comm.HasValue && e.Comm > 400);
        Assert.True(result);
    }

    // 18.
    [Fact]
    public void ShouldReturnEmployeeManagerPairs()
    {
        var emps = Database.GetEmps();
        var result = (from e1 in emps
                      join e2 in emps on e1.Mgr equals e2.EmpNo
                      select new
                      {
                          Employee = e1.EName,
                          Manager = e2.EName
                      }).ToList();
        Assert.Contains(result, r => r.Employee == "SMITH" && r.Manager == "FORD");
    }

    // 19.
    [Fact]
    public void ShouldReturnTotalIncomeIncludingCommission()
    {
        var emps = Database.GetEmps();
        var result = emps.Select(e => new
        {
            e.EName,
            Total = e.Sal + e.Comm.GetValueOrDefault()
        }).ToList();

        Assert.Contains(result, r => r.EName == "ALLEN" && r.Total == 1900);
    }

    // 20.
    [Fact]
    public void ShouldJoinEmpDeptSalgrade()
    {
        var emps = Database.GetEmps();
        var depts = Database.GetDepts();
        var grades = Database.GetSalgrades();
        var result = (from e in emps
              join d in depts on e.DeptNo equals d.DeptNo
              from g in grades
              where e.Sal >= g.Losal && e.Sal <= g.Hisal
              select new
              {
                  e.EName,
                  DName = d.DName,
                  Grade = g.Grade
              }).ToList();
          Assert.Contains(result, r => r.EName == "ALLEN" && r.DName == "SALES" && r.Grade == 3);
    }
}
