using Tutorial3.Models;

public class EmpDeptSalgradeTests
{
    // 1.
    [Fact]
    public void ShouldReturnAllSalesmen()
    {
        var emps = Database.GetEmps();
        List<Emp> result = emps.Where(e => e.Job == "SALESMAN").ToList();
        Assert.Equal(2, result.Count);
        Assert.All(result, e => Assert.Equal("SALESMAN", e.Job));
    }

    // 2.
    [Fact]
    public void ShouldReturnDept30EmpsOrderedBySalaryDesc()
    {
        var emps = Database.GetEmps();
        List<Emp> result = emps.Where(e => e.DeptNo == 30).OrderByDescending(e => e.Sal).ToList();
        Assert.Equal(2, result.Count);
        Assert.True(result[0].Sal >= result[1].Sal);
    }

    // 3.
    [Fact]
    public void ShouldReturnEmployeesFromChicago()
    {
        var emps = Database.GetEmps();
        var depts = Database.GetDepts();
        var chicagoDeptNos = depts
        .Where(d => d.Loc == "CHICAGO")
        .Select(d => d.DeptNo);
        List<Emp> result = emps
        .Where(e => chicagoDeptNos.Contains(e.DeptNo))
        .ToList();
        Assert.All(result, e => Assert.Equal(30, e.DeptNo));
    }

    // 4.
    [Fact]
    public void ShouldSelectNamesAndSalaries()
    {
        var emps = Database.GetEmps();
        var result = emps
        .Select(e => new { e.EName, e.Sal })
        .ToList();
            Assert.All(result, r =>
        {
            Assert.False(string.IsNullOrWhiteSpace(r.EName));
            Assert.True(r.Sal > 0);
        });
    }

    // 5.
    [Fact]
    public void ShouldJoinEmployeesWithDepartments()
    {
        var emps = Database.GetEmps();
        var depts = Database.GetDepts();
        var result = emps.Join(depts,
            emp => emp.DeptNo,
            dept => dept.DeptNo,
            (emp, dept) => new
            {
                emp.EName,
                dept.DName
            });
            Assert.Contains(result, r => r.DName == "SALES" && r.EName == "ALLEN");
    }

    // 6.
    [Fact]
    public void ShouldCountEmployeesPerDepartment()
    {
        var emps = Database.GetEmps();
        var result = emps
        .GroupBy(e => e.DeptNo)
        .Select(g => new { DeptNo = g.Key, Count = g.Count() })
        .ToList();
        Assert.Contains(result, g => g.DeptNo == 30 && g.Count == 2);
    }

    // 7.
    [Fact]
    public void ShouldReturnEmployeesWithCommission()
    {
        var emps = Database.GetEmps();
        var result = emps
        .Where(e => e.Comm != null)
        .ToList();
        Assert.All(result, r => Assert.NotNull(r.Comm));
    }

    // 8.
    [Fact]
    public void ShouldMatchEmployeeToSalaryGrade()
    {
        var emps = Database.GetEmps();
        var grades = Database.GetSalgrades();
         var result = (from emp in emps
                  from grade in grades
                  where emp.Sal >= grade.Losal && emp.Sal <= grade.Hisal
                  select new { emp.EName, grade.Grade }).ToList();
        Assert.Contains(result, r => r.EName == "ALLEN" && r.Grade == 3);
    }

    // 9.
    [Fact]
    public void ShouldCalculateAverageSalaryPerDept()
    {
        var emps = Database.GetEmps();
        var result = emps
        .GroupBy(e => e.DeptNo)
        .Select(g => new { DeptNo = g.Key, AvgSal = g.Average(e => e.Sal) })
        .ToList();
        Assert.Contains(result, r => r.DeptNo == 30 && r.AvgSal > 1000);
    }

    // 10.
    [Fact]
    public void ShouldReturnEmployeesEarningMoreThanDeptAverage()
    {
        var emps = Database.GetEmps();
         var result = emps
        .GroupBy(e => e.DeptNo)
        .SelectMany(group =>
            group.Where(e => e.Sal > group.Average(x => x.Sal))
        )
        .Select(e => e.EName)
        .ToList();
        Assert.Contains("ALLEN", result);
    }
}
