using System;
using System.Collections.Generic;
using System.Linq;
using GradeBook.Enums;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
      public RankedGradeBook(string name) : base(name)
      {
        this.Type = GradeBookType.Ranked;
      }

      public override char GetLetterGrade(double averageGrade)
      {
        if (this.Students.Count < 5)
        {
          throw new InvalidOperationException();
        }

        var gradeThreshold = this.Students.Count / 5;
        var gradesList = new List<double>();

        foreach (var student in this.Students)
        {
          foreach (var grade in student.Grades)
          {
            gradesList.Add(grade);
          }
        }

        var query = (from grades in gradesList
                      orderby grades descending
                      select grades).ToList();

        switch (averageGrade)
        {
          case var grade when grade >= GetLowestGrade(query.Take(gradeThreshold)):
            return 'A';
          case var grade when grade >= GetLowestGrade(query.Take(gradeThreshold * 2)):
            return 'B';
          case var grade when grade >= GetLowestGrade(query.Take(gradeThreshold * 3)):
            return 'C';
          case var grade when grade >= GetLowestGrade(query.Take(gradeThreshold * 4)):
            return 'D';
          default:
            return 'F';
        }
      }

      private double GetLowestGrade(IEnumerable<double> grades)
      {
        var lowestGrade = from grade in grades
                          orderby grade
                          select grade;
        return lowestGrade.Take(1).First();
      }
    }
}